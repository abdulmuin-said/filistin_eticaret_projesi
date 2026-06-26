using System.Collections.Generic; // cache-invalidation comment to trigger dotnet watch reload
using System.Diagnostics;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Core.Helpers;
using FilistinProje.Web.Models;
using FilistinProje.Web.Resources;
using FilistinProje.Service.Services;
using FilistinProje.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Net.Mail;


namespace FilistinProje.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KanvasDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IHomePageSectionService _homePageSectionService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public HomeController(
            ILogger<HomeController> logger,
            KanvasDbContext context,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            IHomePageSectionService homePageSectionService,
            IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
            _homePageSectionService = homePageSectionService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var aktifUrunler = _context.Urunler
                .AsNoTracking()
                .Include(u => u.Kategori)
                .Include(u => u.UrunSecenek)
                .Include(u => u.UrunResimleri)
                .Where(u =>
                    u.AktifMi &&
                    u.YayindaMi &&
                    !u.SilindiMi &&
                    u.Kategori != null &&
                    u.Kategori.AktifMi);

            var secilenUrunler = new HashSet<int>();

            var besParcaliKoleksiyon = await aktifUrunler
                .Where(u =>
                    (EF.Functions.ILike(u.Baslik, "%5 ParÃ§a%") ||
                     EF.Functions.ILike(u.Baslik, "%5 ParÃ§alÄ±%") ||
                     EF.Functions.ILike(u.Aciklama ?? string.Empty, "%5 ParÃ§a%") ||
                     EF.Functions.ILike(u.Etiketler ?? string.Empty, "%5 ParÃ§a%")) &&
                    (EF.Functions.ILike(u.Baslik, "%gÃ¼zel ahlak%") ||
                     EF.Functions.ILike(u.Baslik, "%islam%") ||
                     EF.Functions.ILike(u.Baslik, "%islami%") ||
                     EF.Functions.ILike(u.Baslik, "%tarihi%") ||
                     EF.Functions.ILike(u.Baslik, "%osmanlÄ±%") ||
                     EF.Functions.ILike(u.Baslik, "%ayet%") ||
                     EF.Functions.ILike(u.Baslik, "%hadis%") ||
                     EF.Functions.ILike(u.Etiketler ?? string.Empty, "%islam%") ||
                     EF.Functions.ILike(u.Etiketler ?? string.Empty, "%tarihi%") ||
                     (u.Kategori != null &&
                      (EF.Functions.ILike(u.Kategori.Ad, "%islam%") ||
                       EF.Functions.ILike(u.Kategori.Ad, "%tarihi%")))))
                .OrderBy(u => u.Sira)
                .ThenByDescending(u => u.OneCikanMi)
                .ThenByDescending(u => u.GoruntulenmeSayisi)
                .ThenByDescending(u => u.OlusturulmaTarihi)
                .Take(15)
                .ToListAsync();

            secilenUrunler.UnionWith(besParcaliKoleksiyon.Select(x => x.Id));
            await TamamlayiciUrunleriEkleAsync(
                besParcaliKoleksiyon,
                aktifUrunler
                    .Where(u =>
                        EF.Functions.ILike(u.Baslik, "%5 ParÃ§a%") ||
                        EF.Functions.ILike(u.Baslik, "%5 ParÃ§alÄ±%") ||
                        EF.Functions.ILike(u.Aciklama ?? string.Empty, "%5 ParÃ§a%") ||
                        EF.Functions.ILike(u.Etiketler ?? string.Empty, "%5 ParÃ§a%"))
                    .OrderBy(u => u.Sira)
                    .ThenByDescending(u => u.GoruntulenmeSayisi)
                    .ThenByDescending(u => u.OlusturulmaTarihi),
                secilenUrunler,
                15);

            var vitrinUrunleri = await aktifUrunler
                .Where(u => (u.AnaSayfadaGoster || u.OneCikanMi || u.YeniUrunMu) && !secilenUrunler.Contains(u.Id))
                .OrderBy(u => u.Sira)
                .ThenByDescending(u => u.OneCikanMi)
                .ThenByDescending(u => u.OlusturulmaTarihi)
                .Take(10)
                .ToListAsync();

            secilenUrunler.UnionWith(vitrinUrunleri.Select(x => x.Id));
            await TamamlayiciUrunleriEkleAsync(
                vitrinUrunleri,
                aktifUrunler.OrderBy(u => u.Sira).ThenByDescending(u => u.OlusturulmaTarihi),
                secilenUrunler,
                10);

            var cokSatanlar = await aktifUrunler
                .Where(u => (u.SatisSayisi > 0 || u.OneCikanMi) && !secilenUrunler.Contains(u.Id))
                .OrderByDescending(u => u.SatisSayisi)
                .ThenByDescending(u => u.GoruntulenmeSayisi)
                .ThenBy(u => u.Sira)
                .Take(8)
                .ToListAsync();

            secilenUrunler.UnionWith(cokSatanlar.Select(x => x.Id));
            await TamamlayiciUrunleriEkleAsync(
                cokSatanlar,
                aktifUrunler.OrderByDescending(u => u.SatisSayisi).ThenBy(u => u.Sira),
                secilenUrunler,
                8);

            var firsatUrunleri = await aktifUrunler
                .Where(u =>
                    (u.KampanyaliMi || (u.IndirimliFiyat.HasValue && u.IndirimliFiyat > 0 && u.IndirimliFiyat < u.Fiyat)) &&
                    !secilenUrunler.Contains(u.Id))
                .OrderByDescending(u => u.KampanyaliMi)
                .ThenBy(u => u.IndirimliFiyat ?? u.Fiyat)
                .ThenBy(u => u.Sira)
                .Take(8)
                .ToListAsync();

            secilenUrunler.UnionWith(firsatUrunleri.Select(x => x.Id));
            await TamamlayiciUrunleriEkleAsync(
                firsatUrunleri,
                aktifUrunler.OrderBy(u => u.IndirimliFiyat ?? u.Fiyat).ThenBy(u => u.Sira),
                secilenUrunler,
                8);

var kategoriler = await _context.Kategoriler
                .AsNoTracking()
                .Where(k => k.AktifMi && !k.SilindiMi && k.ParentKategoriId == null)
                .OrderBy(k => k.Sira)
                .ThenBy(k => k.Ad)
                .Take(18)
                .ToListAsync();

            var aktifSlaytlar = await _context.Slaytlar
                .AsNoTracking()
                .Where(s => s.AktifMi)
                .OrderBy(s => s.Sira)
                .ToListAsync();

            var sections = await _homePageSectionService.GetActiveSectionsAsync();

            var viewModel = new HomeViewModel
            {
                VitrinUrunleri = vitrinUrunleri,
                CokSatanlar = cokSatanlar,
                BesParcaliKoleksiyon = besParcaliKoleksiyon,
                FirsatUrunleri = firsatUrunleri,
                Kategoriler = kategoriler,
                Sections = sections,
                AktifSlaytlar = aktifSlaytlar
            };

            return View(viewModel);
        }

        private static async Task TamamlayiciUrunleriEkleAsync(
            List<Urun> hedefListe,
            IOrderedQueryable<Urun> kaynakSorgu,
            HashSet<int> secilenUrunler,
            int hedefAdet)
        {
            if (hedefListe.Count >= hedefAdet)
            {
                return;
            }

            var eksikAdet = hedefAdet - hedefListe.Count;
            var ekUrunler = await kaynakSorgu
                .Where(x => !secilenUrunler.Contains(x.Id))
                .Take(eksikAdet)
                .ToListAsync();

            hedefListe.AddRange(ekUrunler);
            secilenUrunler.UnionWith(ekUrunler.Select(x => x.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulteneAboneOl(string email)
        {
            email = (email ?? string.Empty).Trim().ToLowerInvariant();

            if (!GecerliEmailMi(email))
            {
                return Json(new { success = false, message = _localizer["Home_InvalidEmail"].Value });
            }

            try
            {
                var mevcutAbone = await _context.BultenAbonelikleri.FirstOrDefaultAsync(x => x.Email == email);
            if (mevcutAbone?.AktifMi == true)
            {
                return Json(new { success = false, message = _localizer["Home_AlreadySubscribed"].Value });
            }

            if (mevcutAbone != null)
            {
                mevcutAbone.AktifMi = true;
                mevcutAbone.KayitTarihi = DateTime.UtcNow.AddHours(3);
                mevcutAbone.IpAdresi = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            else
            {
                var yeniAbone = new BultenAboneligi
                {
                    Email = email,
                    KayitTarihi = DateTime.UtcNow.AddHours(3),
                    AktifMi = true,
                    IpAdresi = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                _context.BultenAbonelikleri.Add(yeniAbone);
            }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bulten aboneligi kaydedilemedi. Email: {Email}", email);
                return Json(new { success = false, message = _localizer["Home_SubscriptionFailed"].Value });
            }

            var settings = _siteSettingsService.GetSettings();
            var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
            var siteUrl = _siteSettingsService.BuildAbsoluteUrl(string.Empty);

            try
            {
                await _emailService.SendTemplateMailAsync(
                    email,
                    _localizer["Home_NewsletterSubject"].Value,
                    _localizer["Siparis_EmailDearCustomer"].Value,
                    string.Format(_localizer["Home_NewsletterWelcome"].Value, brandName),
                    siteUrl,
                    _localizer["Home_BrowseSite"].Value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Bulten hos geldiniz e-postasi gonderilemedi. Email: {Email}", email);
            }

            try
            {
                var recipientEmail = string.IsNullOrWhiteSpace(settings.BildirimAliciEmail)
                    ? "info@7anrps48.com"
                    : settings.BildirimAliciEmail;
                var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "-";
                var adminMailIcerik = $@"
                    <p>{_localizer["Home_AdminNewSubscriberBody"].Value}</p>
                    <p><b>Abone e-posta:</b> {WebUtility.HtmlEncode(email)}</p>
                    <p><b>IP:</b> {WebUtility.HtmlEncode(ipAddress)}</p>";

                await _emailService.SendTemplateMailAsync(
                    recipientEmail,
                    _localizer["Home_AdminNewSubscriber"].Value,
                    "Operasyon Ekibi",
                    adminMailIcerik,
                    siteUrl,
                    _localizer["Home_BrowseSite"].Value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Bulten admin bildirimi gonderilemedi. Email: {Email}", email);
            }

            return Json(new { success = true, message = _localizer["Home_SubscriptionSuccess"].Value });
        }

        private static bool GecerliEmailMi(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var adres = new MailAddress(email);
                return string.Equals(adres.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> SiteDegerlendir()
        {
            var degerlendirmeler = await _context.SiteDegerlendirmeleri
                .AsNoTracking()
                .Where(x => x.OnayliMi && !x.SilindiMi)
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .Take(10)
                .ToListAsync();

            ViewBag.Degerlendirmeler = degerlendirmeler;
            ViewBag.OrtalamaPuan = degerlendirmeler.Count > 0
                ? degerlendirmeler.Average(x => x.Puan)
                : 0;
            ViewBag.ToplamDegerlendirme = degerlendirmeler.Count;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SiteDegerlendir([Bind("AdSoyad,Eposta,Puan,Baslik,YorumMetni")] SiteDegerlendirme model)
        {
            if (string.IsNullOrWhiteSpace(model.AdSoyad) || string.IsNullOrWhiteSpace(model.YorumMetni))
            {
                TempData["Hata"] = _localizer["Home_ReviewNameCommentRequired"].Value;
                return RedirectToAction("SiteDegerlendir");
            }

            model.OlusturulmaTarihi = DateTime.UtcNow;
            model.OnayliMi = false;
            model.SilindiMi = false;

            _context.SiteDegerlendirmeleri.Add(model);
            await _context.SaveChangesAsync();

            TempData["Basari"] = _localizer["Home_ReviewReceived"].Value;
            return RedirectToAction("SiteDegerlendir");
        }
    }
}
