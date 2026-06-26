using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    public class PushBildirimController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IFirebaseNotificationService _firebase;

        public PushBildirimController(
            KanvasDbContext context,
            IStringLocalizer<SharedResource> localizer,
            IFirebaseNotificationService firebase)
        {
            _context = context;
            _localizer = localizer;
            _firebase = firebase;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AboneSayisi = await _firebase.GetActiveSubscriptionCountAsync();
            ViewBag.FirebaseAyarlariTam = _firebase.IsAvailable;

            var abonelikler = await _context.PushAbonelikleri
                .Include(p => p.AppUser)
                .Where(p => !p.SilindiMi)
                .OrderByDescending(p => p.OlusturulmaTarihi)
                .Take(200)
                .ToListAsync();

            return View(abonelikler);
        }

        [HttpGet]
        public IActionResult Gonder()
        {
            ViewBag.AboneSayisi = _firebase.IsAvailable
                ? _context.PushAbonelikleri.Count(p => p.AktifMi && !p.SilindiMi)
                : 0;
            ViewBag.FirebaseAyarlariTam = _firebase.IsAvailable;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Gonder(string title, string body, string? clickUrl)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                ViewBag.Hata = _localizer["Admin_PushValidationError"];
                return View();
            }

            var successCount = await _firebase.SendBulkPushAsync(title, body, clickUrl);
            TempData["Success"] = string.Format(_localizer["Admin_PushSent"], successCount);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> AboneOl([FromBody] PushAboneRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
                return Json(new { success = false });

            var userId = HttpContext.User?.Identity?.IsAuthenticated == true
                ? HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            await _firebase.SubscribeAsync(request.token, userId, request.tarayici, request.platform);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> AboneliktenCik([FromBody] PushAboneRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
                return Json(new { success = false });

            await _firebase.UnsubscribeAsync(request.token);
            return Json(new { success = true });
        }
    }

    public class PushAboneRequest
    {
        public string? token { get; set; }
        public string? tarayici { get; set; }
        public string? platform { get; set; }
    }
}
