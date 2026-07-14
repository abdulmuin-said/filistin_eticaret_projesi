using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeSectionsController : AdminBaseController
    {
        private readonly IHomePageSectionService _sectionService;
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public HomeSectionsController(IHomePageSectionService sectionService, KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _sectionService = sectionService;
            _context = context;
            _localizer = localizer;
        }

        // GET: /admin/homesections
        public async Task<IActionResult> Index()
        {
            var sections = await _sectionService.GetActiveSectionsAsync();
            // Aktif+pasif hepsini göster (GetActiveSectionsAsync sadece Enabled olanları döner)
            // Tüm bölümleri sıralayarak göster
            var allSections = await _context.HomePageSections
                .AsNoTracking()
                .OrderBy(s => s.SortOrder)
                .ToListAsync();
            return View(allSections);
        }

        // GET: /admin/homesections/create
        public IActionResult Create()
        {
            var model = new HomePageSection
            {
                Enabled = true,
                SortOrder = 0
            };
            return View(model);
        }

        // POST: /admin/homesections/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HomePageSection model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError(nameof(model.Title), _localizer["FillRequiredFields"]);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _sectionService.CreateSectionAsync(model);
            TempData["Basari"] = "Yeni bölüm başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /admin/homesections/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var section = await _sectionService.GetSectionAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            // Ürün listesini view'a gönder (ürün seçimi için)
            ViewBag.TumUrunler = await _context.Urunler
                .AsNoTracking()
                .Where(u => u.AktifMi && !u.SilindiMi)
                .OrderBy(u => u.Baslik)
                .Select(u => new { u.Id, u.Baslik })
                .ToListAsync();

            return View(section);
        }

        // POST: /admin/homesections/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HomePageSection model, int[]? selectedProducts)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TumUrunler = await _context.Urunler
                    .AsNoTracking()
                    .Where(u => u.AktifMi && !u.SilindiMi)
                    .OrderBy(u => u.Baslik)
                    .Select(u => new { u.Id, u.Baslik })
                    .ToListAsync();
                return View(model);
            }

            await _sectionService.UpdateSectionAsync(model);

            // Ürün listesini güncelle (ProductBlock tipi için)
            if (model.SectionType == HomePageSectionType.ProductBlock && selectedProducts != null)
            {
                await _sectionService.SetSectionProductsAsync(id, selectedProducts.ToList());
            }

            TempData["Basari"] = "Bölüm başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // Destructive operations are POST-only so links and crawlers cannot delete content.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var section = await _context.HomePageSections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            await _sectionService.DeleteSectionAsync(id);
            TempData["Basari"] = "Bölüm silindi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /admin/homesections/toggleenabled
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEnabled(int id)
        {
            var section = await _context.HomePageSections.FindAsync(id);
            if (section == null) return NotFound();

            section.Enabled = !section.Enabled;
            await _sectionService.UpdateSectionAsync(section);

            return Json(new { success = true, enabled = section.Enabled });
        }
    }
}
