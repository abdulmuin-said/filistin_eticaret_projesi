using FilistinProje.Core.Models;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net;
using System.Net.Mail;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BultenController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingsService _siteSettingsService;

        public BultenController(KanvasDbContext context, IEmailService emailService, ISiteSettingsService siteSettingsService)
        {
            _context = context;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
        }

        public async Task<IActionResult> Index(int durum = 1, string? q = null, int page = 1, int pageSize = 20)
        {
            page = Math.Max(page, 1);
            pageSize = pageSize is 20 or 50 or 100 ? pageSize : 20;

            var query = BuildSubscriberQuery(durum, q);
            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Min(page, totalPages);

            var aboneler = await query
                .OrderByDescending(x => x.KayitTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = await BuildSubscriberListAsync(aboneler);

            ViewBag.Durum = durum;
            ViewBag.Search = q?.Trim();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.ActiveCount = await _context.BultenAbonelikleri.CountAsync(x => x.AktifMi);
            ViewBag.PassiveCount = await _context.BultenAbonelikleri.CountAsync(x => !x.AktifMi);

            var todayStart = DateTime.UtcNow.Date.AddHours(3);
            var tomorrowStart = todayStart.AddDays(1);
            ViewBag.TodayCount = await _context.BultenAbonelikleri.CountAsync(x => x.KayitTarihi >= todayStart && x.KayitTarihi < tomorrowStart);

            return View(model);
        }

        public async Task<IActionResult> Export(int durum = 1, string? q = null)
        {
            var aboneler = await BuildSubscriberQuery(durum, q)
                .OrderByDescending(x => x.KayitTarihi)
                .ToListAsync();
            var model = await BuildSubscriberListAsync(aboneler);

            ExcelPackage.License.SetNonCommercialOrganization("7ANRPS48");
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("BÃ¼lten Aboneleri");
            var headers = new[] { "Id", "Durum", "E-Posta", "KayÄ±t Tarihi", "IP Adresi", "Åehir", "Ãœlke", "Cihaz", "TarayÄ±cÄ±", "Ä°ÅŸletim Sistemi" };

            for (var i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            using (var range = worksheet.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(49, 53, 17));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            for (var i = 0; i < model.Count; i++)
            {
                var item = model[i];
                var row = i + 2;
                worksheet.Cells[row, 1].Value = item.Id;
                worksheet.Cells[row, 2].Value = item.AktifMi ? "Aktif" : "Pasif";
                worksheet.Cells[row, 3].Value = item.Email;
                worksheet.Cells[row, 4].Value = item.KayitTarihi.ToString("dd.MM.yyyy HH:mm");
                worksheet.Cells[row, 5].Value = item.IpAdresi;
                worksheet.Cells[row, 6].Value = item.Sehir;
                worksheet.Cells[row, 7].Value = item.Ulke;
                worksheet.Cells[row, 8].Value = item.Cihaz;
                worksheet.Cells[row, 9].Value = item.Tarayici;
                worksheet.Cells[row, 10].Value = item.IsletimSistemi;
            }

            if (worksheet.Dimension != null)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"bulten-aboneleri-{DateTime.Now:yyyyMMdd-HHmm}.xlsx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasifYap(int id, int durum = 1, string? q = null, int page = 1, int pageSize = 20)
        {
            var abone = await _context.BultenAbonelikleri.FirstOrDefaultAsync(x => x.Id == id);
            if (abone != null)
            {
                abone.AktifMi = false;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "BÃ¼lten aboneliÄŸi pasif hale getirildi.";
                TempData["Durum"] = "success";
            }

            return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AktifYap(int id, int durum = 0, string? q = null, int page = 1, int pageSize = 20)
        {
            var abone = await _context.BultenAbonelikleri.FirstOrDefaultAsync(x => x.Id == id);
            if (abone != null)
            {
                abone.AktifMi = true;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "BÃ¼lten aboneliÄŸi tekrar aktif hale getirildi.";
                TempData["Durum"] = "success";
            }

            return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluPasifYap(List<int> aboneIds, int durum = 1, string? q = null, int page = 1, int pageSize = 20)
        {
            aboneIds = aboneIds.Where(x => x > 0).Distinct().ToList();
            if (!aboneIds.Any())
            {
                TempData["Hata"] = "Ä°ÅŸlem yapmak iÃ§in en az bir abone seÃ§in.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
            }

            var aboneler = await _context.BultenAbonelikleri
                .Where(x => aboneIds.Contains(x.Id))
                .ToListAsync();

            foreach (var abone in aboneler)
            {
                abone.AktifMi = false;
            }

            await _context.SaveChangesAsync();
            TempData["Mesaj"] = $"{aboneler.Count} bÃ¼lten aboneliÄŸi pasif hale getirildi.";
            TempData["Durum"] = "success";

            return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluMailGonder(
            string baslik,
            string icerik,
            string? butonLink,
            string? butonYazi,
            string hedef = "aktif",
            List<int>? mailAboneIds = null,
            int durum = 1,
            string? q = null,
            int page = 1,
            int pageSize = 20)
        {
            baslik = (baslik ?? string.Empty).Trim();
            icerik = (icerik ?? string.Empty).Trim();
            butonLink = (butonLink ?? string.Empty).Trim();
            butonYazi = (butonYazi ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(icerik))
            {
                TempData["Hata"] = "Mail baÅŸlÄ±ÄŸÄ± ve mesaj iÃ§eriÄŸi zorunludur.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
            }

            IQueryable<BultenAboneligi> query;
            if (hedef == "secili")
            {
                mailAboneIds = (mailAboneIds ?? new List<int>()).Where(x => x > 0).Distinct().ToList();
                if (!mailAboneIds.Any())
                {
                    TempData["Hata"] = "SeÃ§ili abonelere mail gÃ¶ndermek iÃ§in en az bir abone seÃ§in.";
                    TempData["Durum"] = "warning";
                    return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
                }

                query = _context.BultenAbonelikleri.Where(x => x.AktifMi && mailAboneIds.Contains(x.Id));
            }
            else if (hedef == "filtre")
            {
                query = BuildSubscriberQuery(durum, q).Where(x => x.AktifMi);
            }
            else
            {
                query = _context.BultenAbonelikleri.Where(x => x.AktifMi);
            }

            var aboneler = await query
                .AsNoTracking()
                .Select(x => x.Email)
                .Distinct()
                .ToListAsync();

            if (!aboneler.Any())
            {
                TempData["Hata"] = "GÃ¶nderilecek aktif bÃ¼lten abonesi bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
            }

            var htmlIcerik = BuildNewsletterHtml(icerik);
            var normalizedButtonLink = string.IsNullOrWhiteSpace(butonLink)
                ? string.Empty
                : _siteSettingsService.BuildAbsoluteUrl(butonLink);
            var sent = 0;
            var failed = 0;

            foreach (var email in aboneler)
            {
                if (!IsValidEmail(email))
                {
                    failed++;
                    continue;
                }

                try
                {
                    await _emailService.SendTemplateMailAsync(
                        email,
                        baslik,
                        "DeÄŸerli Abonemiz",
                        htmlIcerik,
                        normalizedButtonLink,
                        string.IsNullOrWhiteSpace(butonYazi) ? "Koleksiyonu Ä°ncele" : butonYazi);
                    sent++;
                }
                catch
                {
                    failed++;
                }
            }

            TempData["Mesaj"] = failed == 0
                ? $"BÃ¼lten maili {sent} aktif aboneye gÃ¶nderildi."
                : $"BÃ¼lten maili {sent} aboneye gÃ¶nderildi, {failed} gÃ¶nderim baÅŸarÄ±sÄ±z oldu.";
            TempData["Durum"] = failed == 0 ? "success" : "warning";

            return RedirectToAction(nameof(Index), new { durum, q, page, pageSize });
        }

        private IQueryable<BultenAboneligi> BuildSubscriberQuery(int durum, string? q)
        {
            var query = _context.BultenAbonelikleri.AsNoTracking().AsQueryable();

            query = durum switch
            {
                0 => query,
                2 => query.Where(x => !x.AktifMi),
                _ => query.Where(x => x.AktifMi)
            };

            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = q.Trim().ToLower();
                query = query.Where(x =>
                    x.Email.ToLower().Contains(search) ||
                    (x.IpAdresi != null && x.IpAdresi.ToLower().Contains(search)));
            }

            return query;
        }

        private static string BuildNewsletterHtml(string content)
        {
            var encoded = WebUtility.HtmlEncode(content.Trim());
            var paragraphs = encoded
                .Replace("\r\n", "\n")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => $"<p>{x}</p>");

            return string.Join("", paragraphs);
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

        private async Task<List<BultenListViewModel>> BuildSubscriberListAsync(List<BultenAboneligi> aboneler)
        {
            var ips = aboneler
                .Select(x => x.IpAdresi)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var logs = await _context.ZiyaretciLoglari
                .AsNoTracking()
                .Where(x => x.IpAdresi != null && ips.Contains(x.IpAdresi))
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .ToListAsync();

            var latestLogsByIp = logs
                .GroupBy(x => x.IpAdresi)
                .ToDictionary(x => x.Key!, x => x.First());

            return aboneler.Select(abone =>
            {
                latestLogsByIp.TryGetValue(abone.IpAdresi ?? string.Empty, out var logDetay);

                return new BultenListViewModel
                {
                    Id = abone.Id,
                    Email = abone.Email,
                    KayitTarihi = abone.KayitTarihi,
                    AktifMi = abone.AktifMi,
                    IpAdresi = string.IsNullOrWhiteSpace(abone.IpAdresi) ? "Bilinmiyor" : abone.IpAdresi,
                    Sehir = string.IsNullOrWhiteSpace(logDetay?.Sehir) ? "-" : logDetay.Sehir,
                    Ulke = string.IsNullOrWhiteSpace(logDetay?.Ulke) ? "-" : logDetay.Ulke,
                    Cihaz = string.IsNullOrWhiteSpace(logDetay?.CihazModeli) ? "Bilinmiyor" : logDetay.CihazModeli,
                    IsletimSistemi = string.IsNullOrWhiteSpace(logDetay?.IsletimSistemi) ? "-" : logDetay.IsletimSistemi,
                    Tarayici = string.IsNullOrWhiteSpace(logDetay?.Tarayici) ? "-" : logDetay.Tarayici
                };
            }).ToList();
        }
    }
}

