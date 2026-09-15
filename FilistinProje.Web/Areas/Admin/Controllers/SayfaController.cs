using System.Text;
using Microsoft.AspNetCore.Mvc;
using FilistinProje.Data;
using FilistinProje.Core.Varliklar;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SayfaController : AdminBaseController
    {
        private readonly KanvasDbContext _context;

        public SayfaController(KanvasDbContext context)
        {
            _context = context;
        }

        // 1. Listeleme
        public async Task<IActionResult> Index()
        {
            var sayfalar = await _context.KurumsalSayfalar.OrderBy(x => x.Sira).ToListAsync();
            return View(sayfalar);
        }

        // 2. Ekleme ve Duzenleme
        [HttpGet]
        public async Task<IActionResult> Form(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var sayfa = await _context.KurumsalSayfalar.FindAsync(id.Value);
                if (sayfa == null) return NotFound();
                if (string.IsNullOrWhiteSpace(sayfa.BaslikAr) && !string.IsNullOrWhiteSpace(sayfa.Baslik))
                {
                    sayfa.BaslikAr = sayfa.Baslik;
                }
                if (string.IsNullOrWhiteSpace(sayfa.IcerikAr) && !string.IsNullOrWhiteSpace(sayfa.Icerik))
                {
                    sayfa.IcerikAr = sayfa.Icerik;
                }
                return View(sayfa);
            }
            return View(new KurumsalSayfa());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(KurumsalSayfa model)
        {
            NormalizeContent(model);

            if (string.IsNullOrWhiteSpace(model.UrlSlug))
            {
                var baseText = !string.IsNullOrWhiteSpace(model.BaslikEn)
                    ? model.BaslikEn
                    : (!string.IsNullOrWhiteSpace(model.Baslik) ? model.Baslik : model.BaslikAr);
                model.UrlSlug = FriendlyUrl(baseText);
            }
            else
            {
                model.UrlSlug = FriendlyUrl(model.UrlSlug);
            }

            ModelState.Remove(nameof(model.UrlSlug));
            ModelState.Remove(nameof(model.Baslik));
            ModelState.Remove(nameof(model.Icerik));

            if (string.IsNullOrWhiteSpace(model.Baslik))
            {
                ModelState.AddModelError(nameof(model.Baslik), "العنوان مطلوب / Title is required");
            }
            if (string.IsNullOrWhiteSpace(model.Icerik))
            {
                ModelState.AddModelError(nameof(model.Icerik), "المحتوى مطلوب / Content is required");
            }

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var exists = await _context.KurumsalSayfalar.AnyAsync(x => x.UrlSlug == model.UrlSlug);
                    if (exists)
                    {
                        model.UrlSlug += "-" + Random.Shared.Next(100, 999);
                    }
                    _context.KurumsalSayfalar.Add(model);
                }
                else
                {
                    var mevcut = await _context.KurumsalSayfalar.FindAsync(model.Id);
                    if (mevcut == null)
                    {
                        return NotFound();
                    }

                    mevcut.Baslik = model.Baslik;
                    mevcut.Icerik = model.Icerik;
                    mevcut.BaslikEn = model.BaslikEn;
                    mevcut.BaslikAr = model.BaslikAr;
                    mevcut.IcerikEn = model.IcerikEn;
                    mevcut.IcerikAr = model.IcerikAr;
                    mevcut.Sira = model.Sira;
                    if (!string.IsNullOrWhiteSpace(model.UrlSlug))
                    {
                        if (mevcut.UrlSlug != model.UrlSlug)
                        {
                            var exists = await _context.KurumsalSayfalar.AnyAsync(x => x.UrlSlug == model.UrlSlug && x.Id != model.Id);
                            if (exists)
                            {
                                model.UrlSlug += "-" + Random.Shared.Next(100, 999);
                            }
                            mevcut.UrlSlug = model.UrlSlug;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // 3. Silme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var sayfa = await _context.KurumsalSayfalar.FindAsync(id);
            if (sayfa != null)
            {
                _context.KurumsalSayfalar.Remove(sayfa);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private static string FriendlyUrl(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Guid.NewGuid().ToString("N")[..8];

            var normalized = text.Trim().ToLowerInvariant()
                .Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u")
                .Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");

            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    sb.Append(ch);
                }
                else if (ch == ' ' || ch == '-' || ch == '_')
                {
                    if (sb.Length > 0 && sb[^1] != '-')
                    {
                        sb.Append('-');
                    }
                }
            }

            var slug = sb.ToString().Trim('-');
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = "page-" + Random.Shared.Next(100, 999);
            }
            return slug;
        }

        private static void NormalizeContent(KurumsalSayfa model)
        {
            model.Baslik = model.Baslik?.Trim() ?? string.Empty;
            model.Icerik = model.Icerik?.Trim() ?? string.Empty;
            model.BaslikEn = model.BaslikEn?.Trim() ?? string.Empty;
            model.BaslikAr = model.BaslikAr?.Trim() ?? string.Empty;
            model.IcerikEn = model.IcerikEn?.Trim() ?? string.Empty;
            model.IcerikAr = model.IcerikAr?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(model.Baslik))
            {
                model.Baslik = !string.IsNullOrWhiteSpace(model.BaslikAr) ? model.BaslikAr : model.BaslikEn;
            }
            if (string.IsNullOrWhiteSpace(model.BaslikAr))
            {
                model.BaslikAr = model.Baslik;
            }
            if (string.IsNullOrWhiteSpace(model.BaslikEn))
            {
                model.BaslikEn = model.Baslik;
            }
            if (string.IsNullOrWhiteSpace(model.Icerik))
            {
                model.Icerik = !string.IsNullOrWhiteSpace(model.IcerikAr) ? model.IcerikAr : model.IcerikEn;
            }
            if (string.IsNullOrWhiteSpace(model.IcerikAr))
            {
                model.IcerikAr = model.Icerik;
            }
            if (string.IsNullOrWhiteSpace(model.IcerikEn))
            {
                model.IcerikEn = model.Icerik;
            }
        }
    }
}


