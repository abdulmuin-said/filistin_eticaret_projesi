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
                .Include(u => u.HediyePaketSecenekleri)
                .Include(u => u.ToptanFiyatKademeleri)
                .AsSplitQuery()
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

            var settings = _siteSettingsService?.GetSettings() ?? new FilistinProje.Core.Models.SiteAyarlari();

            // Sepet satırlarını grupla — aynı varyant birden fazla satırda olabilir.
            var grouped = sepetItems
                .GroupBy(i => new { i.UrunId, i.UrunSecenekId, Cerceve = (i.CerceveModeli ?? string.Empty), i.HediyePaketSecenegiId })
                .ToList();

            foreach (var grp in grouped)
            {
                var ornek = grp.First();
                var adetToplam = grp.Sum(i => i.Adet);
                var sepetItemId = ornek.Id;

                if (!urunById.TryGetValue(ornek.UrunId, out var urun) || !urun.AktifMi || urun.SilindiMi || urun.WhatsappSiparisVarMi || urun.FiyatGizliMi)
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

                if (secenek != null && (!secenek.AktifMi || secenek.SilindiMi || !secenek.SatinAlinabilirMi))
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

                // Min / Max siparis adedi doğrulaması (POST-AUDIT-001)
                if (urun.MinSiparisAdedi > 0 && adetToplam < urun.MinSiparisAdedi)
                {
                    result.LimitAsimlari.Add($"MinSiparisAdediNotMet:{ornek.UrunBaslik}:{urun.MinSiparisAdedi}");
                }
                if (urun.MaxSiparisAdedi.HasValue && urun.MaxSiparisAdedi.Value > 0 && adetToplam > urun.MaxSiparisAdedi.Value)
                {
                    result.LimitAsimlari.Add($"MaxSiparisAdediExceeded:{ornek.UrunBaslik}:{urun.MaxSiparisAdedi.Value}");
                }

                // Fiyat kaynağı — yalnızca DB. SepetItem.Fiyat değil.
                decimal birimFiyat = HesaplaBirimFiyat(urun, secenek, isWholesale, adetToplam, toptanciGrupIskonto);
                decimal cerceveFark = HesaplaCerceveFarki(secenek, ornek.CerceveModeli);

                UrunHediyePaketSecenegi? hediyePaketSecenegi = null;
                if (ornek.HediyePaketSecenegiId.HasValue)
                {
                    hediyePaketSecenegi = urun.HediyePaketSecenekleri.FirstOrDefault(x =>
                        x.Id == ornek.HediyePaketSecenegiId.Value &&
                        x.AktifMi &&
                        !x.SilindiMi);
                    if (hediyePaketSecenegi == null)
                    {
                        result.GecersizHediyePaketSecenegiIds.Add(ornek.HediyePaketSecenegiId.Value);
                        continue;
                    }
                }

                decimal hediyeBirim = hediyePaketSecenegi?.Fiyat ?? 0;
                bool hediyePaketi = hediyePaketSecenegi != null;
                decimal birimToplam = birimFiyat + cerceveFark;
                decimal satirToplam = (birimToplam * adetToplam) + (hediyeBirim * adetToplam);

                var line = new OrderLinePricing
                {
                    SepetItemId = sepetItemId,
                    UrunId = urun.Id,
                    UrunSecenekId = ornek.UrunSecenekId,
                    BirimFiyat = birimFiyat + cerceveFark, // tek fiyat olarak sakla, çerçeve farkı entegre
                    HediyePaketSecenegiId = hediyePaketSecenegi?.Id,
                    HediyePaketBirim = hediyeBirim,
                    HediyePaketi = hediyePaketi,
                    HediyePaketAdi = hediyePaketSecenegi?.Ad ?? string.Empty,
                    HediyePaketAdiEn = hediyePaketSecenegi?.AdEn ?? string.Empty,
                    HediyePaketAdiAr = hediyePaketSecenegi?.AdAr ?? string.Empty,
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
                        line.StokSorunu = $"Insufficient stock: {ornek.UrunBaslik} — requested {adetToplam}, available {secenek.StokAdedi}";
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
                else if (secenek == null && urun.ToplamStok > 0 && urun.ToplamStok < adetToplam)
                {
                    line.StokSorunu = $"Insufficient product stock: {ornek.UrunBaslik} — requested {adetToplam}, available {urun.ToplamStok}";
                    result.StokYetersizlikleri.Add(new StockShortageEntry
                    {
                        SepetItemId = sepetItemId,
                        UrunBaslik = ornek.UrunBaslik,
                        UrunSecenekId = null,
                        IstenenAdet = adetToplam,
                        MevcutStok = urun.ToplamStok,
                    });
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
                        HataMesaji = $"Invalid quantity: {satir.Adet}",
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
                        HataMesaji = "Insufficient stock or product depleted by another order.",
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
                && (!kupon.BaslangicTarihi.HasValue || kupon.BaslangicTarihi <= System.DateTime.UtcNow)
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
            if (isWholesale)
            {
                var directTier = WholesaleTierResolver.Resolve(
                    urun.ToptanFiyatKademeleri,
                    secenek?.Id,
                    adet);
                if (directTier != null)
                {
                    return System.Math.Round(directTier.BirimFiyat, 2);
                }
            }

            decimal birimFiyatBase;

            if (secenek != null && secenek.SatisFiyati > 0)
            {
                birimFiyatBase = secenek.EtkinFiyat;
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
            // Frame price calculation is disabled per business directive.
            return 0m;
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
