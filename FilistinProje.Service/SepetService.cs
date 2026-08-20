using System.Linq;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FilistinProje.Service
{
    public class SepetService : ISepetService
    {
        private readonly KanvasDbContext _context;
        private readonly ILogger<SepetService> _logger;

        public SepetService(KanvasDbContext context, ILogger<SepetService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Sepet> GetOrCreateSepetAsync(string? userId, string sessionId)
        {
            Sepet? sepet = null;

            if (!string.IsNullOrEmpty(userId))
            {
                sepet = await _context.Sepetler
                    .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                        .ThenInclude(i => i.Urun)
                    .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                        .ThenInclude(i => i.UrunSecenek)
                    .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                        .ThenInclude(i => i.HediyePaketSecenegi)
                    .FirstOrDefaultAsync(s => s.AppUserId == userId && !s.SilindiMi);
            }
            else
            {
                sepet = await _context.Sepetler
                    .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                        .ThenInclude(i => i.Urun)
                    .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                        .ThenInclude(i => i.UrunSecenek)
                    .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                        .ThenInclude(i => i.HediyePaketSecenegi)
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId && !s.SilindiMi);
            }

            if (sepet == null)
            {
                sepet = new Sepet
                {
                    AppUserId = userId,
                    SessionId = string.IsNullOrEmpty(userId) ? sessionId : null,
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SonGuncellemeTarihi = DateTime.UtcNow,
                    SilindiMi = false
                };

                _context.Sepetler.Add(sepet);
                await _context.SaveChangesAsync();
            }

            return sepet;
        }

        public async Task<bool> SepeteEkleAsync(string? userId, string sessionId, int urunId, int? urunSecenekId, int adet, string? cerceveModeli = null, string? musteriNotu = null, decimal? cerceveFarki = null, int? hediyePaketSecenegiId = null)
        {
            try
            {
                var sepet = await GetOrCreateSepetAsync(userId, sessionId);
                var urun = await _context.Urunler
                    .AsNoTracking()
                    .Include(x => x.Kategori!)
                        .ThenInclude(x => x.ParentKategori)
                    .Include(x => x.UrunSecenek)
                    .Include(x => x.HediyePaketSecenekleri)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == urunId && x.AktifMi && !x.SilindiMi);

                if (urun == null)
                {
                    return false;
                }

                // WhatsApp-only and hidden-price products must never enter normal checkout.
                if (urun.WhatsappSiparisVarMi || urun.FiyatGizliMi)
                {
                    return false;
                }

                var secenek = ResolveSelectedVariant(urun, urunSecenekId);
                if (urunSecenekId.HasValue && secenek == null)
                {
                    return false;
                }

                var requiresFrameSelection = RequiresFrameSelection(urun);
                var normalizedCerceveModeli = NormalizeFrameModel(cerceveModeli);
                if (requiresFrameSelection && string.IsNullOrWhiteSpace(normalizedCerceveModeli))
                {
                    normalizedCerceveModeli = "Çerçevesiz";
                }

                var hedefSecenekId = secenek?.Id;
                var normalizedMusteriNotu = NormalizeCustomerNote(musteriNotu);
                var hediyePaketSecenegi = hediyePaketSecenegiId.HasValue
                    ? urun.HediyePaketSecenekleri.FirstOrDefault(x => x.Id == hediyePaketSecenegiId.Value && x.AktifMi && !x.SilindiMi)
                    : null;
                if (hediyePaketSecenegiId.HasValue && hediyePaketSecenegi == null)
                {
                    return false;
                }

                var guvenliCerceveFarki = CalculateFramePrice(secenek, normalizedCerceveModeli);
                var mevcutItem = sepet.SepetItems.FirstOrDefault(i =>
                    i.UrunId == urunId &&
                    i.UrunSecenekId == hedefSecenekId &&
                    i.CerceveModeli == normalizedCerceveModeli &&
                    NormalizeCustomerNote(i.MusteriNotu) == normalizedMusteriNotu &&
                    i.HediyePaketSecenegiId == hediyePaketSecenegiId &&
                    !i.SilindiMi);

                var toplamAdet = (mevcutItem?.Adet ?? 0) + adet;
                if (!CanAddQuantity(urun, secenek, toplamAdet))
                {
                    return false;
                }

                if (mevcutItem != null)
                {
                    mevcutItem.Adet += adet;
                }
                else
                {
                    var fiyat = await CalculateCurrentUnitPriceAsync(urun, secenek, userId, adet) + guvenliCerceveFarki;
                    var secenekAdi = secenek != null ? BuildVariantLabel(secenek) : null;
                    var gorsel = secenek != null && !string.IsNullOrWhiteSpace(secenek.GorselUrl)
                        ? secenek.GorselUrl
                        : urun.AnaGorselUrl;

                    var yeniItem = new SepetItem
                    {
                        SepetId = sepet.Id,
                        UrunId = urunId,
                        UrunSecenekId = hedefSecenekId,
                        Adet = adet,
                        Fiyat = fiyat,
                        UrunBaslik = urun.Baslik,
                        UrunResimUrl = gorsel,
                        SecenekAdi = BuildCartOptionLabel(secenekAdi, normalizedCerceveModeli),
                        CerceveModeli = normalizedCerceveModeli,
                        MusteriNotu = normalizedMusteriNotu,
                        HediyePaketSecenegiId = hediyePaketSecenegi?.Id,
                        HediyePaketi = hediyePaketSecenegi != null,
                        HediyePaketFiyati = hediyePaketSecenegi?.Fiyat ?? 0,
                        HediyePaketAdi = hediyePaketSecenegi?.Ad ?? string.Empty,
                        HediyePaketAdiEn = hediyePaketSecenegi?.AdEn ?? string.Empty,
                        HediyePaketAdiAr = hediyePaketSecenegi?.AdAr ?? string.Empty,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        SilindiMi = false
                    };

                    _context.SepetItems.Add(yeniItem);
                }

                MarkCartAsUpdated(sepet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepete ekleme hatasi. UrunId={UrunId}, SecenekId={SecenekId}", urunId, urunSecenekId);
                return false;
            }
        }

        public async Task<bool> AdediGuncelleAsync(int sepetItemId, int yeniAdet)
        {
            try
            {
                var item = await _context.SepetItems
                    .Include(x => x.Sepet)
                    .Include(x => x.Urun)
                        .ThenInclude(x => x.Kategori!)
                            .ThenInclude(x => x.ParentKategori)
                    .Include(x => x.UrunSecenek)
                    .FirstOrDefaultAsync(x => x.Id == sepetItemId);
                if (item == null || item.SilindiMi)
                {
                    return false;
                }

                if (yeniAdet <= 0)
                {
                    return await SepettenCikarAsync(sepetItemId);
                }

                if (!CanAddQuantity(item.Urun, item.UrunSecenek, yeniAdet))
                {
                    return false;
                }

                item.Adet = yeniAdet;
                MarkCartAsUpdated(item.Sepet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Adet guncelleme hatasi. SepetItemId={SepetItemId}", sepetItemId);
                return false;
            }
        }

        public async Task<bool> SepettenCikarAsync(int sepetItemId)
        {
            try
            {
                var item = await _context.SepetItems
                    .Include(x => x.Sepet)
                    .FirstOrDefaultAsync(x => x.Id == sepetItemId);
                if (item == null)
                {
                    return false;
                }

                item.SilindiMi = true;
                MarkCartAsUpdated(item.Sepet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepetten cikarma hatasi. SepetItemId={SepetItemId}", sepetItemId);
                return false;
            }
        }

        public async Task<List<SepetItem>> GetSepetItemsAsync(string? userId, string sessionId)
        {
            var sepet = await GetOrCreateSepetAsync(userId, sessionId);
            var items = sepet.SepetItems.Where(i => !i.SilindiMi).ToList();
            var changed = false;
            foreach (var item in items)
            {
                if (item.Urun == null)
                {
                    continue;
                }

                var currentPrice = await CalculateCurrentUnitPriceAsync(item.Urun, item.UrunSecenek, userId, item.Adet)
                    + CalculateFramePrice(item.UrunSecenek, item.CerceveModeli);
                if (item.Fiyat != currentPrice)
                {
                    item.Fiyat = currentPrice;
                    changed = true;
                }

                if (item.HediyePaketSecenegiId.HasValue && item.HediyePaketSecenegi is { AktifMi: true, SilindiMi: false } package && package.UrunId == item.UrunId)
                {
                    if (!item.HediyePaketi || item.HediyePaketFiyati != package.Fiyat || item.HediyePaketAdi != package.Ad || item.HediyePaketAdiEn != package.AdEn || item.HediyePaketAdiAr != package.AdAr)
                    {
                        item.HediyePaketi = true;
                        item.HediyePaketFiyati = package.Fiyat;
                        item.HediyePaketAdi = package.Ad;
                        item.HediyePaketAdiEn = package.AdEn;
                        item.HediyePaketAdiAr = package.AdAr;
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                await _context.SaveChangesAsync();
            }

            return items;
        }

        private async Task<decimal> CalculateCurrentUnitPriceAsync(Urun urun, UrunSecenek? secenek, string? userId, int adet)
        {
            var isWholesale = !string.IsNullOrWhiteSpace(userId) && await (
                from userRole in _context.UserRoles
                join role in _context.Roles on userRole.RoleId equals role.Id
                where userRole.UserId == userId && role.Name == "Wholesale"
                select userRole.UserId).AnyAsync();

            if (isWholesale)
            {
                var secenekId = secenek?.Id;
                var directTier = await _context.UrunToptanFiyatKademeleri
                    .AsNoTracking()
                    .Where(x => !x.SilindiMi && x.AktifMi && x.UrunId == urun.Id && x.MinAdet <= adet
                        && ((secenekId.HasValue && x.UrunSecenekId == secenekId.Value)
                            || (!secenekId.HasValue && !x.UrunSecenekId.HasValue)))
                    .OrderByDescending(x => x.UrunSecenekId.HasValue)
                    .ThenByDescending(x => x.MinAdet)
                    .ThenBy(x => x.Sira)
                    .FirstOrDefaultAsync();
                if (directTier == null && secenek != null)
                {
                    directTier = await _context.UrunToptanFiyatKademeleri
                        .AsNoTracking()
                        .Where(x => !x.SilindiMi && x.AktifMi && x.UrunId == urun.Id && !x.UrunSecenekId.HasValue && x.MinAdet <= adet)
                        .OrderByDescending(x => x.MinAdet)
                        .ThenBy(x => x.Sira)
                        .FirstOrDefaultAsync();
                }
                if (directTier != null)
                {
                    return Math.Round(directTier.BirimFiyat, 2);
                }
            }

            var price = secenek is { SatisFiyati: > 0 }
                ? secenek.SatisFiyati
                : isWholesale ? urun.EtkinTopFiyat : urun.EtkinFiyat;

            if (isWholesale && urun.ToptanciUrunGrubuId.HasValue)
            {
                var discount = await _context.ToptanciIskontoOranlari
                    .AsNoTracking()
                    .Where(x => !x.SilindiMi && x.ToptanciUrunGrubuId == urun.ToptanciUrunGrubuId && adet >= x.MinAdet)
                    .MaxAsync(x => (decimal?)x.IskontoYuzdesi) ?? 0;
                if (discount > 0)
                {
                    price *= 1m - discount / 100m;
                }
            }

            return Math.Round(price, 2);
        }

        public async Task<decimal> GetSepetToplamiAsync(string? userId, string sessionId)
        {
            var items = await GetSepetItemsAsync(userId, sessionId);
            return items.Sum(i => i.Toplam);
        }

        public async Task<int> GetSepetUrunSayisiAsync(string? userId, string sessionId)
        {
            var query = _context.Sepetler.AsNoTracking();
            query = !string.IsNullOrWhiteSpace(userId)
                ? query.Where(s => s.AppUserId == userId && !s.SilindiMi)
                : query.Where(s => s.SessionId == sessionId && !s.SilindiMi);

            return await query
                .SelectMany(s => s.SepetItems)
                .Where(i => !i.SilindiMi)
                .SumAsync(i => (int?)i.Adet) ?? 0;
        }

        public async Task<bool> SepetTemizleAsync(string? userId, string sessionId)
        {
            try
            {
                var sepet = await GetOrCreateSepetAsync(userId, sessionId);

                foreach (var item in sepet.SepetItems.Where(i => !i.SilindiMi))
                {
                    item.SilindiMi = true;
                }

                sepet.SonGuncellemeTarihi = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet temizleme hatasi");
                return false;
            }
        }

        public async Task MergeSepetlerAsync(string sessionId, string userId)
        {
            await MergeSepetlerDetailedAsync(sessionId, userId);
        }

        public async Task<FilistinProje.Core.DTOs.SepetMergeResult> MergeSepetlerDetailedAsync(string sessionId, string userId)
        {
            var result = new FilistinProje.Core.DTOs.SepetMergeResult();

            var anonymousSepet = await _context.Sepetler
                .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && string.IsNullOrEmpty(s.AppUserId) && !s.SilindiMi);

            if (anonymousSepet == null || !anonymousSepet.SepetItems.Any())
            {
                return result;
            }

            var userSepet = await _context.Sepetler
                .Include(s => s.SepetItems.Where(i => !i.SilindiMi))
                .FirstOrDefaultAsync(s => s.AppUserId == userId && !s.SilindiMi);

            if (userSepet == null)
            {
                userSepet = new Sepet
                {
                    AppUserId = userId,
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SonGuncellemeTarihi = DateTime.UtcNow,
                    SilindiMi = false
                };

                _context.Sepetler.Add(userSepet);
                await _context.SaveChangesAsync();
            }

            if (userSepet.Id == anonymousSepet.Id)
            {
                return result;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);

            try
            {
                var anonItems = anonymousSepet.SepetItems.Where(i => !i.SilindiMi).ToList();
                var urunIds = anonItems.Select(i => i.UrunId)
                    .Concat(userSepet.SepetItems.Where(i => !i.SilindiMi).Select(i => i.UrunId))
                    .Distinct()
                    .ToList();

                var urunler = await _context.Urunler
                    .AsNoTracking()
                    .Include(u => u.UrunSecenek)
                    .Include(u => u.HediyePaketSecenekleri)
                    .Where(u => urunIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id);

                foreach (var anonItem in anonItems)
                {
                    if (!urunler.TryGetValue(anonItem.UrunId, out var urun) || !urun.AktifMi || urun.SilindiMi || urun.WhatsappSiparisVarMi || urun.FiyatGizliMi)
                    {
                        await transaction.RollbackAsync();
                        result.Basarili = false;
                        result.MessageKey = "Sepet_ProductUnavailable";
                        result.HataMesaji = $"Urun pasif veya satis disi: {anonItem.UrunBaslik}";
                        result.EngellenenUrunler.Add(anonItem.UrunBaslik);
                        return result;
                    }

                    UrunSecenek? secenek = null;
                    if (anonItem.UrunSecenekId.HasValue)
                    {
                        secenek = urun.UrunSecenek.FirstOrDefault(s => s.Id == anonItem.UrunSecenekId.Value && !s.SilindiMi && s.AktifMi);
                        if (secenek == null)
                        {
                            await transaction.RollbackAsync();
                            result.Basarili = false;
                            result.MessageKey = "Sepet_VariantUnavailable";
                            result.HataMesaji = $"Varyant gecersiz: {anonItem.UrunBaslik}";
                            result.EngellenenUrunler.Add(anonItem.UrunBaslik);
                            return result;
                        }
                    }

                    UrunHediyePaketSecenegi? package = null;
                    if (anonItem.HediyePaketSecenegiId.HasValue)
                    {
                        package = urun.HediyePaketSecenekleri.FirstOrDefault(x =>
                            x.Id == anonItem.HediyePaketSecenegiId.Value &&
                            x.AktifMi &&
                            !x.SilindiMi);
                        if (package == null)
                        {
                            await transaction.RollbackAsync();
                            result.Basarili = false;
                            result.MessageKey = "Sepet_GiftPackageUnavailable";
                            result.HataMesaji = $"Hediye paket secenegi gecersiz: {anonItem.UrunBaslik}";
                            result.EngellenenUrunler.Add(anonItem.UrunBaslik);
                            return result;
                        }
                    }

                    var mevcutItem = userSepet.SepetItems.FirstOrDefault(i =>
                        i.UrunId == anonItem.UrunId &&
                        i.UrunSecenekId == anonItem.UrunSecenekId &&
                        i.CerceveModeli == anonItem.CerceveModeli &&
                        NormalizeCustomerNote(i.MusteriNotu) == NormalizeCustomerNote(anonItem.MusteriNotu) &&
                        i.HediyePaketSecenegiId == anonItem.HediyePaketSecenegiId &&
                        !i.SilindiMi);

                    var mevcutAdet = mevcutItem?.Adet ?? 0;

                    long candidateLong = (long)mevcutAdet + (long)anonItem.Adet;
                    if (candidateLong <= 0 || candidateLong > int.MaxValue)
                    {
                        await transaction.RollbackAsync();
                        result.Basarili = false;
                        result.MessageKey = "Sepet_InvalidQuantity";
                        result.HataMesaji = $"Gecersiz adet toplami: {anonItem.UrunBaslik}";
                        result.EngellenenUrunler.Add(anonItem.UrunBaslik);
                        return result;
                    }

                    int candidateQty = (int)candidateLong;

                    if (!CanAddQuantity(urun, secenek, candidateQty))
                    {
                        await transaction.RollbackAsync();
                        result.Basarili = false;
                        result.MessageKey = "Sepet_MaxSiparisAdediAsildi";
                        result.HataMesaji = $"Siparis veya stok limiti asildi: {anonItem.UrunBaslik}";
                        result.EngellenenUrunler.Add(anonItem.UrunBaslik);
                        return result;
                    }

                    if (mevcutItem != null)
                    {
                        mevcutItem.Adet = candidateQty;
                    }
                    else
                    {
                        var yeniItem = new SepetItem
                        {
                            SepetId = userSepet.Id,
                            UrunId = anonItem.UrunId,
                            UrunSecenekId = anonItem.UrunSecenekId,
                            Adet = anonItem.Adet,
                            Fiyat = anonItem.Fiyat,
                            UrunBaslik = anonItem.UrunBaslik,
                            UrunResimUrl = anonItem.UrunResimUrl,
                            SecenekAdi = anonItem.SecenekAdi,
                            CerceveModeli = anonItem.CerceveModeli,
                            MusteriNotu = anonItem.MusteriNotu,
                            HediyePaketSecenegiId = package?.Id,
                            HediyePaketi = package != null,
                            HediyePaketFiyati = package?.Fiyat ?? 0,
                            HediyePaketAdi = package?.Ad ?? string.Empty,
                            HediyePaketAdiEn = package?.AdEn ?? string.Empty,
                            HediyePaketAdiAr = package?.AdAr ?? string.Empty,
                            OlusturulmaTarihi = DateTime.UtcNow,
                            SilindiMi = false
                        };
                        _context.SepetItems.Add(yeniItem);
                    }

                    anonItem.SilindiMi = true;
                }

                anonymousSepet.SilindiMi = true;
                userSepet.SonGuncellemeTarihi = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Basarili = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet birlestirme hatasi.");
                await transaction.RollbackAsync();
                result.Basarili = false;
                result.MessageKey = "Sepet_MergeFailed";
                result.HataMesaji = ex.Message;
                return result;
            }
        }

        public async Task<bool> NotGuncelleAsync(int sepetItemId, string? musteriNotu)
        {
            try
            {
                var item = await _context.SepetItems
                    .Include(x => x.Sepet)
                    .FirstOrDefaultAsync(x => x.Id == sepetItemId);
                if (item == null || item.SilindiMi)
                {
                    return false;
                }

                item.MusteriNotu = NormalizeCustomerNote(musteriNotu);
                MarkCartAsUpdated(item.Sepet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void MarkCartAsUpdated(Sepet sepet)
        {
            sepet.SonGuncellemeTarihi = DateTime.UtcNow;
            sepet.HatirlatmaGonderildi = false;
            sepet.TerkEdildi = false;
            sepet.TerkEdilmeTarihi = null;
        }

        private static UrunSecenek? ResolveSelectedVariant(Urun urun, int? requestedVariantId)
        {
            var variants = urun.UrunSecenek
                .Where(x =>
                    !x.SilindiMi &&
                    x.AktifMi &&
                    (!x.TukeninceGizle || x.StokAdedi > 0 || x.OnSipariseAcikMi))
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Sira)
                .ThenBy(x => x.SatisFiyati)
                .ToList();

            if (!variants.Any())
            {
                return null;
            }

            if (requestedVariantId.HasValue)
            {
                return variants.FirstOrDefault(x => x.Id == requestedVariantId.Value);
            }

            return variants.FirstOrDefault(x => x.SatinAlinabilirMi)
                ?? variants.FirstOrDefault();
        }

        private static bool CanAddQuantity(Urun urun, UrunSecenek? secenek, int toplamAdet)
        {
            if (toplamAdet < urun.MinSiparisAdedi)
            {
                return false;
            }

            if (urun.MaxSiparisAdedi.HasValue && urun.MaxSiparisAdedi.Value > 0 && toplamAdet > urun.MaxSiparisAdedi.Value)
            {
                return false;
            }

            if (secenek == null)
            {
                if (urun.ToplamStok > 0 && toplamAdet > urun.ToplamStok)
                {
                    return false;
                }
                return true;
            }

            if (!secenek.SatinAlinabilirMi)
            {
                return false;
            }

            if (secenek.StokAdedi > 0 && toplamAdet > secenek.StokAdedi && !secenek.OnSipariseAcikMi)
            {
                return false;
            }

            return true;
        }

        private static string BuildVariantLabel(UrunSecenek secenek)
        {
            var baslikParcalari = new[]
                {
                    secenek.Olcu,
                    secenek.MalzemeTuru,
                    secenek.Yon,
                    secenek.ParcaSayisi > 1 ? $"{secenek.ParcaSayisi} Parca" : null
                }
                .Where(IsMeaningfulVariantPart)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var detayParcalari = new List<string>();

            if (IsMeaningfulVariantPart(secenek.CerceveKalinligi))
            {
                detayParcalari.Add($"Kalinlik: {secenek.CerceveKalinligi.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(secenek.VaryantSku))
            {
                detayParcalari.Add($"SKU: {secenek.VaryantSku.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(secenek.KisilestirmeMetni))
            {
                detayParcalari.Add($"Kisisellestirme: {secenek.KisilestirmeMetni.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(secenek.OzelTasarimNotu))
            {
                detayParcalari.Add($"Not: {secenek.OzelTasarimNotu.Trim()}");
            }

            var baslik = string.Join(" / ", baslikParcalari);
            var detay = string.Join(" | ", detayParcalari);

            if (string.IsNullOrWhiteSpace(baslik) && string.IsNullOrWhiteSpace(detay))
            {
                return string.Empty;
            }

            return string.Join(" | ", new[] { baslik, detay }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private static bool IsMeaningfulVariantPart(string? value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !IsGenericVariantValue(value);
        }

        private static bool IsGenericVariantValue(string value)
        {
            var normalized = value.Trim().ToLowerInvariant();
            return normalized is "standart" or "standard" or "varsayilan" or "varsayılan";
        }

        private static string BuildCartOptionLabel(string? variantLabel, string frameModel)
        {
            if (string.IsNullOrWhiteSpace(frameModel) || frameModel == "Çerçevesiz")
            {
                return variantLabel ?? string.Empty;
            }

            return string.IsNullOrWhiteSpace(variantLabel)
                ? $"Çerçeve: {frameModel}"
                : $"{variantLabel} | Çerçeve: {frameModel}";
        }

        private static string NormalizeFrameModel(string? frameModel)
        {
            var value = (frameModel ?? string.Empty).Trim().ToLowerInvariant();
            return value switch
            {
                "çerçevesiz" => "Çerçevesiz",
                "cercevesiz" => "Çerçevesiz",
                "siyah" => "Siyah",
                "beyaz" => "Beyaz",
                "gold" => "Gold",
                "gümüş" or "gumus" => "Gümüş",
                "meşe" or "mese" => "Meşe",
                "ceviz" => "Ceviz",
                _ => string.Empty
            };
        }

        private static decimal CalculateFramePrice(UrunSecenek? secenek, string frameModel)
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

            var parts = olcu.Split(new[] { 'x', 'X', '×' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
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

        private static bool RequiresFrameSelection(Urun urun)
        {
            return ContainsKanvas(urun.UrunTipi)
                || ContainsKanvas(urun.Kategori?.Ad)
                || ContainsKanvas(urun.Kategori?.ParentKategori?.Ad);
        }

        private static bool ContainsKanvas(string? value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && value.Contains("kanvas", StringComparison.OrdinalIgnoreCase);
        }

        private static string? NormalizeCustomerNote(string? note)
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                return null;
            }

            var trimmed = note.Trim();
            return trimmed.Length > 500 ? trimmed[..500] : trimmed;
        }
    }
}
