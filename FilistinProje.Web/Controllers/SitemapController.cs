using Microsoft.AspNetCore.Mvc;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using FilistinProje.Service.Services;
using System.Net;

namespace FilistinProje.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly KanvasDbContext _context;
        private readonly ISiteSettingsService _siteSettingsService;

        public SitemapController(KanvasDbContext context, ISiteSettingsService siteSettingsService)
        {
            _context = context;
            _siteSettingsService = siteSettingsService;
        }

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var baseUrl = _siteSettingsService.BuildAbsoluteUrl(string.Empty).TrimEnd('/');
            var sitemap = new StringBuilder();
            sitemap.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sitemap.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Ana sayfa
            AppendUrl(sitemap, baseUrl + "/", "daily", "1.0", DateTime.UtcNow);

            // Statik kurumsal sayfalar
            var staticPages = new (string Url, string Changefreq, string Priority)[]
            {
                ("/pages/about", "monthly", "0.6"),
                ("/pages/contact", "monthly", "0.6"),
                ("/pages/faq", "monthly", "0.5"),
                ("/pages/privacy", "monthly", "0.4"),
                ("/pages/terms", "monthly", "0.4"),
                ("/pages/distance-sales", "monthly", "0.4"),
                ("/pages/return-policy", "monthly", "0.4"),
                ("/products", "daily", "0.9"),
                ("/favorites", "weekly", "0.5"),
            };

            foreach (var (url, freq, priority) in staticPages)
            {
                AppendUrl(sitemap, baseUrl + url, freq, priority, DateTime.UtcNow);
            }

            // Kategoriler
            var kategoriler = await _context.Kategoriler
                .AsNoTracking()
                .Where(k => !k.SilindiMi && k.AktifMi)
                .ToListAsync();

            foreach (var kategori in kategoriler)
            {
                AppendUrl(sitemap, $"{baseUrl}/products?k={kategori.Id}", "weekly", "0.8", DateTime.UtcNow);
            }

            // ÃœrÃ¼nler (ilk 2000 Ã¼rÃ¼n - performans iÃ§in)
            var urunler = await _context.Urunler
                .AsNoTracking()
                .Where(u => !u.SilindiMi && u.AktifMi && u.YayindaMi)
                .OrderByDescending(u => u.OlusturulmaTarihi)
                .Take(2000)
                .ToListAsync();

            foreach (var urun in urunler)
            {
                var detailSegment = string.IsNullOrWhiteSpace(urun.Slug) ? urun.Id.ToString() : $"{urun.Slug}-{urun.Id}";
                var lastMod = urun.OlusturulmaTarihi;
                AppendUrl(sitemap, $"{baseUrl}/products/{detailSegment}", "monthly", "0.7", lastMod);
            }

            sitemap.AppendLine("</urlset>");

            return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
        }

        private static void AppendUrl(StringBuilder sb, string loc, string changefreq, string priority, DateTime? lastmod)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{XmlEncode(loc)}</loc>");
            if (lastmod.HasValue)
                sb.AppendLine($"    <lastmod>{lastmod.Value:yyyy-MM-dd}</lastmod>");
            sb.AppendLine($"    <changefreq>{changefreq}</changefreq>");
            sb.AppendLine($"    <priority>{priority}</priority>");
            sb.AppendLine("  </url>");
        }

        private static string XmlEncode(string value) => WebUtility.HtmlEncode(value);
    }
}


