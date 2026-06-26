using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    public class BankalarController : AdminBaseController
    {
        private readonly KanvasDbContext _context;

        public BankalarController(KanvasDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hesaplar = await _context.BankaHesaplari
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.BankaAdi)
                .ToListAsync();

            return View(hesaplar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kaydet(BankaHesap model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.BankaAdi) ||
                    string.IsNullOrWhiteSpace(model.HesapSahibi) ||
                    string.IsNullOrWhiteSpace(model.IBAN))
                {
                    TempData["Mesaj"] = "Banka adı, hesap sahibi ve IBAN zorunludur.";
                    TempData["Durum"] = "danger";
                    return RedirectToAction(nameof(Index));
                }

                model.BankaAdi = model.BankaAdi.Trim();
                model.HesapSahibi = model.HesapSahibi.Trim();
                model.IBAN = model.IBAN.Trim().Replace(" ", "").ToUpperInvariant();

                if (model.Id == 0)
                {
                    model.OlusturulmaTarihi = DateTime.UtcNow;
                    _context.BankaHesaplari.Add(model);
                    TempData["Mesaj"] = $"{model.BankaAdi} banka hesabı eklendi.";
                }
                else
                {
                    var hesap = await _context.BankaHesaplari
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.Id == model.Id && !x.SilindiMi);

                    if (hesap == null)
                    {
                        TempData["Mesaj"] = "Banka hesabı bulunamadı.";
                        TempData["Durum"] = "danger";
                        return RedirectToAction(nameof(Index));
                    }

                    hesap.BankaAdi = model.BankaAdi;
                    hesap.HesapSahibi = model.HesapSahibi;
                    hesap.IBAN = model.IBAN;
                    hesap.SubeKodu = model.SubeKodu;
                    hesap.HesapNo = model.HesapNo;
                    hesap.AktifMi = model.AktifMi;
                    hesap.Sira = model.Sira;
                    TempData["Mesaj"] = $"{model.BankaAdi} banka hesabı güncellendi.";
                }

                await _context.SaveChangesAsync();
                TempData["Durum"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mesaj"] = "Hata: " + ex.Message;
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var hesap = await _context.BankaHesaplari
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);

            if (hesap == null)
            {
                TempData["Mesaj"] = "Banka hesabı bulunamadı.";
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            hesap.SilindiMi = true;
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{hesap.BankaAdi} banka hesabı silindi.";
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
