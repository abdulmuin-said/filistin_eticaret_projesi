using FilistinProje.Data;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IletisimController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public IletisimController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool unreadOnly = false, string? q = null)
        {
            var query = _context.IletisimMesajlari
                .AsNoTracking()
                .AsQueryable();

            if (unreadOnly)
            {
                query = query.Where(x => !x.OkunduMu);
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = q.Trim().ToLower();
                query = query.Where(x =>
                    x.AdSoyad.ToLower().Contains(search) ||
                    x.Email.ToLower().Contains(search) ||
                    x.Konu.ToLower().Contains(search) ||
                    x.Mesaj.ToLower().Contains(search));
            }

            ViewBag.UnreadOnly = unreadOnly;
            ViewBag.Search = q?.Trim();
            ViewBag.TotalCount = await _context.IletisimMesajlari.CountAsync();
            ViewBag.UnreadCount = await _context.IletisimMesajlari.CountAsync(x => !x.OkunduMu);
            ViewBag.ReadCount = await _context.IletisimMesajlari.CountAsync(x => x.OkunduMu);
            ViewBag.TodayCount = await _context.IletisimMesajlari.CountAsync(x => x.Tarih >= DateTime.UtcNow.Date);

            var messages = await query
                .OrderBy(x => x.OkunduMu)
                .ThenByDescending(x => x.Tarih)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OkunduYap(int id)
        {
            var message = await _context.IletisimMesajlari.FirstOrDefaultAsync(x => x.Id == id);
            if (message != null)
            {
                message.OkunduMu = true;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = _localizer["Admin_ContactMarkedRead"].Value;
                TempData["Durum"] = "success";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OkunmadiYap(int id)
        {
            var message = await _context.IletisimMesajlari.FirstOrDefaultAsync(x => x.Id == id);
            if (message != null)
            {
                message.OkunduMu = false;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = _localizer["Admin_ContactMarkedUnread"].Value;
                TempData["Durum"] = "success";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluOkunduYap(List<int> mesajIds)
        {
            mesajIds = mesajIds.Where(x => x > 0).Distinct().ToList();
            if (!mesajIds.Any())
            {
                TempData["Hata"] = _localizer["Admin_SelectAtLeastOneMessage"].Value;
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var messages = await _context.IletisimMesajlari
                .Where(x => mesajIds.Contains(x.Id))
                .ToListAsync();

            foreach (var message in messages)
            {
                message.OkunduMu = true;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = _localizer["Admin_MessagesMarkedRead", messages.Count].Value;
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detay(int id)
        {
            var message = await _context.IletisimMesajlari.FirstOrDefaultAsync(x => x.Id == id);
            if (message == null)
            {
                TempData["Hata"] = _localizer["Admin_MessageNotFound"].Value;
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var message = await _context.IletisimMesajlari.FirstOrDefaultAsync(x => x.Id == id);
            if (message != null)
            {
                _context.IletisimMesajlari.Remove(message);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = _localizer["Admin_ContactMessageDeleted"].Value;
                TempData["Durum"] = "success";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
