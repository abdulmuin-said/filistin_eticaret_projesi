using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KargoController : AdminBaseController
    {
        private readonly KanvasDbContext _context;

        public KargoController(KanvasDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var firmalar = await _context.KargoFirmalari
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Ad)
                .ToListAsync();

            return View(firmalar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kaydet(KargoFirmasi model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Ad))
                {
                    TempData["Mesaj"] = "Kargo firması adı zorunludur.";
                    TempData["Durum"] = "danger";
                    return RedirectToAction(nameof(Index));
                }

                model.Ad = model.Ad.Trim();
                model.Kod = string.IsNullOrWhiteSpace(model.Kod)
                    ? model.Ad.ToLowerInvariant().Replace(" ", "-")
                    : model.Kod.Trim().ToLowerInvariant();

                if (model.VarsayilanMi)
                {
                    await _context.KargoFirmalari
                        .Where(x => x.Id != model.Id)
                        .ExecuteUpdateAsync(x => x.SetProperty(v => v.VarsayilanMi, false));
                }

                if (model.Id == 0)
                {
                    model.OlusturulmaTarihi = DateTime.UtcNow;
                    _context.KargoFirmalari.Add(model);
                    TempData["Mesaj"] = $"{model.Ad} kargo firması eklendi.";
                }
                else
                {
                    var firma = await _context.KargoFirmalari.FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (firma == null)
                    {
                        TempData["Mesaj"] = "Kargo firması bulunamadı.";
                        TempData["Durum"] = "danger";
                        return RedirectToAction(nameof(Index));
                    }

                    firma.Ad = model.Ad;
                    firma.Kod = model.Kod;
                    firma.LogoUrl = model.LogoUrl;
                    firma.Telefon = model.Telefon;
                    firma.TakipUrl = model.TakipUrl;
                    firma.GondericiUnvan = model.GondericiUnvan;
                    firma.GondericiAdres = model.GondericiAdres;
                    firma.GondericiTelefon = model.GondericiTelefon;
                    firma.AktifMi = model.AktifMi;
                    firma.VarsayilanMi = model.VarsayilanMi;
                    firma.Fiyat = model.Fiyat;
                    TempData["Mesaj"] = $"{model.Ad} kargo firması güncellendi.";
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
        public async Task<IActionResult> VarsayilanYap(int id)
        {
            var firma = await _context.KargoFirmalari.FirstOrDefaultAsync(x => x.Id == id && !x.SilindiMi);
            if (firma == null)
            {
                TempData["Mesaj"] = "Kargo firması bulunamadı.";
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            await _context.KargoFirmalari.ExecuteUpdateAsync(x => x.SetProperty(v => v.VarsayilanMi, false));
            firma.VarsayilanMi = true;
            firma.AktifMi = true;
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{firma.Ad} varsayılan kargo firması yapıldı.";
            TempData["Durum"] = "success";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BolgeListesi()
        {
            var bolgeler = await _context.KargoBolgeler
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .OrderBy(x => x.Sira)
                .Select(b => new
                {
                    b.Id,
                    b.Ad,
                    b.Ulke,
                    b.Aciklama,
                    b.Sira,
                    b.Fiyat,
                    SehirSayisi = b.Sehirler.Count,
                    Sehirler = b.Sehirler.Select(s => new { s.Id, s.SehirAdi }).OrderBy(s => s.SehirAdi).ToList()
                })
                .ToListAsync();

            return Json(bolgeler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BolgeKaydet([FromForm] string ad, [FromForm] string? ulke, [FromForm] string? aciklama, [FromForm] int sira, [FromForm] decimal fiyat, [FromForm] int id = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ad))
                    return Json(new { success = false, message = "Bölge adı zorunludur." });

                if (id == 0)
                {
                    var bolge = new KargoBolge
                    {
                        Ad = ad.Trim(),
                        Ulke = ulke?.Trim(),
                        Aciklama = aciklama?.Trim(),
                        Sira = sira,
                        Fiyat = fiyat,
                        OlusturulmaTarihi = DateTime.UtcNow
                    };
                    _context.KargoBolgeler.Add(bolge);
                }
                else
                {
                    var bolge = await _context.KargoBolgeler.FirstOrDefaultAsync(x => x.Id == id);
                    if (bolge == null)
                        return Json(new { success = false, message = "Bölge bulunamadı." });

                    bolge.Ad = ad.Trim();
                    bolge.Ulke = ulke?.Trim();
                    bolge.Aciklama = aciklama?.Trim();
                    bolge.Sira = sira;
                    bolge.Fiyat = fiyat;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = id == 0 ? "Bölge eklendi." : "Bölge güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BolgeSil(int id)
        {
            try
            {
                var bolge = await _context.KargoBolgeler.FirstOrDefaultAsync(x => x.Id == id);
                if (bolge == null)
                    return Json(new { success = false, message = "Bölge bulunamadı." });

                bolge.SilindiMi = true;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Bölge silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SehirKaydet([FromForm] int bolgeId, [FromForm] string sehirAdi, [FromForm] int id = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sehirAdi))
                    return Json(new { success = false, message = "Şehir adı zorunludur." });

                var bolge = await _context.KargoBolgeler.FirstOrDefaultAsync(x => x.Id == bolgeId && !x.SilindiMi);
                if (bolge == null)
                    return Json(new { success = false, message = "Bölge bulunamadı." });

                if (id == 0)
                {
                    var mevcut = await _context.KargoBolgeSehirler
                        .AnyAsync(x => x.BolgeId == bolgeId && x.SehirAdi == sehirAdi.Trim() && !x.SilindiMi);
                    if (mevcut)
                        return Json(new { success = false, message = "Bu şehir zaten bu bölgede mevcut." });

                    _context.KargoBolgeSehirler.Add(new KargoBolgeSehir
                    {
                        BolgeId = bolgeId,
                        SehirAdi = sehirAdi.Trim(),
                        OlusturulmaTarihi = DateTime.UtcNow
                    });
                }
                else
                {
                    var sehir = await _context.KargoBolgeSehirler.FirstOrDefaultAsync(x => x.Id == id);
                    if (sehir == null)
                        return Json(new { success = false, message = "Şehir bulunamadı." });

                    sehir.SehirAdi = sehirAdi.Trim();
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = id == 0 ? "Şehir eklendi." : "Şehir güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SehirSil(int id)
        {
            try
            {
                var sehir = await _context.KargoBolgeSehirler.FirstOrDefaultAsync(x => x.Id == id);
                if (sehir == null)
                    return Json(new { success = false, message = "Şehir bulunamadı." });

                _context.KargoBolgeSehirler.Remove(sehir);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Şehir silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        public async Task<IActionResult> BolgeFiyatListesi()
        {
            var firmalar = await _context.KargoFirmalari
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Ad)
                .Select(x => new { x.Id, x.Ad, x.Fiyat })
                .ToListAsync();

            var bolgeler = await _context.KargoBolgeler
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi && x.Ulke == "Filistin")
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Ad)
                .Select(x => new { x.Id, x.Ad, x.Ulke })
                .ToListAsync();

            var fiyatlar = await _context.KargoBolgeFiyatlari
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .Select(x => new { x.KargoFirmasiId, x.BolgeId, x.Fiyat })
                .ToListAsync();

            return Json(new { firmalar, bolgeler, fiyatlar });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BolgeFiyatKaydet([FromForm] int kargoFirmasiId, [FromForm] int bolgeId, [FromForm] decimal fiyat)
        {
            try
            {
                if (fiyat < 0)
                    return Json(new { success = false, message = "Kargo fiyatı 0 veya daha büyük olmalıdır." });

                var firmaVar = await _context.KargoFirmalari.IgnoreQueryFilters()
                    .AnyAsync(x => x.Id == kargoFirmasiId && !x.SilindiMi);
                if (!firmaVar)
                    return Json(new { success = false, message = "Kargo firması bulunamadı." });

                var bolgeVar = await _context.KargoBolgeler.IgnoreQueryFilters()
                    .AnyAsync(x => x.Id == bolgeId && !x.SilindiMi);
                if (!bolgeVar)
                    return Json(new { success = false, message = "Bölge bulunamadı." });

                var bolgeFiyat = await _context.KargoBolgeFiyatlari.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.KargoFirmasiId == kargoFirmasiId && x.BolgeId == bolgeId);

                if (bolgeFiyat == null)
                {
                    _context.KargoBolgeFiyatlari.Add(new KargoBolgeFiyat
                    {
                        KargoFirmasiId = kargoFirmasiId,
                        BolgeId = bolgeId,
                        Fiyat = fiyat,
                        OlusturulmaTarihi = DateTime.UtcNow
                    });
                }
                else
                {
                    bolgeFiyat.Fiyat = fiyat;
                    bolgeFiyat.SilindiMi = false;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Bölge kargo fiyatı güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}
