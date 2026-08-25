using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using FilistinProje.Web.Resources;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SosyalMedyaController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SosyalMedyaController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET /Admin/SosyalMedya
        public async Task<IActionResult> Index()
        {
            var linkler = await _context.SosyalMedyaLinkleri
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Id)
                .ToListAsync();
            return View(linkler);
        }

        // POST /Admin/SosyalMedya/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(
            string platformAdi, string url, string ikonSinifi, int sira, bool aktifMi)
        {
            if (!IsGecerliUrl(url))
            {
                TempData["Hata"] = _localizer["Admin_SosyalMedya_GeçersizUrl"].Value;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(platformAdi))
            {
                TempData["Hata"] = _localizer["Admin_SosyalMedya_PlatformZorunlu"].Value;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            _context.SosyalMedyaLinkleri.Add(new SosyalMedyaLink
            {
                PlatformAdi = platformAdi.Trim(),
                Url = url.Trim(),
                IkonSinifi = (ikonSinifi ?? string.Empty).Trim(),
                Sira = sira,
                AktifMi = aktifMi,
                OlusturulmaTarihi = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            TempData["Basari"] = _localizer["Admin_SosyalMedya_Eklendi"].Value;
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // POST /Admin/SosyalMedya/Guncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guncelle(
            int id, string platformAdi, string url, string ikonSinifi, int sira, bool aktifMi)
        {
            var link = await _context.SosyalMedyaLinkleri.FindAsync(id);
            if (link == null)
            {
                TempData["Hata"] = _localizer["Admin_SosyalMedya_GeçersizUrl"].Value;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            if (!IsGecerliUrl(url))
            {
                TempData["Hata"] = _localizer["Admin_SosyalMedya_GeçersizUrl"].Value;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(platformAdi))
            {
                TempData["Hata"] = _localizer["Admin_SosyalMedya_PlatformZorunlu"].Value;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            link.PlatformAdi = platformAdi.Trim();
            link.Url = url.Trim();
            link.IkonSinifi = (ikonSinifi ?? string.Empty).Trim();
            link.Sira = sira;
            link.AktifMi = aktifMi;

            await _context.SaveChangesAsync();
            TempData["Basari"] = _localizer["Admin_SosyalMedya_Guncellendi"].Value;
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        // POST /Admin/SosyalMedya/Sil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var link = await _context.SosyalMedyaLinkleri.FindAsync(id);
            if (link != null)
            {
                _context.SosyalMedyaLinkleri.Remove(link);
                await _context.SaveChangesAsync();
                TempData["Basari"] = _localizer["Admin_SosyalMedya_Silindi"].Value;
                TempData["Durum"] = "success";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST /Admin/SosyalMedya/SiraGuncelle  (JSON: [{id,sira},...])
        [HttpPost]
        public async Task<IActionResult> SiraGuncelle([FromBody] List<SiraGuncelleDto> satirlar)
        {
            if (satirlar == null || satirlar.Count == 0)
                return BadRequest();

            var ids = satirlar.Select(x => x.Id).ToList();
            var linkler = await _context.SosyalMedyaLinkleri
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            foreach (var link in linkler)
            {
                var dto = satirlar.FirstOrDefault(x => x.Id == link.Id);
                if (dto != null) link.Sira = dto.Sira;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // Sadece http:// ve https:// kabul et; javascript:, data:, vb. reddet
        private static bool IsGecerliUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            var trimmed = url.Trim();
            return Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
        }

        public record SiraGuncelleDto(int Id, int Sira);
    }
}
