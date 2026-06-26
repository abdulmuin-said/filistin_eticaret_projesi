using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    public class CarkOdulController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CarkOdulController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var oduller = await _context.CarkOdulleri
                .Where(x => !x.SilindiMi)
                .OrderBy(x => x.Sira)
                .ToListAsync();
            return View(oduller);
        }

        public IActionResult Ekle()
        {
            return View(new CarkOdul());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(CarkOdul model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.OlusturulmaTarihi = DateTime.UtcNow;
            _context.CarkOdulleri.Add(model);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = _localizer["Admin_CarkOdul_Added"];
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Duzenle(int id)
        {
            var odul = await _context.CarkOdulleri.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (odul == null) return NotFound();
            return View(odul);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, CarkOdul model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var odul = await _context.CarkOdulleri.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (odul == null) return NotFound();

            odul.LabelTr = model.LabelTr;
            odul.LabelEn = model.LabelEn;
            odul.LabelAr = model.LabelAr;
            odul.Tip = model.Tip;
            odul.Deger = model.Deger;
            odul.Renk = model.Renk;
            odul.MesajTr = model.MesajTr;
            odul.MesajEn = model.MesajEn;
            odul.MesajAr = model.MesajAr;
            odul.Sira = model.Sira;
            odul.AktifMi = model.AktifMi;

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = _localizer["Admin_CarkOdul_Updated"];
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var odul = await _context.CarkOdulleri.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (odul == null) return NotFound();

            odul.SilindiMi = true;
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = _localizer["Admin_CarkOdul_Deleted"];
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
