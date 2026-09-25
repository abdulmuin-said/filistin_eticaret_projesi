using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaketlemeController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PaketlemeController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET /Admin/Paketleme -> Redirect to Settings tab
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ayarlar", new { area = "Admin", tab = "paketleme" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KaydetJson(int id, string? ad, string? adEn, string? adAr, decimal fiyat, int sira, bool aktifMi)
        {
            var cleanAr = (adAr ?? string.Empty).Trim();
            var cleanEn = (adEn ?? string.Empty).Trim();
            var cleanAd = (ad ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(cleanAr) && string.IsNullOrWhiteSpace(cleanEn) && string.IsNullOrWhiteSpace(cleanAd))
            {
                return Json(new { success = false, message = _localizer["Admin_GiftPackageName"].Value + " " + _localizer["Common_Required"].Value });
            }

            if (fiyat < 0)
            {
                return Json(new { success = false, message = _localizer["Admin_GiftPackagePriceInvalid", 1].Value });
            }

            var defaultName = !string.IsNullOrWhiteSpace(cleanAr)
                ? cleanAr
                : (!string.IsNullOrWhiteSpace(cleanEn) ? cleanEn : cleanAd);

            UrunHediyePaketSecenegi package;
            if (id == 0)
            {
                package = new UrunHediyePaketSecenegi
                {
                    UrunId = null,
                    Ad = defaultName,
                    AdAr = cleanAr,
                    AdEn = cleanEn,
                    Fiyat = decimal.Round(Math.Max(0, fiyat), 2),
                    Sira = sira > 0 ? sira : (await _context.UrunHediyePaketSecenekleri.CountAsync(x => x.UrunId == null && !x.SilindiMi)) + 1,
                    AktifMi = aktifMi,
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                };
                _context.UrunHediyePaketSecenekleri.Add(package);
            }
            else
            {
                var existing = await _context.UrunHediyePaketSecenekleri.FirstOrDefaultAsync(x => x.Id == id && x.UrunId == null && !x.SilindiMi);
                if (existing == null)
                {
                    return Json(new { success = false, message = "Package not found." });
                }

                existing.Ad = defaultName;
                existing.AdAr = cleanAr;
                existing.AdEn = cleanEn;
                existing.Fiyat = decimal.Round(Math.Max(0, fiyat), 2);
                existing.Sira = sira;
                existing.AktifMi = aktifMi;
                package = existing;
            }

            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                id = package.Id,
                message = _localizer["Admin_GiftPackageSaved"].Value
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SilJson(int id)
        {
            var package = await _context.UrunHediyePaketSecenekleri.FirstOrDefaultAsync(x => x.Id == id && x.UrunId == null);
            if (package == null)
            {
                return Json(new { success = false, message = "Package not found." });
            }

            package.SilindiMi = true;
            package.AktifMi = false;
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = _localizer["Admin_GiftPackageDeleted"].Value
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluKaydet(List<UrunHediyePaketSecenegi>? paketler)
        {
            try
            {
                var incomingList = paketler ?? new List<UrunHediyePaketSecenegi>();
                var existingList = await _context.UrunHediyePaketSecenekleri
                    .Where(x => x.UrunId == null && !x.SilindiMi)
                    .ToListAsync();

                var incomingIds = incomingList.Where(x => x.Id > 0).Select(x => x.Id).ToHashSet();

                // Silinenler
                foreach (var existing in existingList.Where(x => !incomingIds.Contains(x.Id)))
                {
                    existing.SilindiMi = true;
                    existing.AktifMi = false;
                }

                // Ekle ve Güncelle
                for (var i = 0; i < incomingList.Count; i++)
                {
                    var item = incomingList[i];
                    var cleanAr = (item.AdAr ?? string.Empty).Trim();
                    var cleanEn = (item.AdEn ?? string.Empty).Trim();
                    var cleanAd = (item.Ad ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(cleanAr) && string.IsNullOrWhiteSpace(cleanEn) && string.IsNullOrWhiteSpace(cleanAd))
                    {
                        continue;
                    }

                    var defaultName = !string.IsNullOrWhiteSpace(cleanAr)
                        ? cleanAr
                        : (!string.IsNullOrWhiteSpace(cleanEn) ? cleanEn : cleanAd);

                    if (item.Id > 0)
                    {
                        var target = existingList.FirstOrDefault(x => x.Id == item.Id);
                        if (target != null)
                        {
                            target.Ad = defaultName;
                            target.AdAr = cleanAr;
                            target.AdEn = cleanEn;
                            target.Fiyat = decimal.Round(Math.Max(0, item.Fiyat), 2);
                            target.Sira = item.Sira > 0 ? item.Sira : (i + 1);
                            target.AktifMi = item.AktifMi;
                        }
                    }
                    else
                    {
                        _context.UrunHediyePaketSecenekleri.Add(new UrunHediyePaketSecenegi
                        {
                            UrunId = null,
                            Ad = defaultName,
                            AdAr = cleanAr,
                            AdEn = cleanEn,
                            Fiyat = decimal.Round(Math.Max(0, item.Fiyat), 2),
                            Sira = item.Sira > 0 ? item.Sira : (i + 1),
                            AktifMi = item.AktifMi,
                            OlusturulmaTarihi = DateTime.UtcNow,
                            SilindiMi = false
                        });
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Basari"] = _localizer["Admin_GiftPackageSaved"].Value;
                TempData["Durum"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = ex.Message;
                TempData["Durum"] = "danger";
            }

            return RedirectToAction("Index", "Ayarlar", new { area = "Admin", tab = "paketleme" });
        }
    }
}
