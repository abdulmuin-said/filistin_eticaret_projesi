using System.Data;
using FilistinProje.Core.DTOs;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FilistinProje.Service.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly KanvasDbContext _context;
        private readonly IOrderPricingService _orderPricingService;
        private readonly ISepetService _sepetService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IKargoHesaplamaServisi _kargoHesaplama;
        private readonly ILogger<PurchaseOrderService> _logger;

        public PurchaseOrderService(
            KanvasDbContext context,
            IOrderPricingService orderPricingService,
            ISepetService sepetService,
            ISiteSettingsService siteSettingsService,
            IKargoHesaplamaServisi kargoHesaplama,
            ILogger<PurchaseOrderService> logger)
        {
            _context = context;
            _orderPricingService = orderPricingService;
            _sepetService = sepetService;
            _siteSettingsService = siteSettingsService;
            _kargoHesaplama = kargoHesaplama;
            _logger = logger;
        }

        public async Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequest request)
        {
            if (request.SepetItems.Count == 0)
            {
                return BusinessError("Siparis_OrderFailed");
            }

            var dto = request.Checkout;
            var sehirForPricing = dto.TeslimatTipi == "MagazadanTeslim" ? null : dto.Sehir;
            var odemeForPricing = dto.OdemeYontemi == "KapidaOdeme" ? "KapidaOdeme" : "BankaHavalesi";

            var pricing = await _orderPricingService.HesaplaAsync(
                request.SepetItems,
                sehirForPricing,
                odemeForPricing,
                request.IsWholesale,
                request.KuponKodu);

            // Store pickup has neither shipping nor cash-on-delivery handling costs.
            if (dto.TeslimatTipi == "MagazadanTeslim")
            {
                pricing.KargoUcreti = 0;
                pricing.KapidaOdemeHizmetBedeli = 0;
            }

            if (pricing.StokSorunuVar)
            {
                return new PlaceOrderResult
                {
                    Status = PlaceOrderStatus.StockShortage,
                    Pricing = pricing,
                    StockShortages = pricing.StokYetersizlikleri,
                    MessageKey = "Siparis_StockShortageGeneric"
                };
            }

            if (pricing.LimitSorunuVar)
            {
                var firstLimit = pricing.LimitAsimlari.FirstOrDefault() ?? string.Empty;
                var isMaxExceeded = firstLimit.StartsWith("MaxSiparisAdediExceeded");
                var messageKey = isMaxExceeded ? "Sepet_MaxSiparisAdediAsildi" : "Sepet_MinSiparisAdediNotMet";
                return new PlaceOrderResult
                {
                    Status = PlaceOrderStatus.ValidationError,
                    Pricing = pricing,
                    MessageKey = messageKey
                };
            }

            if (!string.IsNullOrWhiteSpace(request.KuponKodu) && string.IsNullOrWhiteSpace(pricing.UygulananKuponKodu))
            {
                return new PlaceOrderResult
                {
                    Status = PlaceOrderStatus.InvalidCoupon,
                    Pricing = pricing,
                    MessageKey = "Sepet_InvalidCoupon"
                };
            }

            var settings = _siteSettingsService.GetSettings();
            var sepetToplamiIndirimli = pricing.AraToplam - pricing.IndirimTutari;

            if (odemeForPricing == "KapidaOdeme" && !settings.KapidaOdemeAktifMi)
            {
                return BusinessError("Siparis_OrderFailed", pricing);
            }

            if (odemeForPricing == "KapidaOdeme" && sepetToplamiIndirimli > settings.KapidaOdemeLimiti)
            {
                return new PlaceOrderResult
                {
                    Status = PlaceOrderStatus.CodLimitExceeded,
                    Pricing = pricing,
                    MessageKey = "Siparis_CODLimitExceeded",
                    MessageArgs = new object[] { settings.KapidaOdemeLimiti.ToString("N0"), settings.ParaBirimi }
                };
            }

            if (request.IsWholesale && settings.ToptanciMinSiparisTutari > 0 && sepetToplamiIndirimli < settings.ToptanciMinSiparisTutari)
            {
                return new PlaceOrderResult
                {
                    Status = PlaceOrderStatus.WholesaleMinimumNotMet,
                    Pricing = pricing,
                    MessageKey = "Siparis_WholesaleMinOrder",
                    MessageArgs = new object[] { settings.ToptanciMinSiparisTutari.ToString("N0"), settings.ParaBirimi }
                };
            }

            if (dto.TeslimatTipi != "MagazadanTeslim")
            {
                var aktifKargoVarMi = await _kargoHesaplama.SehirdeAktifKargoVarMiAsync(dto.Sehir ?? string.Empty);
                if (!aktifKargoVarMi)
                {
                    return new PlaceOrderResult
                    {
                        Status = PlaceOrderStatus.ShippingNotConfigured,
                        Pricing = pricing,
                        MessageKey = "Siparis_NoShippingPrice"
                    };
                }
            }

            if (odemeForPricing == "BankaHavalesi")
            {
                var aktifBankaVarMi = await _context.BankaHesaplari
                    .Where(x => !x.SilindiMi && x.AktifMi)
                    .AnyAsync();
                if (!aktifBankaVarMi)
                {
                    return new PlaceOrderResult
                    {
                        Status = PlaceOrderStatus.NoActiveBankAccount,
                        Pricing = pricing,
                        MessageKey = "Siparis_NoActiveBankAccount"
                    };
                }
            }

            var siparis = await BuildOrderEntityAsync(request, pricing, odemeForPricing);

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            try
            {
                if (dto.AdresiKaydet && !string.IsNullOrEmpty(request.AppUserId))
                {
                    _context.Adresler.Add(new Adres
                    {
                        AppUserId = request.AppUserId,
                        Baslik = string.IsNullOrWhiteSpace(dto.YeniAdresBasligi) ? "Yeni Adresim" : dto.YeniAdresBasligi.Trim(),
                        AdSoyad = siparis.MusteriAdSoyad,
                        Telefon = siparis.Telefon,
                        Sehir = siparis.Sehir,
                        Ilce = siparis.Ilce,
                        AcikAdres = siparis.AcikAdres,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        SilindiMi = false
                    });
                }

                _context.Siparisler.Add(siparis);
                await _context.SaveChangesAsync();

                var stokSonuc = await _orderPricingService.StokDusAsync(pricing.Satirlar);
                if (!stokSonuc.Basarili)
                {
                    _logger.LogWarning(
                        "Siparis stok dusumu basarisiz. Transaction rollback. SecenekId={SecenekId}, Mesaj={Mesaj}",
                        stokSonuc.BasarisizUrunSecenekId,
                        stokSonuc.HataMesaji);

                    await transaction.RollbackAsync();
                    return new PlaceOrderResult
                    {
                        Status = PlaceOrderStatus.StockShortage,
                        Pricing = pricing,
                        StockShortages = pricing.StokYetersizlikleri,
                        MessageKey = "Siparis_StockShortageGeneric"
                    };
                }

                foreach (var line in pricing.Satirlar)
                {
                    var sourceItem = request.SepetItems.First(i => i.Id == line.SepetItemId);
                    var gercekSecenekId = await ResolveOrderVariantIdAsync(sourceItem);
                    _context.SiparisDetaylari.Add(new SiparisDetay
                    {
                        SiparisId = siparis.Id,
                        UrunSecenekId = gercekSecenekId,
                        Adet = line.Adet,
                        BirimFiyat = line.BirimFiyat,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        UrunId = line.UrunId,
                        CerceveModeli = sourceItem.CerceveModeli,
                        MusteriNotu = sourceItem.MusteriNotu,
                        HediyePaketi = line.HediyePaketi,
                        HediyePaketFiyati = line.HediyePaketBirim,
                        SilindiMi = false
                    });
                }

                if (!string.IsNullOrWhiteSpace(siparis.KuponKodu))
                {
                    var now = DateTime.UtcNow;
                    var affected = await _context.Kuponlar
                        .Where(x =>
                            x.Kod == siparis.KuponKodu &&
                            !x.SilindiMi &&
                            x.AktifMi &&
                            (!x.BaslangicTarihi.HasValue || x.BaslangicTarihi <= now) &&
                            x.SonKullanmaTarihi > now &&
                            (x.KullanimLimiti <= 0 || x.KullanilanMiktar < x.KullanimLimiti))
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(x => x.KullanilanMiktar, x => x.KullanilanMiktar + 1));

                    if (affected != 1)
                    {
                        await transaction.RollbackAsync();
                        return new PlaceOrderResult
                        {
                            Status = PlaceOrderStatus.InvalidCoupon,
                            Pricing = pricing,
                            MessageKey = "Sepet_InvalidCoupon"
                        };
                    }
                }

                var cartCleared = await _sepetService.SepetTemizleAsync(request.AppUserId, request.SessionId);
                if (!cartCleared)
                {
                    await transaction.RollbackAsync();
                    return BusinessError("Siparis_OrderFailed", pricing);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return PlaceOrderResult.SuccessResult(siparis.Id, siparis.SiparisNo, pricing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Siparis transaction hatasi. Transaction rollback.");
                await transaction.RollbackAsync();
                return BusinessError("Siparis_OrderFailed", pricing);
            }
        }

        private async Task<Siparis> BuildOrderEntityAsync(PlaceOrderRequest request, OrderPricingResult pricing, string odemeForPricing)
        {
            var dto = request.Checkout;
            var siparis = dto.ToSiparisEntity();
            siparis.SiparisNo = await GenerateUniqueOrderNumberAsync();
            siparis.EmailHashKodu = Guid.NewGuid().ToString("N")[..16];
            siparis.OlusturulmaTarihi = DateTime.UtcNow;
            siparis.Durum = 0;
            siparis.SilindiMi = false;
            siparis.AppUserId = request.AppUserId;
            siparis.KargoTakipNo ??= string.Empty;
            siparis.OdemeYontemi = odemeForPricing;
            siparis.KapidaOdemeHizmetBedeli = pricing.KapidaOdemeHizmetBedeli;

            var kullaniciNotu = dto.Aciklama;
            siparis.Aciklama = siparis.OdemeYontemi == "KapidaOdeme"
                ? request.PayOnDeliveryPendingMessage
                : request.PaymentPendingMessage;
            if (!string.IsNullOrWhiteSpace(kullaniciNotu))
            {
                siparis.Aciklama += " | Not: " + TrimToMaxLength(kullaniciNotu, 1000);
            }

            siparis.IndirimTutari = pricing.IndirimTutari;
            siparis.KuponKodu = pricing.UygulananKuponKodu;
            siparis.ToplamTutar = pricing.GenelToplam;

            if (dto.TeslimatTipi == "MagazadanTeslim")
            {
                siparis.KargoFirmasi = null;
                siparis.KargoFirmasiId = null;
                siparis.Sehir = request.StorePickupCity;
                siparis.Ilce = request.StorePickupDistrict;
                siparis.AcikAdres = request.StorePickupAddress;
                siparis.KargoUcreti = 0;
            }
            else
            {
                siparis.KargoFirmasi = string.Empty;
                siparis.KargoFirmasiId = null;
                siparis.KargoUcreti = Math.Max(0, pricing.KargoUcreti);
            }

            return siparis;
        }

        private async Task<string> GenerateUniqueOrderNumberAsync()
        {
            for (var attempt = 0; attempt < 5; attempt++)
            {
                var candidate = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + Random.Shared.Next(100, 999);
                if (!await _context.Siparisler.AnyAsync(x => x.SiparisNo == candidate))
                {
                    return candidate;
                }
            }

            return $"{DateTime.UtcNow:yyyyMMddHHmmssfff}{Guid.NewGuid():N}"[..24];
        }

        private async Task<int?> ResolveOrderVariantIdAsync(SepetItem item)
        {
            if (item.UrunSecenekId.HasValue)
            {
                return item.UrunSecenekId.Value;
            }

            var varsayilan = await _context.UrunSecenekleri
                .AsNoTracking()
                .Where(x =>
                    x.UrunId == item.UrunId &&
                    !x.SilindiMi &&
                    x.AktifMi &&
                    (!x.TukeninceGizle || x.StokAdedi > 0 || x.OnSipariseAcikMi))
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Sira)
                .ThenBy(x => x.SatisFiyati)
                .FirstOrDefaultAsync();

            return varsayilan?.Id;
        }

        private static string TrimToMaxLength(string value, int maxLength)
        {
            var trimmed = value.Trim();
            return trimmed.Length > maxLength ? trimmed[..maxLength] : trimmed;
        }

        private static PlaceOrderResult BusinessError(string messageKey, OrderPricingResult? pricing = null)
        {
            return new PlaceOrderResult
            {
                Status = PlaceOrderStatus.BusinessError,
                Pricing = pricing,
                MessageKey = messageKey
            };
        }
    }
}
