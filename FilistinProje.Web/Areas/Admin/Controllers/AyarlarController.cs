using System.Net.Mail;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AyarlarController : AdminBaseController
    {
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly KanvasDbContext _context;

        public AyarlarController(
            ISiteSettingsService siteSettingsService,
            IEmailService emailService,
            IConfiguration config,
            KanvasDbContext context)
        {
            _siteSettingsService = siteSettingsService;
            _emailService = emailService;
            _config = config;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await HazirlaKargoFirmaSecenekleriAsync();
            return View(_siteSettingsService.GetSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteAyarlari model)
        {
            try
            {
                await VarsayilanKargoFirmasiniGuncelleAsync(model.KargoFirmasi);
                _siteSettingsService.SaveSettings(model);
                TempData["Basari"] = "Site ayarları başarıyla kaydedildi.";
                TempData["Durum"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = "Ayarlar kaydedilirken hata oluştu: " + ex.Message;
                TempData["Durum"] = "danger";
            }

            return RedirectToAction(nameof(Index));
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
                TempData["Hata"] = "Test maili için bildirim alıcı e-postası veya site e-postası doldurulmalıdır.";
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
                TempData["Hata"] = "SMTP ayarları eksik. Sunucu, kullanıcı, parola ve geçerli bir gönderici e-posta adresi tanımlanmalıdır.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            if (!IsValidEmail(recipientEmail))
            {
                TempData["Hata"] = "Test maili için geçerli bir alıcı e-posta adresi yazın.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _emailService.SendTemplateMailAsync(
                    recipientEmail,
                    "Test Maili",
                    string.IsNullOrWhiteSpace(model.MarkaAdi) ? "7ANRPS48" : model.MarkaAdi,
                    "Mail altyapısı başarıyla çalışıyor. Sipariş, kargo, üyelik ve kampanya bildirimleri bu kanal üzerinden gönderilecektir.",
                    string.Empty,
                    string.Empty);

                TempData["Basari"] = $"Test maili gönderildi. Alıcı: {recipientEmail}. Gelen kutusunu ve SMTP sunucu günlüklerini kontrol edin.";
                TempData["Durum"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = ex is TimeoutException
                    ? "Test maili gönderimi 30 saniye içinde tamamlanamadı. SMTP bağlantısını ve ağ erişimini kontrol edin."
                    : "Test maili gönderilemedi: " + ex.Message;
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
