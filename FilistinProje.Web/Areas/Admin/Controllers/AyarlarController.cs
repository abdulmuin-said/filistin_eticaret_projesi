using System.Net.Mail;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AyarlarController : AdminBaseController
    {
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AyarlarController(
            ISiteSettingsService siteSettingsService,
            IEmailService emailService,
            IConfiguration config,
            KanvasDbContext context,
            IStringLocalizer<SharedResource> localizer)
        {
            _siteSettingsService = siteSettingsService;
            _emailService = emailService;
            _config = config;
            _context = context;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string? tab = null)
        {
            await HazirlaKargoFirmaSecenekleriAsync();
            ViewBag.ActiveTab = string.IsNullOrWhiteSpace(tab) ? "genel" : tab;
            return View(_siteSettingsService.GetSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteAyarlari model, string? aktifSekme)
        {
            try
            {
                await VarsayilanKargoFirmasiniGuncelleAsync(model.KargoFirmasi);
                _siteSettingsService.SaveSettings(model);
                TempData["Basari"] = _localizer["Admin_SettingsSaveSuccess"].Value;
                TempData["Durum"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = _localizer["Admin_SettingsSaveFailed", ex.Message].Value;
                TempData["Durum"] = "danger";
            }

            return RedirectToAction(nameof(Index), new { tab = aktifSekme });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestMail(SiteAyarlari model)
        {
            var recipientEmail = !string.IsNullOrWhiteSpace(model.BildirimAliciEmail)
                ? model.BildirimAliciEmail
                : model.Email;

            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                TempData["Hata"] = _localizer["Admin_TestMailRecipientRequired"].Value;
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var smtpUser = _config["EmailSettings:Username"];
            var smtpPassword = _config["EmailSettings:Password"];
            var fromEmail = _config["EmailSettings:FromEmail"];
            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                fromEmail = model.Email;
            }

            if (string.IsNullOrWhiteSpace(_config["EmailSettings:Host"]) || !IsValidEmail(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword) || !IsValidEmail(fromEmail))
            {
                TempData["Hata"] = _localizer["Admin_SmtpSettingsMissing"].Value;
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            if (!IsValidEmail(recipientEmail))
            {
                TempData["Hata"] = _localizer["Admin_TestMailInvalidRecipient"].Value;
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _emailService.SendTemplateMailAsync(
                    recipientEmail,
                    _localizer["Admin_TestMailSubject"].Value,
                    string.IsNullOrWhiteSpace(model.MarkaAdi) ? "7ANRPS48" : model.MarkaAdi,
                    _localizer["Admin_TestMailBody"].Value,
                    string.Empty,
                    string.Empty);

                TempData["Basari"] = _localizer["Admin_TestMailSuccess", recipientEmail].Value;
                TempData["Durum"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = ex is TimeoutException
                    ? _localizer["Admin_TestMailTimeout"].Value
                    : _localizer["Admin_TestMailFailed", ex.Message].Value;
                TempData["Durum"] = "danger";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task HazirlaKargoFirmaSecenekleriAsync()
        {
            var firmalar = await _context.KargoFirmalari
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi && x.AktifMi)
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Ad)
                .ToListAsync();

            if (!firmalar.Any())
            {
                firmalar = VarsayilanKargoFirmalari();
            }

            var seciliFirma = _siteSettingsService.GetSettings().KargoFirmasi;
            if (!string.IsNullOrWhiteSpace(seciliFirma) &&
                firmalar.All(x => !string.Equals(x.Ad, seciliFirma, StringComparison.OrdinalIgnoreCase)))
            {
                firmalar.Insert(0, new KargoFirmasi
                {
                    Ad = seciliFirma,
                    Kod = seciliFirma.ToLowerInvariant().Replace(" ", "-"),
                    AktifMi = true,
                    VarsayilanMi = true
                });
            }

            ViewBag.KargoFirmalari = firmalar;
        }

        private async Task VarsayilanKargoFirmasiniGuncelleAsync(string? firmaAdi)
        {
            if (string.IsNullOrWhiteSpace(firmaAdi))
            {
                return;
            }

            var firma = await _context.KargoFirmalari
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => !x.SilindiMi && x.Ad == firmaAdi.Trim());

            if (firma == null)
            {
                return;
            }

            await _context.KargoFirmalari
                .Where(x => x.Id != firma.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(v => v.VarsayilanMi, false));

            firma.VarsayilanMi = true;
            firma.AktifMi = true;
            await _context.SaveChangesAsync();
        }

        private static List<KargoFirmasi> VarsayilanKargoFirmalari()
        {
            return new List<KargoFirmasi>
            {
                new() { Ad = "توصيل محلي", Kod = "local-delivery", TakipUrl = string.Empty }
            };
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                _ = new MailAddress(email.Trim());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
