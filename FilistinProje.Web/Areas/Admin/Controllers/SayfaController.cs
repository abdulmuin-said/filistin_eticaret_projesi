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

        // 1. LÄ°STELEME
        public async Task<IActionResult> Index()
        {
            var sayfalar = await _context.KurumsalSayfalar.OrderBy(x => x.Sira).ToListAsync();
            return View(sayfalar);
        }

        // 2. EKLEME VE DÃœZENLEME (Tek Action'da halledelim)
        [HttpGet]
        public async Task<IActionResult> Form(int? id)
        {
            if (id.HasValue) // DÃ¼zenleme Modu
            {
                var sayfa = await _context.KurumsalSayfalar.FindAsync(id.Value);
                if (sayfa == null) return NotFound();
                return View(sayfa);
            }
            return View(new KurumsalSayfa()); // Ekleme Modu (BoÅŸ model)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(KurumsalSayfa model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0) // Yeni KayÄ±t
                {
                    model.UrlSlug = FriendlyUrl(model.Baslik); // Link oluÅŸtur
                    _context.KurumsalSayfalar.Add(model);
                }
                else // GÃ¼ncelleme
                {
                    var mevcut = await _context.KurumsalSayfalar.FindAsync(model.Id);
                    if (mevcut != null)
                    {
                        mevcut.Baslik = model.Baslik;
                        mevcut.Icerik = model.Icerik;
                        mevcut.Sira = model.Sira;
                        // UrlSlug'Ä± gÃ¼ncellemiyoruz ki Google'daki linkler kÄ±rÄ±lmasÄ±n
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 3. SÄ°LME
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var sayfa = await _context.KurumsalSayfalar.FindAsync(id);
            if(sayfa != null)
            {
                _context.KurumsalSayfalar.Remove(sayfa);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // YardÄ±mcÄ±: URL Dostu Ä°sim OluÅŸturucu (Ã–rn: "Gizlilik PolitikasÄ±" -> "gizlilik-politikasi")
        private string FriendlyUrl(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.ToLower()
                .Replace("Ä±", "i").Replace("ÄŸ", "g").Replace("Ã¼", "u")
                .Replace("ÅŸ", "s").Replace("Ã¶", "o").Replace("Ã§", "c")
                .Replace(" ", "-").Replace(".", "").Replace("/", "")
                + "-" + new Random().Next(100,999); // Sonuna rastgele sayÄ± ekledim ki Ã§akÄ±ÅŸma olmasÄ±n
        }
    }
}


