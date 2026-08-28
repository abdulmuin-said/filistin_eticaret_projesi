using System.IO;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlaytController : AdminBaseController
    {
        private readonly KanvasDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SlaytController(KanvasDbContext db, IWebHostEnvironment env, IStringLocalizer<SharedResource> localizer)
        {
            _db = db;
            _env = env;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var slaytlar = await _db.Slaytlar
                .OrderBy(s => s.Sira)
                .ThenBy(s => s.OlusturmaTarihi)
                .ToListAsync();

            return View(slaytlar);
        }

        public IActionResult Ekle()
        {
            return View(new Slayt { Sira = 1, AktifMi = true, Tur = "Resim" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(52_428_800)]
        [RequestFormLimits(MultipartBodyLengthLimit = 52_428_800)]
        public async Task<IActionResult> Ekle(Slayt model, IFormFile? Resim, IFormFile? Video)
        {
            model.Baslik = "Slider";
            model.BaslikEn ??= string.Empty;
            model.BaslikAr ??= string.Empty;
            model.AltBaslikEn ??= string.Empty;
            model.AltBaslikAr ??= string.Empty;
            model.AciklamaEn ??= string.Empty;
            model.AciklamaAr ??= string.Empty;
            model.ButonMetni ??= string.Empty;
            model.ButonMetniEn ??= string.Empty;
            model.ButonMetniAr ??= string.Empty;

            ModelState.Remove(nameof(model.Baslik));
            ModelState.Remove(nameof(model.AltBaslik));
            ModelState.Remove(nameof(model.Aciklama));
            ModelState.Remove(nameof(model.BaglantiUrl));
            ModelState.Remove(nameof(model.BaslikEn));
            ModelState.Remove(nameof(model.BaslikAr));
            ModelState.Remove(nameof(model.AltBaslikEn));
            ModelState.Remove(nameof(model.AltBaslikAr));
            ModelState.Remove(nameof(model.AciklamaEn));
            ModelState.Remove(nameof(model.AciklamaAr));
            ModelState.Remove(nameof(model.ButonMetni));
            ModelState.Remove(nameof(model.ButonMetniEn));
            ModelState.Remove(nameof(model.ButonMetniAr));
            ModelState.Remove(nameof(model.LocalizedBaslik));
            ModelState.Remove(nameof(model.LocalizedAltBaslik));
            ModelState.Remove(nameof(model.LocalizedAciklama));
            ModelState.Remove(nameof(model.LocalizedButonMetni));

            if (!ModelState.IsValid)
                return View(model);

            if (model.Tur == "Video" && Video == null && string.IsNullOrWhiteSpace(model.VideoUrl))
            {
                ModelState.AddModelError("Video", _localizer["Admin_Slide_VideoRequired"].Value);
                return View(model);
            }

            if (model.Tur == "Resim" && Resim == null && string.IsNullOrWhiteSpace(model.ResimUrl))
            {
                ModelState.AddModelError("Resim", _localizer["Admin_Slide_ImageRequired"].Value);
                return View(model);
            }

            if (Resim != null)
            {
                var resimPath = await SaveFileAsync(Resim, "uploads/slider");
                model.ResimUrl = resimPath;
            }

            if (Video != null)
            {
                var videoPath = await SaveFileAsync(Video, "uploads/slider");
                model.VideoUrl = videoPath;
            }

            var maxSira = await _db.Slaytlar.MaxAsync(s => (int?)s.Sira) ?? 0;
            model.Sira = maxSira + 1;
            model.Baslik = $"Slider {model.Sira}";
            model.OlusturmaTarihi = DateTime.UtcNow;

            _db.Slaytlar.Add(model);
            await _db.SaveChangesAsync();

            TempData["Basari"] = _localizer["Admin_IslemBasarili"].Value;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Duzenle(int id)
        {
            var slayt = await _db.Slaytlar.FindAsync(id);
            if (slayt == null)
            {
                TempData["Hata"] = _localizer["Admin_Sonuc_bulunamadi"].Value;
                return RedirectToAction(nameof(Index));
            }
            return View(slayt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(52_428_800)]
        [RequestFormLimits(MultipartBodyLengthLimit = 52_428_800)]
        public async Task<IActionResult> Duzenle(int id, Slayt model, IFormFile? Resim, IFormFile? Video, bool? ResimSil, bool? VideoSil)
        {
            var slayt = await _db.Slaytlar.FindAsync(id);
            if (slayt == null)
            {
                TempData["Hata"] = _localizer["Admin_Sonuc_bulunamadi"].Value;
                return RedirectToAction(nameof(Index));
            }

            model.BaslikEn ??= string.Empty;
            model.BaslikAr ??= string.Empty;
            model.AltBaslikEn ??= string.Empty;
            model.AltBaslikAr ??= string.Empty;
            model.AciklamaEn ??= string.Empty;
            model.AciklamaAr ??= string.Empty;
            model.ButonMetni ??= string.Empty;
            model.ButonMetniEn ??= string.Empty;
            model.ButonMetniAr ??= string.Empty;

            ModelState.Remove(nameof(model.Baslik));
            ModelState.Remove(nameof(model.AltBaslik));
            ModelState.Remove(nameof(model.Aciklama));
            ModelState.Remove(nameof(model.BaglantiUrl));
            ModelState.Remove(nameof(model.BaslikEn));
            ModelState.Remove(nameof(model.BaslikAr));
            ModelState.Remove(nameof(model.AltBaslikEn));
            ModelState.Remove(nameof(model.AltBaslikAr));
            ModelState.Remove(nameof(model.AciklamaEn));
            ModelState.Remove(nameof(model.AciklamaAr));
            ModelState.Remove(nameof(model.ButonMetni));
            ModelState.Remove(nameof(model.ButonMetniEn));
            ModelState.Remove(nameof(model.ButonMetniAr));
            ModelState.Remove(nameof(model.LocalizedBaslik));
            ModelState.Remove(nameof(model.LocalizedAltBaslik));
            ModelState.Remove(nameof(model.LocalizedAciklama));
            ModelState.Remove(nameof(model.LocalizedButonMetni));

            if (!ModelState.IsValid)
                return View(slayt);

            slayt.Tur = model.Tur;
            slayt.Sira = model.Sira;
            slayt.AktifMi = model.AktifMi;

            if (Resim != null)
            {
                if (!string.IsNullOrEmpty(slayt.ResimUrl))
                    DeleteFile(slayt.ResimUrl);
                slayt.ResimUrl = await SaveFileAsync(Resim, "uploads/slider");
            }

            if (Video != null)
            {
                if (!string.IsNullOrEmpty(slayt.VideoUrl))
                    DeleteFile(slayt.VideoUrl);
                slayt.VideoUrl = await SaveFileAsync(Video, "uploads/slider");
            }

            if (ResimSil == true && !string.IsNullOrEmpty(slayt.ResimUrl))
            {
                DeleteFile(slayt.ResimUrl);
                slayt.ResimUrl = null;
            }

            if (VideoSil == true && !string.IsNullOrEmpty(slayt.VideoUrl))
            {
                DeleteFile(slayt.VideoUrl);
                slayt.VideoUrl = null;
            }

            if (!string.IsNullOrWhiteSpace(model.ResimUrl) && model.ResimUrl != slayt.ResimUrl)
            {
                slayt.ResimUrl = model.ResimUrl;
            }

            if (!string.IsNullOrWhiteSpace(model.VideoUrl) && model.VideoUrl != slayt.VideoUrl)
            {
                slayt.VideoUrl = model.VideoUrl;
            }

            await _db.SaveChangesAsync();

            TempData["Basari"] = _localizer["Admin_IslemBasarili"].Value;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AktiflikDegistir(int id)
        {
            var slayt = await _db.Slaytlar.FindAsync(id);
            if (slayt != null)
            {
                slayt.AktifMi = !slayt.AktifMi;
                await _db.SaveChangesAsync();
                TempData["Basari"] = slayt.AktifMi ? "Slide activated." : "Slide deactivated.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JSONdanAl()
        {
            var jsonPath = System.IO.Path.Combine(_env.ContentRootPath, "App_Data", "home-page-settings.json");
            if (!System.IO.File.Exists(jsonPath))
            {
                TempData["Hata"] = _localizer["Admin_Sonuc_bulunamadi"].Value;
                return RedirectToAction(nameof(Index));
            }

            var json = await System.IO.File.ReadAllTextAsync(jsonPath);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var hero = doc.RootElement.GetProperty("Hero");
            var slides = hero.GetProperty("DesktopSlides");

            int sira = 1;
            foreach (var slide in slides.EnumerateArray())
            {
                var imageUrl = slide.GetProperty("ImageUrl").GetString() ?? "";
                var videoUrl = slide.GetProperty("VideoUrl").GetString() ?? "";
                var title = slide.GetProperty("Title").GetString() ?? "";
                var subtitle = slide.GetProperty("Subtitle").GetString() ?? "";
                var description = slide.GetProperty("Description").GetString() ?? "";

                var tur = !string.IsNullOrEmpty(videoUrl) ? "Video" : "Resim";

                var mevcut = await _db.Slaytlar
                    .FirstOrDefaultAsync(s => s.ResimUrl == imageUrl || s.VideoUrl == videoUrl);

                if (mevcut == null)
                {
                    var cleanTitle = title.Replace("\\n", " ").Replace("\n", "");
                    _db.Slaytlar.Add(new Slayt
                    {
                        Baslik = cleanTitle,
                        AltBaslik = subtitle ?? "",
                        Aciklama = description ?? "",
                        ResimUrl = imageUrl,
                        VideoUrl = videoUrl,
                        Tur = tur,
                        Sira = sira,
                        AktifMi = true,
                        OlusturmaTarihi = DateTime.UtcNow
                    });
                    sira++;
                }
            }

            await _db.SaveChangesAsync();
            TempData["Basari"] = _localizer["Admin_IslemBasarili"] + (sira - 1) + " سلايد مستورد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var slayt = await _db.Slaytlar.FindAsync(id);
            if (slayt == null)
            {
                return Json(new { success = false, message = "Slayt bulunamadÄ±." });
            }

            var totalCount = await _db.Slaytlar.CountAsync();
            if (totalCount <= 1)
            {
                return Json(new { success = false, message = "En az 1 slayt kalmalÄ±dÄ±r. TÃ¼m slaytlarÄ± silemezsiniz." });
            }

            if (!string.IsNullOrEmpty(slayt.ResimUrl))
                DeleteFile(slayt.ResimUrl);

            if (!string.IsNullOrEmpty(slayt.VideoUrl))
                DeleteFile(slayt.VideoUrl);

            _db.Slaytlar.Remove(slayt);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Slayt silindi." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Siralama(List<int> ids)
        {
            var slaytlar = await _db.Slaytlar.ToListAsync();
            for (int i = 0; i < ids.Count; i++)
            {
                var slayt = slaytlar.FirstOrDefault(s => s.Id == ids[i]);
                if (slayt != null)
                    slayt.Sira = i + 1;
            }
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            var isVideo = file.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase);
            var allowedExtensions = isVideo
                ? new[] { ".mp4", ".webm" }
                : new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (file.Length <= 0 || file.Length > (isVideo ? 50L * 1024 * 1024 : 10L * 1024 * 1024) ||
                !allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException(_localizer["Admin_InvalidSliderUpload"].Value);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, subFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString("N") + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/" + subFolder.Replace("\\", "/") + "/" + uniqueFileName;
        }

        private void DeleteFile(string? url)
        {
            if (string.IsNullOrEmpty(url) || !url.StartsWith("/uploads/slider/", StringComparison.Ordinal)) return;
            var relativePath = url.TrimStart('/').Replace("/", "\\");
            var fullPath = System.IO.Path.Combine(_env.WebRootPath, relativePath);
            var uploadRoot = Path.GetFullPath(Path.Combine(_env.WebRootPath, "uploads", "slider"));
            if (!Path.GetFullPath(fullPath).StartsWith(uploadRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) return;
            if (System.IO.File.Exists(fullPath))
            {
                try { System.IO.File.Delete(fullPath); } catch { }
            }
        }
    }
}


