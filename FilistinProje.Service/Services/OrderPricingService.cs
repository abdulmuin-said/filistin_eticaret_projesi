using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilistinProje.Core.DTOs;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FilistinProje.Service.Services
{
    /// <summary>
    /// Sipariş fiyat/stok doğrulaması için server-side tek hesaplama yolu (B3).
    /// SepetItem.Fiyat (snapshot) hiçbir koşulda siparişe yazılmaz.
    /// </summary>
    public class OrderPricingService : IOrderPricingService
    {
        private readonly KanvasDbContext _context;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IKargoHesaplamaServisi _kargoHesaplama;
        private readonly ILogger<OrderPricingService> _logger;

        public OrderPricingService(
            KanvasDbContext context,
            ISiteSettingsService siteSettingsService,
            IKargoHesaplamaServisi kargoHesaplama,
            ILogger<OrderPricingService> logger)
        {
            _context = context;
            _siteSettingsService = siteSettingsService;
            _kargoHesaplama = kargoHesaplama;
            _logger = logger;
        }

        public async Task<OrderPricingResult> HesaplaAsync(
            IReadOnlyList<SepetItem> sepetItems,
            string? sehir,
            string odemeYontemi,
            bool isWholesale,
            string? kuponKodu)
        {
            var result = new OrderPricingResult();

            if (sepetItems == null || sepetItems.Count == 0)
            {
                return result;
            }

            // Tüm ilgili ürün ve seçenekleri TEK sorguda çek — N+1 yok.
            var urunIds = sepetItems.Select(i => i.UrunId).Distinct().ToList();
            var urunSecenekIds = sepetItems
                .Where(i => i.UrunSecenekId.HasValue)
                .Select(i => i.UrunSecenekId!.Value)
                .Distinct()
                .ToList();

            var urunler = await _context.Urunler
                .AsNoTracking()
                .Include(u => u.UrunSecenek)
                .Where(u => urunIds.Contains(u.Id) && !u.SilindiMi)
                .ToListAsync();

            var urunById = urunler.ToDictionary(u => u.Id);
            var secenekById = urunler
                .SelectMany(u => u.UrunSecenek.Where(s => !s.SilindiMi))
                .ToDictionary(s => s.Id);

            // Toptancı iskonto kodu: ürün → min adet → iskonto yüzde. (LR cache)
            Dictionary<int, List<ToptanciIskontoOrani>> toptanciGrupIskonto = new();
            var activeGroupIds = urunler
                .Where(u => u.ToptanciUrunGrubuId.HasValue)
                .Select(u => u.ToptanciUrunGrubuId!.Value)
                .Distinct()
                .ToList();

            if (isWholesale && activeGroupIds.Count > 0)
            {
                var oranlar = await _context.ToptanciIskontoOranlari
                    .AsNoTracking()
                    .Where(o => activeGroupIds.Contains(o.ToptanciUrunGrubuId)
                        && o.AktifMi
                        && !o.SilindiMi)
                    .OrderBy(o => o.MinAdet)
                    .ToListAsync();
                toptanciGrupIskonto = oranlar
                    .GroupBy(o => o.ToptanciUrunGrubuId)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }

            var settings = _siteSettingsService.GetSettings();

            // Sepet satırlarını grupla — aynı varyant birden fazla satırda olabilir.
            var grouped = sepetItems
                .GroupBy(i => new { i.UrunId, i.UrunSecenekId, Cerceve = (i.CerceveModeli ?? string.Empty), i.HediyePaketi })
                .ToList();

            foreach (var grp in grouped)
            {
                var ornek = grp.First();
                var adetToplam = grp.Sum(i => i.Adet);
                var sepetItemId = ornek.Id;

                if (!urunById.TryGetValue(ornek.UrunId, out var urun))
                {
                    var shortage = new StockShortageEntry
                    {
                        SepetItemId = sepetItemId,
                        UrunBaslik = ornek.UrunBaslik,
                        UrunSecenekId = ornek.UrunSecenekId,
                        IstenenAdet = adetToplam,
                        MevcutStok = 0,
                    };
                    result.StokYetersizlikleri.Add(shortage);
                    continue;
                }

                var secenek = ornek.UrunSecenekId.HasValue && secenekById.TryGetValue(ornek.UrunSecenekId.Value, out var s)
                    ? s
                    : null;

                // Fiyat kaynağı — yalnızca DB. SepetItem.Fiyat değil.
                decimal birimFiyat = HesaplaBirimFiyat(urun, secenek, isWholesale, adetToplam, toptanciGrupIskonto);
                decimal cerceveFark = HesaplaCerceveFarki(secenek, ornek.CerceveModeli);

                decimal hediyeBirim = 0m;
                bool hediyePaketi = ornek.HediyePaketi && urun.HediyePaketiVarMi;
                if (hediyePaketi)
                {
                    hediyeBirim = urun.HediyePaketFiyati;
                }

                decimal birimToplam = birimFiyat + cerceveFark;
                decimal satirToplam = (birimToplam * adetToplam) + (hediyeBirim * adetToplam);

                var line = new OrderLinePricing
                {
                    SepetItemId = sepetItemId,
                    UrunId = urun.Id,
                    UrunSecenekId = ornek.UrunSecenekId,
                    BirimFiyat = birimFiyat + cerceveFark, // tek fiyat olarak sakla, çerçeve farkı entegre
                    HediyePaketBirim = hediyeBirim,
                    HediyePaketi = hediyePaketi,
                    Adet = adetToplam,
                    SatirToplam = satirToplam,
                    OncekiSepetFiyat = ornek.Fiyat,
                };

                if (line.FiyatDegistiMi)
                {
                    result.FiyatDegisiklikleri.Add(new PriceChangedEntry
                    {
                        SepetItemId = sepetItemId,
                        UrunBaslik = ornek.UrunBaslik,
                        EskiFiyat = ornek.Fiyat,
                        YeniFiyat = line.BirimFiyat,
                    });
                }

                // Stok kontrolü: varyant varsa, sipariş adedi mevcut stok içinde olmalı.
                // OnSipariseAcikMi -> bu kontrolü atla (backorder).
                if (secenek != null && !secenek.OnSipariseAcikMi)
                {
                    if (secenek.StokAdedi < adetToplam)
                    {
                        line.StokSorunu = $"Stok yetersiz: {ornek.UrunBaslik} — istenen {adetToplam}, mevcut {secenek.StokAdedi}";
                        result.StokYetersizlikleri.Add(new StockShortageEntry
                        {
                            SepetItemId = sepetItemId,
                            UrunBaslik = ornek.UrunBaslik,
                            UrunSecenekId = ornek.UrunSecenekId,
                            IstenenAdet = adetToplam,
                            MevcutStok = secenek.StokAdedi,
                        });
                    }
                }

                result.Satirlar.Add(line);
            }

            // B13: Hediye paketi bedeli SepetItem.Toplam'a zaten dahildir
            // (SepetItem.Toplam = Fiyat*Adet + HediyePaketi ? HediyePaket*Adet : 0).
            // Bu nedenle AraToplam'a ek bir hediye paketi eklemesi yapılmaz.
            result.AraToplam = result.Satirlar.Sum(l => l.SatirToplam);

            // Kupon
            if (!string.IsNullOrWhiteSpace(kuponKodu))
            {
                var kupon = await _context.Kuponlar
                    .AsNoTracking()
                    .FirstOrDefaultAsync(k => k.Kod == kuponKodu && !k.SilindiMi);

                if (KuponGeçerliMi(kupon, result.AraToplam))
                {
                    result.IndirimTutari = CalculateCouponDiscount(kupon!, result.AraToplam);
                    result.UygulananKuponKodu = kupon!.Kod;
                }
            }

            // COD bedeli
            decimal sepetToplamiIndirimli = result.AraToplam - result.IndirimTutari;

            result.KapidaOdemeHizmetBedeli = 0;
            if (odemeYontemi == "KapidaOdeme"
                && settings.KapidaOdemeAktifMi
                && sepetToplamiIndirimli <= settings.KapidaOdemeLimiti)
            {
                result.KapidaOdemeHizmetBedeli = settings.KapidaOdemeHizmetBedeli;
            }

            // Kargo — şehir boşsa (mağazadan teslim), 0.
            bool magazadanTeslim = string.IsNullOrWhiteSpace(sehir);
            if (magazadanTeslim)
            {
                result.KargoUcreti = 0;
            }
            else
            {
                result.KargoUcreti = await _kargoHesaplama.HesaplaAsync(
                    sehir!,
                    sepetToplamiIndirimli,
                    settings.UcretsizKargoLimiti);
            }

            return result;
        }

        /// <summary>
        /// Transaction içinde çağrılır. UrunSecenekId varsa atomik stok düşümü:
        /// UPDATE UrunSecenekleri SET StokAdedi = StokAdedi - @adet WHERE Id = @id AND StokAdedi >= @adet
        /// AffectedRows < 1 ise stok yetersiz → result başarısız.
        /// </summary>
        public async Task<StockDeductionResult> StokDusAsync(List<OrderLinePricing> satirlar)
        {
            foreach (var satir in satirlar)
            {
                if (!satir.UrunSecenekId.HasValue)
                {
                    continue;
                }

                if (satir.Adet <= 0)
                {
                    return new StockDeductionResult
                    {
                        Basarili = false,
                        BasarisizUrunSecenekId = satir.UrunSecenekId,
                        HataMesaji = $"Geçersiz adet: {satir.Adet}",
                    };
                }

                var secenekId = satir.UrunSecenekId.Value;
                var adet = satir.Adet;

                // Koşullu atomic update: WHERE StokAdedi >= @adet
                var affected = await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"UPDATE ""UrunSecenekleri"" SET ""StokAdedi"" = ""StokAdedi"" - {adet} WHERE ""Id"" = {secenekId} AND ""StokAdedi"" >= {adet}");

                if (affected != 1)
                {
                    _logger.LogWarning(
                        "Atomic stok düşüm başarısız. UrunSecenekId={SecenekId}, IstenenAdet={Adet}, Affected={Affected}",
                        secenekId, adet, affected);

                    return new StockDeductionResult
                    {
                        Basarili = false,
                        BasarisizUrunSecenekId = secenekId,
                        HataMesaji = "Stok yetersiz veya ürün başka bir siparişle tükendi.",
                    };
                }
            }

            return new StockDeductionResult { Basarili = true };
        }

        /// <summary>
        /// Kupon indirim tutarı. Over-purchase'ı önlemek için tutarı min(tutar, max(0, discount)) ile sınırla.
        /// </summary>
        public decimal CalculateCouponDiscount(Kupon kupon, decimal sepetTutari)
        {
            if (kupon == null)
            {
                return 0m;
            }

            var discount = kupon.Tip == 0
                ? sepetTutari * (kupon.Deger / 100m)
                : kupon.Deger;

            return System.Math.Round(System.Math.Min(sepetTutari, System.Math.Max(0m, discount)), 2);
        }

        private static bool KuponGeçerliMi(Kupon? kupon, decimal sepetTutari)
        {
            return kupon != null
                && kupon.AktifMi
                && kupon.SonKullanmaTarihi > System.DateTime.UtcNow
                && (kupon.KullanimLimiti <= 0 || kupon.KullanilanMiktar < kupon.KullanimLimiti)
                && sepetTutari >= kupon.MinSepetTutari;
        }

        /// <summary>
        /// Birim fiyat hesabı:
        /// - Toptancı: Urun.EtkinTopFiyat (TopFiyat > 0 ise → eğer 0 ise EtkinFiyat fallback).
        ///   + ToptanciIskontoOrani (min adet >= satir adedi olan en yüksek iskonto).
        /// - Normal: UrunSecenek.SatisFiyat > 0 ise onu kullan; değilse Urun.EtkinFiyat.
        /// </summary>
        private static decimal HesaplaBirimFiyat(
            Urun urun,
            UrunSecenek? secenek,
            bool isWholesale,
            int adet,
            Dictionary<int, List<ToptanciIskontoOrani>> toptanciGrupIskonto)
        {
            decimal birimFiyatBase;

            if (secenek != null && secenek.SatisFiyati > 0)
            {
                birimFiyatBase = secenek.SatisFiyati;
            }
            else if (isWholesale)
            {
                birimFiyatBase = urun.EtkinTopFiyat;
            }
            else
            {
                birimFiyatBase = urun.EtkinFiyat;
            }

            // Toptancı ek iskonto
            if (isWholesale && urun.ToptanciUrunGrubuId.HasValue
                && toptanciGrupIskonto.TryGetValue(urun.ToptanciUrunGrubuId.Value, out var oranlar)
                && oranlar.Count > 0)
            {
                decimal enYuksekIskonto = 0m;
                foreach (var oran in oranlar)
                {
                    if (adet >= oran.MinAdet && oran.IskontoYuzdesi > enYuksekIskonto)
                    {
                        enYuksekIskonto = oran.IskontoYuzdesi;
                    }
                }

                if (enYuksekIskonto > 0)
                {
                    birimFiyatBase = birimFiyatBase * (1m - enYuksekIskonto / 100m);
                }
            }

            return System.Math.Round(birimFiyatBase, 2);
        }

        /// <summary>
        /// Çerçeve farkı hesabı: SepetService.CalculateFramePrice ile aynı mantık.
        /// Burada tekrar uygulanır, çünkü SepetItem.Fiyat snapshot'ına güvenmiyoruz.
        /// </summary>
        private static decimal HesaplaCerceveFarki(UrunSecenek? secenek, string? cerceveModeli)
        {
            if (secenek == null || string.IsNullOrWhiteSpace(cerceveModeli) || cerceveModeli == "Çerçevesiz")
            {
                return 0m;
            }

            var dimensions = ParseDimensions(secenek.Olcu);
            if (dimensions == null)
            {
                return 0m;
            }

            const decimal framePricePerMeter = 250m;
            var perimeterMeters = ((dimensions.Value.Width + dimensions.Value.Height) * 2m) / 100m;
            return System.Math.Round(perimeterMeters * framePricePerMeter, 2);
        }

        private static (decimal Width, decimal Height)? ParseDimensions(string? olcu)
        {
            if (string.IsNullOrWhiteSpace(olcu))
            {
                return null;
            }

            var parts = olcu.Split(new[] { 'x', 'X', '×' }, System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                return null;
            }

            if (!decimal.TryParse(parts[0].Replace(',', '.'), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var width) ||
                !decimal.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var height) ||
                width <= 0 || height <= 0)
            {
                return null;
            }

            return (width, height);
        }
    }
}
