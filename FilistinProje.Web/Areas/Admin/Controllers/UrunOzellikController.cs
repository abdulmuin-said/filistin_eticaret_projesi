using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Helpers;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UrunOzellikController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UrunOzellikController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? urunTipi)
        {
            var selectedType = UrunOzellikCatalog.NormalizeProductType(urunTipi);
            var query = _context.UrunOzellikTanimlari.Where(x => !x.SilindiMi);

            if (!string.IsNullOrWhiteSpace(urunTipi))
            {
                query = query.Where(x => x.UrunTipi == selectedType);
            }

            ViewBag.UrunTipleri = await BuildProductTypeSelectListAsync(urunTipi);
            ViewBag.CurrentUrunTipi = string.IsNullOrWhiteSpace(urunTipi) ? string.Empty : selectedType;

            var items = await query
                .OrderBy(x => x.UrunTipi)
                .ThenBy(x => x.Sira)
                .ThenBy(x => x.Ad)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            await PopulateProductTypesAsync(UrunOzellikCatalog.Genel);
            return View(new UrunOzellikTanimi
            {
                UrunTipi = UrunOzellikCatalog.Genel,
                AlanTipi = "text",
                AktifMi = true,
                DetaySayfasindaGoster = true,
                TeknikTablodaGoster = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(UrunOzellikTanimi model)
        {
            NormalizeDefinition(model);
            await ValidateDefinitionAsync(model, null);

            if (!ModelState.IsValid)
            {
                await PopulateProductTypesAsync(model.UrunTipi);
                return View(model);
            }

            model.OlusturulmaTarihi = DateTime.UtcNow;
            model.SilindiMi = false;

            _context.UrunOzellikTanimlari.Add(model);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = _localizer["Admin_FeatureAdded"].Value;
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var model = await _context.UrunOzellikTanimlari.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (model == null)
            {
                return NotFound();
            }

            await PopulateProductTypesAsync(model.UrunTipi);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, UrunOzellikTanimi model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var existing = await _context.UrunOzellikTanimlari.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (existing == null)
            {
                return NotFound();
            }

            NormalizeDefinition(model);
            await ValidateDefinitionAsync(model, id);

            if (!ModelState.IsValid)
            {
                await PopulateProductTypesAsync(model.UrunTipi);
                return View(model);
            }

            existing.Ad = model.Ad;
            existing.Kod = model.Kod;
            existing.UrunTipi = model.UrunTipi;
            existing.AlanTipi = model.AlanTipi;
            existing.YardimMetni = model.YardimMetni;
            existing.Secenekler = model.Secenekler;
            existing.FiltredeGoster = model.FiltredeGoster;
            existing.DetaySayfasindaGoster = model.DetaySayfasindaGoster;
            existing.TeknikTablodaGoster = model.TeknikTablodaGoster;
            existing.AktifMi = model.AktifMi;
            existing.Sira = model.Sira;

            await _context.SaveChangesAsync();
            TempData["Mesaj"] = _localizer["Admin_FeatureUpdated"].Value;
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var model = await _context.UrunOzellikTanimlari.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (model != null)
            {
                model.SilindiMi = true;
                model.AktifMi = false;
                await _context.SaveChangesAsync();
            }

            TempData["Mesaj"] = _localizer["Admin_FeatureArchived"].Value;
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateProductTypesAsync(string? selectedProductType)
        {
            ViewBag.UrunTipleri = await BuildProductTypeSelectListAsync(selectedProductType);
        }

        private async Task<IReadOnlyList<SelectListItem>> BuildProductTypeSelectListAsync(string? selectedProductType)
        {
            var productTypes = await _context.Urunler
                .AsNoTracking()
                .Where(x => !x.SilindiMi && x.UrunTipi != null && x.UrunTipi != string.Empty)
                .Select(x => x.UrunTipi)
                .Concat(_context.UrunOzellikTanimlari
                    .AsNoTracking()
                    .Where(x => !x.SilindiMi && x.UrunTipi != string.Empty)
                    .Select(x => x.UrunTipi))
                .Distinct()
                .ToListAsync();

            var normalizedSelected = UrunOzellikCatalog.NormalizeProductType(selectedProductType);
            return productTypes
                .Append(UrunOzellikCatalog.Genel)
                .Append(normalizedSelected)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => string.Equals(x, UrunOzellikCatalog.Genel, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(x => x)
                .Select(x => new SelectListItem(x, x, string.Equals(x, normalizedSelected, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private static void NormalizeDefinition(UrunOzellikTanimi model)
        {
            model.Ad = model.Ad?.Trim() ?? string.Empty;
            model.Kod = SlugHelper.GenerateSlug(string.IsNullOrWhiteSpace(model.Kod) ? model.Ad : model.Kod).Replace("-", "_");
            model.UrunTipi = UrunOzellikCatalog.NormalizeProductType(model.UrunTipi);
            model.AlanTipi = string.IsNullOrWhiteSpace(model.AlanTipi) ? "text" : model.AlanTipi.Trim().ToLowerInvariant();
            model.YardimMetni = model.YardimMetni?.Trim() ?? string.Empty;
            model.Secenekler = model.Secenekler?.Trim() ?? string.Empty;
            model.Sira = model.Sira < 0 ? 0 : model.Sira;
        }

        private async Task ValidateDefinitionAsync(UrunOzellikTanimi model, int? currentId)
        {
            if (string.IsNullOrWhiteSpace(model.Ad))
            {
                ModelState.AddModelError(nameof(UrunOzellikTanimi.Ad), _localizer["Admin_FeatureNameRequired"]);
            }

            if (string.IsNullOrWhiteSpace(model.Kod))
            {
                ModelState.AddModelError(nameof(UrunOzellikTanimi.Kod), _localizer["Admin_FeatureCodeRequired"]);
            }

            var validFieldTypes = new[] { "text", "textarea", "number", "select" };
            if (!validFieldTypes.Contains(model.AlanTipi))
            {
                ModelState.AddModelError(nameof(UrunOzellikTanimi.AlanTipi), _localizer["Admin_ValidFieldTypeRequired"]);
            }

            if (model.AlanTipi == "select" && string.IsNullOrWhiteSpace(model.Secenekler))
            {
                ModelState.AddModelError(nameof(UrunOzellikTanimi.Secenekler), _localizer["Admin_FeatureOptionsRequired"]);
            }

            var duplicateExists = await _context.UrunOzellikTanimlari
                .IgnoreQueryFilters()
                .AnyAsync(x =>
                    x.Id != currentId &&
                    x.Kod == model.Kod &&
                    x.UrunTipi == model.UrunTipi);

            if (duplicateExists)
            {
                ModelState.AddModelError(nameof(UrunOzellikTanimi.Kod), _localizer["Admin_FeatureCodeDuplicate"]);
            }
        }
    }
}
