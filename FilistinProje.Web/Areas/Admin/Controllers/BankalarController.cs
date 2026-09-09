using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    public class BankalarController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BankalarController(
            KanvasDbContext context,
            ISiteSettingsService siteSettingsService,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _siteSettingsService = siteSettingsService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var hesaplar = await _context.BankaHesaplari
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.BankaAdi)
                .ToListAsync();

            ViewBag.BankaHavalesiAktifMi = _siteSettingsService.GetSettings().BankaHavalesiAktifMi;
            return View(hesaplar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleHavale(bool aktif)
        {
            try
            {
                var settings = _siteSettingsService.GetSettings();
                settings.BankaHavalesiAktifMi = aktif;
                _siteSettingsService.SaveSettings(settings);
                TempData["Mesaj"] = _localizer[aktif ? "Admin_BankTransferActivated" : "Admin_BankTransferDeactivated"].Value;
                TempData["Durum"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mesaj"] = ex.Message;
                TempData["Durum"] = "danger";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kaydet(BankaHesap model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.BankaAdi) ||
                    string.IsNullOrWhiteSpace(model.HesapSahibi) ||
                    string.IsNullOrWhiteSpace(model.IBAN))
                {
                    TempData["Mesaj"] = "Admin_Bank_RequiredFields";
                    TempData["Durum"] = "danger";
                    return RedirectToAction(nameof(Index));
                }

                model.BankaAdi = model.BankaAdi.Trim();
                model.HesapSahibi = model.HesapSahibi.Trim();
                model.IBAN = model.IBAN.Trim().Replace(" ", "").ToUpperInvariant();

                if (model.Id == 0)
                {
                    model.OlusturulmaTarihi = DateTime.UtcNow;
                    _context.BankaHesaplari.Add(model);
                    TempData["Mesaj"] = string.Format("Bank account {0} added.", model.BankaAdi);
                }
                else
                {
                    var hesap = await _context.BankaHesaplari
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Id == model.Id && !x.SilindiMi);

                    if (hesap == null)
                    {
                        TempData["Mesaj"] = "Admin_Bank_NotFound";
                        TempData["Durum"] = "danger";
                        return RedirectToAction(nameof(Index));
                    }

                    hesap.BankaAdi = model.BankaAdi;
                    hesap.HesapSahibi = model.HesapSahibi;
                    hesap.IBAN = model.IBAN;
                    hesap.SubeKodu = model.SubeKodu;
                    hesap.HesapNo = model.HesapNo;
                    hesap.AktifMi = model.AktifMi;
                    hesap.Sira = model.Sira;
                    TempData["Mesaj"] = string.Format("Bank account {0} updated.", model.BankaAdi);
                }

                await _context.SaveChangesAsync();
                TempData["Durum"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mesaj"] = "Error: " + ex.Message;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var hesap = await _context.BankaHesaplari
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);

            if (hesap == null)
            {
                TempData["Mesaj"] = "Admin_Bank_NotFound";
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            hesap.SilindiMi = true;
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = string.Format("Bank account {0} deleted.", hesap.BankaAdi);
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}


