using FilistinProje.Data;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class YorumController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public YorumController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // ─── Index ────────────────────────────────────────────────────────────────

        public async Task<IActionResult> Index(int durum = 0, string? q = null, int page = 1, int pageSize = 20)
        {
            page = Math.Max(page, 1);
            pageSize = pageSize is 20 or 50 or 100 ? pageSize : 20;

            var query = BuildReviewQuery(durum, q);
            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Min(page, totalPages);

            var yorumlar = await query
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Durum = durum;
            ViewBag.Search = q?.Trim();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PendingCount = await _context.Yorumlar.CountAsync(x => !x.SilindiMi && !x.OnayliMi);
            ViewBag.ApprovedCount = await _context.Yorumlar.CountAsync(x => !x.SilindiMi && x.OnayliMi);
            ViewBag.AverageRating = await _context.Yorumlar
                .Where(x => !x.SilindiMi && x.OnayliMi)
                .Select(x => (double?)x.Puan)
                .AverageAsync() ?? 0;

            return View(yorumlar);
        }

        // ─── Export ───────────────────────────────────────────────────────────────

        public async Task<IActionResult> Export(int durum = 0, string? q = null)
        {
            var yorumlar = await BuildReviewQuery(durum, q)
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add(_localizer["Admin_ReviewsWorksheet"].Value);
            var headers = new[]
            {
                "Id",
                _localizer["Admin_Status"].Value,
                _localizer["Admin_Urun"].Value,
                _localizer["Admin_Musteri"].Value,
                _localizer["Admin_Rating"].Value,
                _localizer["Admin_Review"].Value,
                _localizer["Admin_Tarih"].Value
            };

            for (var i = 0; i < headers.Length; i++)
                ws.Cells[1, i + 1].Value = headers[i];

            using (var range = ws.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(49, 53, 17));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            for (var i = 0; i < yorumlar.Count; i++)
            {
                var y = yorumlar[i];
                var row = i + 2;
                ws.Cells[row, 1].Value = y.Id;
                ws.Cells[row, 2].Value = y.OnayliMi ? _localizer["Admin_Approved"].Value : _localizer["Admin_Pending"].Value;
                ws.Cells[row, 3].Value = y.Urun?.Baslik ?? "-";
                ws.Cells[row, 4].Value = y.AdSoyad;
                ws.Cells[row, 5].Value = y.Puan;
                ws.Cells[row, 6].Value = y.YorumMetni;
                ws.Cells[row, 7].Value = y.OlusturulmaTarihi.ToString("dd.MM.yyyy HH:mm");
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"reviews-{DateTime.Now:yyyyMMdd-HHmm}.xlsx");
        }

        // ─── Onayla / OnayiKaldir (mevcut, audit alanları temizlendi) ────────────

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(int id)
        {
            var yorum = await _context.Yorumlar.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (yorum != null)
            {
                yorum.OnayliMi = true;
                yorum.GizlemeTarihi = null;
                yorum.GizleyenKullaniciId = null;
                await _context.SaveChangesAsync();
                SetSuccess(_localizer["Admin_ReviewApprovedMessage"].Value);
            }

            return RedirectToAction(nameof(Index), new { durum = 0 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> OnayiKaldir(int id)
        {
            var yorum = await _context.Yorumlar.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (yorum != null)
            {
                yorum.OnayliMi = false;
                await _context.SaveChangesAsync();
                SetSuccess(_localizer["Admin_ReviewApprovalRemoved"].Value);
            }

            return RedirectToAction(nameof(Index), new { durum = 1 });
        }

        // ─── Gizle — OnayliMi=false + audit log ──────────────────────────────────

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Gizle(int id)
        {
            var yorum = await _context.Yorumlar.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (yorum == null)
            {
                SetError(_localizer["Admin_ReviewNotFound"].Value);
                return RedirectToAction(nameof(Index));
            }

            yorum.OnayliMi = false;
            yorum.GizlemeTarihi = DateTime.UtcNow;
            yorum.GizleyenKullaniciId = CurrentUserId();
            await _context.SaveChangesAsync();

            SetSuccess(_localizer["Admin_ReviewHidden"].Value);
            return RedirectToAction(nameof(Index), new { durum = 1 });
        }

        // ─── YayimlaTekrar — OnayliMi=true + audit temizle ───────────────────────

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> YayimlaTekrar(int id)
        {
            var yorum = await _context.Yorumlar.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (yorum == null)
            {
                SetError(_localizer["Admin_ReviewNotFound"].Value);
                return RedirectToAction(nameof(Index));
            }

            yorum.OnayliMi = true;
            yorum.GizlemeTarihi = null;
            yorum.GizleyenKullaniciId = null;
            await _context.SaveChangesAsync();

            SetSuccess(_localizer["Admin_ReviewPublished"].Value);
            return RedirectToAction(nameof(Index), new { durum = 1 });
        }

        // ─── Duzenle POST — AJAX/JSON, antiforgery, metin 5-2000 karakter ────────

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, [Bind("YorumMetni,Puan")] YorumDuzenleDto dto)
        {
            var yorumMetni = dto.YorumMetni?.Trim() ?? string.Empty;
            if (yorumMetni.Length < 5 || yorumMetni.Length > 2000)
                return Json(new { ok = false, hata = _localizer["Urun_ReviewLengthError"].Value });

            if (dto.Puan is < 1 or > 5)
                return Json(new { ok = false, hata = "Geçersiz puan (1-5)." });

            var yorum = await _context.Yorumlar.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (yorum == null)
                return Json(new { ok = false, hata = _localizer["Admin_ReviewNotFound"].Value });

            yorum.YorumMetni = yorumMetni;   // Razor encode eder, @Html.Raw kullanılmaz
            yorum.Puan = dto.Puan;
            yorum.DuzenlenmeTarihi = DateTime.UtcNow;
            yorum.DuzenleyenKullaniciId = CurrentUserId();
            await _context.SaveChangesAsync();

            return Json(new { ok = true, mesaj = _localizer["Admin_ReviewUpdated"].Value });
        }

        // ─── TopluOnayla ─────────────────────────────────────────────────────────

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluOnayla(List<int> yorumIds)
        {
            yorumIds = yorumIds.Where(x => x > 0).Distinct().ToList();
            if (!yorumIds.Any())
            {
                SetWarning(_localizer["Admin_SelectAtLeastOneReview"].Value);
                return RedirectToAction(nameof(Index), new { durum = 0 });
            }

            var yorumlar = await _context.Yorumlar
                .Where(x => yorumIds.Contains(x.Id) && !x.SilindiMi)
                .ToListAsync();

            foreach (var y in yorumlar)
                y.OnayliMi = true;

            await _context.SaveChangesAsync();

            SetSuccess(_localizer["Admin_ReviewsApprovedCount", yorumlar.Count].Value);
            return RedirectToAction(nameof(Index), new { durum = 0 });
        }

        // ─── Sil (soft-delete / arşivle) ─────────────────────────────────────────

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var yorum = await _context.Yorumlar.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (yorum != null)
            {
                yorum.SilindiMi = true;
                yorum.OnayliMi = false;
                await _context.SaveChangesAsync();
                SetSuccess(_localizer["Admin_ReviewArchived"].Value);
            }

            return RedirectToAction(nameof(Index));
        }

        // ─── Yardımcılar ──────────────────────────────────────────────────────────

        private IQueryable<FilistinProje.Core.Varliklar.Yorum> BuildReviewQuery(int durum, string? q)
        {
            var query = _context.Yorumlar
                .AsNoTracking()
                .Include(x => x.Urun)
                .Where(x => !x.SilindiMi);

            query = durum switch
            {
                1 => query.Where(x => x.OnayliMi),
                2 => query,                            // tümü (arşiv hariç)
                _ => query.Where(x => !x.OnayliMi)    // 0 = bekleyenler / gizliler
            };

            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = q.Trim().ToLower();
                query = query.Where(x =>
                    x.AdSoyad.ToLower().Contains(search) ||
                    x.YorumMetni.ToLower().Contains(search) ||
                    (x.Urun != null && x.Urun.Baslik.ToLower().Contains(search)));
            }

            return query;
        }

        private string? CurrentUserId() =>
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        private void SetSuccess(string msg) { TempData["Mesaj"] = msg; TempData["Durum"] = "success"; }
        private void SetError(string msg)   { TempData["Hata"]  = msg; TempData["Durum"] = "error"; }
        private void SetWarning(string msg) { TempData["Hata"]  = msg; TempData["Durum"] = "warning"; }
    }

    /// <summary>Overposting koruması için bind DTO — sadece YorumMetni ve Puan.</summary>
    public sealed class YorumDuzenleDto
    {
        public string? YorumMetni { get; set; }
        public int Puan { get; set; }
    }
}
