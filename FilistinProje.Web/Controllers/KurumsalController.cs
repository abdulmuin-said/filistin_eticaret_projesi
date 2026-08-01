using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Controllers
{
    [Route("pages")]
    public class KurumsalController : Controller
    {
        private readonly KanvasDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly ILogger<KurumsalController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public KurumsalController(
            KanvasDbContext context,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            ILogger<KurumsalController> logger,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
            _logger = logger;
            _localizer = localizer;
        }

        [Route("Kurumsal/Detay/{slug}")]
        public async Task<IActionResult> Detay(string slug)
        {
            var normalizedSlug = NormalizeSlug(slug);
            var sayfalar = await _context.KurumsalSayfalar
                .AsNoTracking()
                .Where(x => !x.SilindiMi)
                .ToListAsync();

            var sayfa = sayfalar.FirstOrDefault(x => NormalizeSlug(x.UrlSlug) == normalizedSlug);
            if (sayfa == null)
            {
                return NotFound();
            }

            ViewData["Title"] = sayfa.LocalizedBaslik;
            return View("Detay", sayfa);
        }

        [HttpGet("about")]
        [HttpGet("/Kurumsal/Hakkimizda")]
        public async Task<IActionResult> Hakkimizda() => await GetDynamicOrFallbackViewAsync("hakkimizda", "Hakkimizda");

        [HttpGet("contact")]
        [HttpGet("/Kurumsal/Iletisim")]
        [HttpGet("/pages/Iletisim")]
        public IActionResult Iletisim()
        {
            var settings = _siteSettingsService.GetSettings();
            ViewBag.Telefon = settings.Telefon;
            ViewBag.Eposta = settings.Email;
            ViewBag.Adres = settings.Adres;
            ViewBag.CalismaSaatleri = settings.CalismaSaatleri;
            return View();
        }

        [HttpPost("contact")]
        [HttpPost("/Kurumsal/IletisimGonder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IletisimGonder(IletisimMesaj model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = _localizer["Kurumsal_FillAllFields"].Value });
            }

            model.Tarih = DateTime.UtcNow.AddHours(3);
            model.IpAdresi = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            model.OkunduMu = false;

            _context.IletisimMesajlari.Add(model);
            await _context.SaveChangesAsync();

            var senderName = System.Net.WebUtility.HtmlEncode(model.AdSoyad);
            var senderEmail = System.Net.WebUtility.HtmlEncode(model.Email);
            var subject = System.Net.WebUtility.HtmlEncode(model.Konu);
            var message = System.Net.WebUtility.HtmlEncode(model.Mesaj).Replace(Environment.NewLine, "<br>");
            var ipAddress = System.Net.WebUtility.HtmlEncode(model.IpAdresi);

            var adminMailIcerik = $@"
                <h3>Yeni ileti&#351;im mesaj&#305; var</h3>
                <p><b>G&ouml;nderen:</b> {senderName} ({senderEmail})</p>
                <p><b>Konu:</b> {subject}</p>
                <p><b>Mesaj:</b><br>{message}</p>
                <hr>
                <small>IP: {ipAddress}</small>";

            var siteSettings = _siteSettingsService.GetSettings();
            var recipientEmail = string.IsNullOrWhiteSpace(siteSettings.BildirimAliciEmail)
                ? siteSettings.Email
                : siteSettings.BildirimAliciEmail;

            if (!string.IsNullOrWhiteSpace(recipientEmail))
            {
                try
                {
                    await _emailService.SendTemplateMailAsync(recipientEmail, "Yeni \u0130leti\u015Fim Formu: " + model.Konu, "Operasyon Ekibi", adminMailIcerik);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Iletisim formu mail bildirimi gonderilemedi. MesajId={MesajId}, Alici={Alici}", model.Id, recipientEmail);
                }
            }

            return Json(new { success = true, message = _localizer["Kurumsal_MessageSent"].Value });
        }

        [HttpGet("faq")]
        [HttpGet("/Kurumsal/SSS")]
        public IActionResult SSS() => View();
        
        [HttpGet("bank-accounts")]
        [HttpGet("/Kurumsal/BankaHesaplari")]
        public IActionResult BankaHesaplari() => View();
        
        [HttpGet("privacy-policy")]
        [HttpGet("/Kurumsal/Gizlilik")]
        [HttpGet("/pages/Gizlilik")]
        public async Task<IActionResult> Gizlilik() => await GetDynamicOrFallbackViewAsync("gizlilik", "Gizlilik");
        
        [HttpGet("terms-of-service")]
        [HttpGet("/Kurumsal/KullaniciSozlesmesi")]
        [HttpGet("/pages/KullaniciSozlesmesi")]
        public async Task<IActionResult> KullaniciSozlesmesi() => await GetDynamicOrFallbackViewAsync("kullanici-sozlesmesi", "KullaniciSozlesmesi");
        
        [HttpGet("distance-selling-contract")]
        [HttpGet("/Kurumsal/MesafeliSatis")]
        public async Task<IActionResult> MesafeliSatis() => await GetDynamicOrFallbackViewAsync("mesafeli-satis", "MesafeliSatis");
        
        [HttpGet("return-policy")]
        [HttpGet("/Kurumsal/IadeKosullari")]
        public async Task<IActionResult> IadeKosullari() => await GetDynamicOrFallbackViewAsync("iade-kosullari", "IadeKosullari");

        private async Task<IActionResult> GetDynamicOrFallbackViewAsync(string slug, string fallbackView)
        {
            var normalizedInput = NormalizeSlug(slug);
            var normalizedFallback = NormalizeSlug(fallbackView);

            var sayfalar = await _context.KurumsalSayfalar
                .AsNoTracking()
                .Where(x => !x.SilindiMi)
                .ToListAsync();

            var sayfa = sayfalar.FirstOrDefault(x =>
            {
                var norm = NormalizeSlug(x.UrlSlug);
                return norm == normalizedInput || norm == normalizedFallback;
            });

            if (sayfa != null)
            {
                ViewData["Title"] = sayfa.LocalizedBaslik;
                return View("Detay", sayfa);
            }

            return View(fallbackView);
        }

        private static string NormalizeSlug(string? slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return string.Empty;
            }

            return slug.Trim('/', ' ').ToLowerInvariant();
        }
    }
}


