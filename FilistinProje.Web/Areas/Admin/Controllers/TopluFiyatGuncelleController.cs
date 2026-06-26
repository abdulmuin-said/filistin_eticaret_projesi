using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopluFiyatGuncelleController : AdminBaseController
    {
        private readonly KanvasDbContext _context;

        public TopluFiyatGuncelleController(KanvasDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var olcuListesi = await _context.UrunSecenekleri
                .AsNoTracking()
                .Where(x => !x.SilindiMi && !string.IsNullOrWhiteSpace(x.Olcu))
                .GroupBy(x => x.Olcu)
                .Select(g => new OlcuFiyatModel
                {
                    Olcu = g.Key,
                    UrunSayisi = g.Select(x => x.UrunId).Distinct().Count(),
                    VaryasyonSayisi = g.Count(),
                    MevcutFiyat = g.Min(x => x.SatisFiyati)
                })
                .OrderBy(x => x.Olcu)
                .ToListAsync();

            return View(olcuListesi);
        }

        [HttpPost]
        public async Task<IActionResult> FiyatGuncelle(string olcu, decimal yeniFiyat)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(olcu) || yeniFiyat <= 0)
                {
                    return Json(new { success = false, message = "Geçersiz parametre" });
                }

                var varyasyonlar = await _context.UrunSecenekleri
                    .Where(x => x.Olcu == olcu && !x.SilindiMi)
                    .ToListAsync();

                if (!varyasyonlar.Any())
                {
                    return Json(new { success = false, message = "Ölçü bulunamadı" });
                }

                var urunIdler = varyasyonlar.Select(x => x.UrunId).Distinct().ToList();

                foreach (var varyasyon in varyasyonlar)
                {
                    varyasyon.SatisFiyati = yeniFiyat;
                }

                var urunler = await _context.Urunler
                    .Where(x => urunIdler.Contains(x.Id))
                    .ToListAsync();

                foreach (var urun in urunler)
                {
                    urun.IndirimliFiyat = null;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = $"{varyasyonlar.Count} varyasyon güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guncelle(List<OlcuFiyatGuncelleModel> fiyatlar)
        {
            if (fiyatlar == null || !fiyatlar.Any())
            {
                TempData["Hata"] = "Güncellenecek fiyat bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            int guncellenenSayisi = 0;

            foreach (var item in fiyatlar.Where(x => x.YeniFiyat.HasValue && x.YeniFiyat > 0 && !string.IsNullOrWhiteSpace(x.Olcu)))
            {
                var varyasyonlar = await _context.UrunSecenekleri
                    .Where(x => x.Olcu == item.Olcu && !x.SilindiMi)
                    .ToListAsync();

                foreach (var varyasyon in varyasyonlar)
                {
                    varyasyon.SatisFiyati = item.YeniFiyat!.Value;
                }

                guncellenenSayisi += varyasyonlar.Count;
            }

            await _context.SaveChangesAsync();

            TempData["Basari"] = $"{guncellenenSayisi} varyasyon güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> NormalizeOlcu()
        {
            try
            {
                var varyasyonlar = await _context.UrunSecenekleri
                    .Where(x => !x.SilindiMi && !string.IsNullOrWhiteSpace(x.Olcu))
                    .ToListAsync();

                int sayac = 0;
                foreach (var v in varyasyonlar)
                {
                    var duzeltilmis = NormalizeOlcuString(v.Olcu);
                    if (duzeltilmis != v.Olcu)
                    {
                        v.Olcu = duzeltilmis;
                        sayac++;
                    }
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = $"{sayac} ölçü normalize edildi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        private string NormalizeOlcuString(string olcu)
        {
            if (string.IsNullOrWhiteSpace(olcu)) return olcu;

            // Parantez içeriğini kaldır: "100 cm x 60 cm (5 Parça...)" -> "100 cm x 60 cm"
            var idx = olcu.IndexOf('(');
            if (idx > 0)
                olcu = olcu.Substring(0, idx).Trim();

            // Tüm fazla boşlukları temizle
            olcu = System.Text.RegularExpressions.Regex.Replace(olcu, @"\s+", " ");

            // "x" veya "X" yerine " x " koy
            olcu = System.Text.RegularExpressions.Regex.Replace(olcu, @"[xX]", " x ");

            // "cm" sonrası ve öncesi boşluk düzelt
            olcu = System.Text.RegularExpressions.Regex.Replace(olcu, @"\s*cm\s*", "cm ");
            olcu = olcu.Trim();

            // Sondaki "cm" sonra boşluk varsa kaldır
            if (olcu.EndsWith("cm "))
                olcu = olcu.Substring(0, olcu.Length - 1);

            // Başına ve sonuna cm ekle (örn: "150x100" -> "150cm x 100cm")
            olcu = System.Text.RegularExpressions.Regex.Replace(olcu, @"(\d+)cm\s*x\s*(\d+)cm?", "$1cm x $2cm");

            return olcu;
        }
    }

    public class OlcuFiyatModel
    {
        public string Olcu { get; set; } = string.Empty;
        public int UrunSayisi { get; set; }
        public int VaryasyonSayisi { get; set; }
        public decimal MevcutFiyat { get; set; }
    }

    public class OlcuFiyatGuncelleModel
    {
        public string Olcu { get; set; } = string.Empty;
        public decimal? YeniFiyat { get; set; }
    }
}