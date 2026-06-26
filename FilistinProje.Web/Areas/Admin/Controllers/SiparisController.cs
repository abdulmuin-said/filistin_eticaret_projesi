using System.Text;
using FilistinProje.Core.Helpers;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiparisController : AdminBaseController
    {
        private readonly IService<Siparis> _siparisService;
        private readonly IService<SiparisDetay> _detayService;
        private readonly IService<UrunSecenek> _secenekService;
        private readonly IService<Urun> _urunService;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IFaturaPdfService _faturaPdfService;
        private readonly KanvasDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<SiparisController> _logger;

        public SiparisController(
            IService<Siparis> siparisService,
            IService<SiparisDetay> detayService,
            IService<UrunSecenek> secenekService,
            IService<Urun> urunService,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            IFaturaPdfService faturaPdfService,
            KanvasDbContext context,
            IWebHostEnvironment env,
            ILogger<SiparisController> logger)
        {
            _siparisService = siparisService;
            _detayService = detayService;
            _secenekService = secenekService;
            _urunService = urunService;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
            _faturaPdfService = faturaPdfService;
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string search, int? durum, int? receteDurum, int page = 1, int pageSize = 20)
        {
            page = Math.Max(page, 1);
            var allowedPageSizes = new[] { 20, 50, 100 };
            if (!allowedPageSizes.Contains(pageSize))
            {
                pageSize = 20;
            }

            var siparisler = await _siparisService.GetAllAsync();
            var siparisDetaylari = await _detayService.GetAllAsync();
            var siparisDetayOzetleri = siparisDetaylari
                .GroupBy(x => x.SiparisId)
                .ToDictionary(
                    x => x.Key,
                    x => $"{x.Count()} Ã¼rÃ¼n / {x.Sum(v => v.Adet)} adet");
            var sorgu = siparisler.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLowerInvariant();
                sorgu = sorgu.Where(x =>
                    x.Id.ToString().Contains(search) ||
                    (!string.IsNullOrWhiteSpace(x.SiparisNo) && x.SiparisNo.ToLowerInvariant().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(x.MusteriAdSoyad) && x.MusteriAdSoyad.ToLowerInvariant().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(x.Telefon) && x.Telefon.Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(x.Eposta) && x.Eposta.ToLowerInvariant().Contains(search)));
            }

            if (durum.HasValue)
            {
                sorgu = sorgu.Where(x => x.Durum == durum.Value);
            }

            if (receteDurum.HasValue)
            {
                sorgu = sorgu.Where(x =>
                    !string.IsNullOrWhiteSpace(x.ReceteDosyaYolu) &&
                    x.ReceteOnayDurumu == receteDurum.Value);
            }

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentDurum = durum;
            ViewBag.CurrentReceteDurum = receteDurum;
            ViewBag.ReceteBekleyenCount = siparisler.Count(x => x.ReceteOnayDurumu == 0 && !string.IsNullOrWhiteSpace(x.ReceteDosyaYolu));
            ViewBag.ReceteOnayliCount = siparisler.Count(x => x.ReceteOnayDurumu == 1);
            ViewBag.ReceteRedliCount = siparisler.Count(x => x.ReceteOnayDurumu == 2);
            ViewBag.TumuCount = siparisler.Count();
            ViewBag.YeniCount = siparisler.Count(x => x.Durum == SiparisDurumHelper.SiparisAlindi);
            ViewBag.HazirlaniyorCount = siparisler.Count(x => x.Durum == SiparisDurumHelper.UretimHazirlaniyor);
            ViewBag.PaketleniyorCount = siparisler.Count(x => x.Durum == SiparisDurumHelper.Paketleniyor);
            ViewBag.KargodaCount = siparisler.Count(x => x.Durum == SiparisDurumHelper.KargoyaVerildi);
            ViewBag.TeslimCount = siparisler.Count(x => x.Durum == SiparisDurumHelper.TeslimEdildi);
            ViewBag.FaturaYuklenmemiÅŸCount = siparisler.Count(x => !x.FaturaYuklendiMi);
            ViewBag.SiparisDetayOzetleri = siparisDetayOzetleri;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.PageSizeOptions = allowedPageSizes;
            ViewBag.TotalCount = sorgu.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / pageSize);

            return View(sorgu
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList());
        }

        public async Task<IActionResult> Detay(int id)
        {
            var siparis = await _siparisService.GetByIdAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }

            var siparisUrunleri = await _context.SiparisDetaylari
                .AsNoTracking()
                .Where(x => x.SiparisId == id && !x.SilindiMi)
                .Include(x => x.Urun)
                .Include(x => x.UrunSecenek)
                .ToListAsync();

            var urunBilgileri = siparisUrunleri.Select(item =>
            {
                var secenek = item.UrunSecenek;
                var urun = item.Urun ?? (secenek?.Urun);
                if (urun == null) return (dynamic?)null;

                return new
                {
                    Baslik = urun.Baslik,
                    Resim = !string.IsNullOrWhiteSpace(secenek?.GorselUrl) ? secenek.GorselUrl : urun.AnaGorselUrl,
                    Olcu = secenek?.Olcu,
                    Cerceve = secenek?.CerceveTipi,
                    Secenek = string.IsNullOrWhiteSpace(secenek?.VaryantBasligi) ? "VarsayÄ±lan varyasyon" : secenek.VaryantBasligi,
                    SecenekDetay = secenek?.VaryantOzeti,
                    CerceveModeli = item.CerceveModeli,
                    HediyePaketi = item.HediyePaketi,
                    Adet = item.Adet,
                    Fiyat = item.BirimFiyat,
                    Toplam = (item.Adet * item.BirimFiyat) + (item.HediyePaketi ? item.HediyePaketFiyati * item.Adet : 0),
                    MusteriNotu = item.MusteriNotu
                };
            }).Where(x => x != null).ToList();

            ViewBag.UrunBilgileri = urunBilgileri;
            ViewBag.KargoFirmalari = await _context.KargoFirmalari
                .AsNoTracking()
                .Where(x => !x.SilindiMi && x.AktifMi)
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Ad)
                .ToListAsync();
            ViewBag.SeciliKargoFirmasi = await ResolveKargoFirmasiAsync(siparis);
            ViewBag.SiteSettings = _siteSettingsService.GetSettings();
            return View(siparis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DurumGuncelle(int id, int durum, string kargoNo, int? kargoFirmasiId)
        {
            var siparis = await _siparisService.GetByIdAsync(id);
            if (siparis == null)
            {
                TempData["Mesaj"] = "SipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "danger";
                return RedirectToAction("Detay", new { id });
            }

            var eskiDurum = siparis.Durum;
            var temizKargoNo = kargoNo?.Trim() ?? string.Empty;
            var firma = await ResolveKargoFirmasiAsync(siparis, kargoFirmasiId);

            if (durum != eskiDurum && !CanMoveOrderStatusForward(eskiDurum, durum))
            {
                TempData["Mesaj"] = $"SipariÅŸ durumu '{SiparisDurumHelper.GetLabel(eskiDurum)}' aÅŸamasÄ±ndan '{SiparisDurumHelper.GetLabel(durum)}' aÅŸamasÄ±na geri alÄ±namaz.";
                TempData["Durum"] = "warning";
                return RedirectToAction("Detay", new { id });
            }

            siparis.Durum = durum;
            siparis.KargoFirmasiId = firma?.Id;
            siparis.KargoFirmasi = firma?.Ad;
            if (!string.IsNullOrWhiteSpace(temizKargoNo))
            {
                siparis.KargoTakipNo = temizKargoNo;
            }

            await _siparisService.UpdateAsync(siparis);

            if (durum == eskiDurum)
            {
                TempData["Mesaj"] = "SipariÅŸ operasyon bilgileri gÃ¼ncellendi.";
                TempData["Durum"] = "success";
                return RedirectToAction("Detay", new { id });
            }

            var mailSonucu = await SendStatusNotificationAsync(siparis, eskiDurum, durum, firma?.Ad ?? "Aras Kargo", temizKargoNo);
            if (mailSonucu.Success)
            {
                TempData["Mesaj"] = "SipariÅŸ durumu gÃ¼ncellendi. MÃ¼ÅŸteriye bilgilendirme e-postasÄ± gÃ¶nderildi.";
                TempData["Durum"] = "success";
            }
            else
            {
                TempData["Mesaj"] = $"SipariÅŸ durumu gÃ¼ncellendi. E-posta gÃ¶nderimi atlandÄ±: {mailSonucu.Message}";
                TempData["Durum"] = "warning";
            }

            return RedirectToAction("Detay", new { id });
        }

        public async Task<IActionResult> ExcelExport()
        {
            ExcelPackage.License.SetNonCommercialOrganization("7ANRPS48");
            var siparisler = await _siparisService.GetAllAsync();
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("SipariÅŸler");

            var headers = new[]
            {
                "SipariÅŸ No",
                "MÃ¼ÅŸteri",
                "E-posta",
                "Telefon",
                "Åehir",
                "Ä°lÃ§e",
                "Tarih",
                "Tutar",
                "Durum",
                "Kargo FirmasÄ±",
                "Kargo Takip No"
            };

            for (var i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            var row = 2;
            foreach (var item in siparisler)
            {
                worksheet.Cells[row, 1].Value = string.IsNullOrWhiteSpace(item.SiparisNo) ? $"#{item.Id}" : item.SiparisNo;
                worksheet.Cells[row, 2].Value = item.MusteriAdSoyad;
                worksheet.Cells[row, 3].Value = item.Eposta;
                worksheet.Cells[row, 4].Value = item.Telefon;
                worksheet.Cells[row, 5].Value = item.Sehir;
                worksheet.Cells[row, 6].Value = item.Ilce;
                worksheet.Cells[row, 7].Value = item.OlusturulmaTarihi.ToLocalTime();
                worksheet.Cells[row, 8].Value = item.ToplamTutar;
                worksheet.Cells[row, 9].Value = SiparisDurumHelper.GetShortLabel(item.Durum);
                worksheet.Cells[row, 10].Value = item.KargoFirmasi;
                worksheet.Cells[row, 11].Value = item.KargoTakipNo;
                row++;
            }

            using (var range = worksheet.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(49, 53, 17));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            worksheet.Column(7).Style.Numberformat.Format = "dd.mm.yyyy hh:mm";
            worksheet.Column(8).Style.Numberformat.Format = "#,##0.00 TL";
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"siparisler-{DateTime.Now:yyyyMMdd-HHmm}.xlsx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluEtiketYazdir(List<int> siparisIds)
        {
            siparisIds = siparisIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (!siparisIds.Any())
            {
                TempData["Mesaj"] = "Etiket yazdÄ±rmak iÃ§in en az bir sipariÅŸ seÃ§melisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var validDurumlar = new[] { 
                SiparisDurumHelper.SiparisAlindi, 
                SiparisDurumHelper.UretimHazirlaniyor, 
                SiparisDurumHelper.Paketleniyor 
            };

            var siralama = siparisIds
                .Select((id, index) => new { id, index })
                .ToDictionary(x => x.id, x => x.index);

            var siparisler = await _context.Siparisler
                .AsNoTracking()
                .Include(x => x.SiparisDetaylari)
                    .ThenInclude(x => x.Urun)
                .Include(x => x.SiparisDetaylari)
                    .ThenInclude(x => x.UrunSecenek)
                .Where(x => siparisIds.Contains(x.Id) && validDurumlar.Contains(x.Durum))
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "SeÃ§ilen sipariÅŸler arasÄ±nda etiket yazdÄ±rÄ±labilecek (Yeni, HazÄ±rlanÄ±yor veya Paketleniyor durumunda) sipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            siparisler = siparisler
                .OrderBy(x => siralama.TryGetValue(x.Id, out var index) ? index : int.MaxValue)
                .ToList();

            var kargoFirmalari = await _context.KargoFirmalari
                .AsNoTracking()
                .Where(x => !x.SilindiMi && x.AktifMi)
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Ad)
                .ToListAsync();

            ViewBag.KargoFirmalari = kargoFirmalari;
            ViewBag.SiteSettings = _siteSettingsService.GetSettings();

            return View("TopluEtiket", siparisler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluOnayla(List<int> siparisIds)
        {
            siparisIds = siparisIds?.Where(x => x > 0).Distinct().ToList() ?? new List<int>();

            if (!siparisIds.Any())
            {
                TempData["Mesaj"] = "Onaylamak iÃ§in en az bir sipariÅŸ seÃ§melisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id) && x.Durum == SiparisDurumHelper.SiparisAlindi)
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "Onaylanacak 'Yeni' durumlu sipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            foreach (var siparis in siparisler)
            {
                siparis.Durum = SiparisDurumHelper.UretimHazirlaniyor;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{siparisler.Count} sipariÅŸ 'HazÄ±rlanÄ±yor' durumuna alÄ±ndÄ±.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Index), new { toast = Uri.EscapeDataString($"{siparisler.Count} sipariÅŸ onaylandÄ±"), toastType = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluKargoyaVer(List<int> siparisIds, int? kargoFirmasiId, string? kargoNo)
        {
            siparisIds = siparisIds?.Where(x => x > 0).Distinct().ToList() ?? new List<int>();

            if (!siparisIds.Any())
            {
                TempData["Mesaj"] = "Kargoya vermek iÃ§in en az bir sipariÅŸ seÃ§melisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var validDurumlar = new[] { SiparisDurumHelper.UretimHazirlaniyor, SiparisDurumHelper.Paketleniyor };
            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id) && validDurumlar.Contains(x.Durum))
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "Kargoya verilecek 'HazÄ±rlanÄ±yor' veya 'Paketleniyor' durumlu sipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            foreach (var siparis in siparisler)
            {
                siparis.Durum = SiparisDurumHelper.KargoyaVerildi;
                if (kargoFirmasiId.HasValue)
                    siparis.KargoFirmasiId = kargoFirmasiId.Value;
                if (!string.IsNullOrWhiteSpace(kargoNo))
                    siparis.KargoTakipNo = kargoNo;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{siparisler.Count} sipariÅŸ 'Kargoya Verildi' durumuna alÄ±ndÄ±.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Index), new { toast = Uri.EscapeDataString($"{siparisler.Count} sipariÅŸ kargoya verildi"), toastType = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluTeslimEt(List<int> siparisIds)
        {
            siparisIds = siparisIds?.Where(x => x > 0).Distinct().ToList() ?? new List<int>();

            if (!siparisIds.Any())
            {
                TempData["Mesaj"] = "Teslim etmek icin en az bir siparis secmelisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum = SiparisDurumHelper.KargoyaVerildi });
            }

            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id) && x.Durum == SiparisDurumHelper.KargoyaVerildi)
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "Teslim edilecek 'Kargoda' durumlu siparis bulunamadi.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum = SiparisDurumHelper.KargoyaVerildi });
            }

            foreach (var siparis in siparisler)
            {
                siparis.Durum = SiparisDurumHelper.TeslimEdildi;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{siparisler.Count} siparis 'Teslim Edildi' durumuna alindi.";
            TempData["Durum"] = "success";

            return RedirectToAction(nameof(Index), new
            {
                durum = SiparisDurumHelper.KargoyaVerildi,
                toast = Uri.EscapeDataString($"{siparisler.Count} siparis teslim edildi"),
                toastType = "success"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluDurumGuncelle(List<int> siparisIds, int yeniDurum)
        {
            siparisIds = siparisIds?.Where(x => x > 0).Distinct().ToList() ?? new List<int>();

            if (!siparisIds.Any())
            {
                TempData["Mesaj"] = "GÃ¼ncellemek iÃ§in en az bir sipariÅŸ seÃ§melisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id))
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "GÃ¼ncellenecek sipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var durumAd = SiparisDurumHelper.GetLabel(yeniDurum);
            var guncellenecekler = siparisler
                .Where(x => CanMoveOrderStatusForward(x.Durum, yeniDurum))
                .ToList();

            if (!guncellenecekler.Any())
            {
                TempData["Mesaj"] = $"SeÃ§ili sipariÅŸler '{durumAd}' durumuna alÄ±namaz. SipariÅŸ durumu geriye dÃ¶ndÃ¼rÃ¼lemez.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            foreach (var siparis in guncellenecekler)
            {
                siparis.Durum = yeniDurum;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{guncellenecekler.Count} sipariÅŸ '{durumAd}' durumuna gÃ¼ncellendi.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Index), new { toast = Uri.EscapeDataString($"{guncellenecekler.Count} sipariÅŸ durumu gÃ¼ncellendi"), toastType = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluIptal(string siparisIds)
        {
            if (string.IsNullOrWhiteSpace(siparisIds))
            {
                TempData["Mesaj"] = "Ä°ptal etmek iÃ§in en az bir sipariÅŸ seÃ§melisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var idList = siparisIds.Split(',').Select(x => int.TryParse(x.Trim(), out var id) ? id : 0).Where(x => x > 0).Distinct().ToList();

            if (!idList.Any())
            {
                TempData["Mesaj"] = "Ä°ptal etmek iÃ§in en az bir sipariÅŸ seÃ§melisiniz.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var siparisler = await _context.Siparisler
                .Where(x =>
                    idList.Contains(x.Id) &&
                    x.Durum != SiparisDurumHelper.KargoyaVerildi &&
                    x.Durum != SiparisDurumHelper.TeslimEdildi &&
                    x.Durum != SiparisDurumHelper.IptalEdildi)
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "Ä°ptal edilebilecek (Kargoya VerilmemiÅŸ) sipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            foreach (var siparis in siparisler)
            {
                siparis.Durum = SiparisDurumHelper.IptalEdildi;
                
                if (!string.IsNullOrWhiteSpace(siparis.Eposta))
                {
                    await SendIptalEmailAsync(siparis);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{siparisler.Count} sipariÅŸ iptal edildi ve mÃ¼ÅŸterilere e-posta gÃ¶nderildi.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Index), new { toast = Uri.EscapeDataString($"{siparisler.Count} sipariÅŸ iptal edildi"), toastType = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SiparisIptal(int id)
        {
            var siparis = await _context.Siparisler.FindAsync(id);
            
            if (siparis == null)
            {
                TempData["Mesaj"] = "SipariÅŸ bulunamadÄ±.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            if (siparis.Durum == SiparisDurumHelper.KargoyaVerildi)
            {
                TempData["Mesaj"] = "Kargoya verilmiÅŸ sipariÅŸ iptal edilemez!";
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Detay), new { id });
            }

            siparis.Durum = SiparisDurumHelper.IptalEdildi;
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(siparis.Eposta))
            {
                await SendIptalEmailAsync(siparis);
            }

            TempData["Mesaj"] = "SipariÅŸ iptal edildi ve mÃ¼ÅŸteriye e-posta gÃ¶nderildi.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Detay), new { id });
        }

        private async Task SendIptalEmailAsync(Siparis siparis)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(siparis.Eposta) || !IsValidEmail(siparis.Eposta))
                    return;

                var siparisNo = string.IsNullOrWhiteSpace(siparis.SiparisNo) ? $"#{siparis.Id}" : siparis.SiparisNo;
                
                var icerik = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #dc3545;'>SipariÅŸiniz Ä°ptal Edildi</h2>
                        <p>Merhaba <strong>{siparis.MusteriAdSoyad}</strong>,</p>
                        <p>SipariÅŸiniz iptal edilmiÅŸtir:</p>
                        <div style='background: #f8f9fa; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                            <p><strong>SipariÅŸ No:</strong> {siparisNo}</p>
                            <p><strong>Ä°ptal Tarihi:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        </div>
                        <p>Ã–deme yapÄ±ldÄ±ysa, iadeniz 3-5 iÅŸ gÃ¼nÃ¼ iÃ§inde hesabÄ±nÄ±za yapÄ±lacaktÄ±r.</p>
                        <p>Herhangi bir sorunuz varsa bizimle iletiÅŸime geÃ§ebilirsiniz.</p>
                        <p style='margin-top: 30px;'>SaygÄ±larÄ±mÄ±zla,<br/><strong>7ANRPS48</strong></p>
                    </div>";

                await _emailService.SendMailAsync(siparis.Eposta, $"SipariÅŸ {siparisNo} - Ä°ptal", icerik);
            }
            catch
            {
            }
        }

        private async Task<(bool Success, string Message)> SendStatusNotificationAsync(
            Siparis siparis,
            int eskiDurum,
            int yeniDurum,
            string kargoFirmasi,
            string kargoTakipNo)
        {
            if (string.IsNullOrWhiteSpace(siparis.Eposta))
            {
                return (false, "mÃ¼ÅŸteri e-posta adresi boÅŸ.");
            }

            if (!IsValidEmail(siparis.Eposta))
            {
                return (false, "mÃ¼ÅŸteri e-posta adresi geÃ§erli formatta deÄŸil.");
            }

            try
            {
                if (SiparisDurumHelper.IsShipped(yeniDurum) && !string.IsNullOrWhiteSpace(kargoTakipNo))
                {
                    var kargoMailGitti = await _emailService.SendKargoNotificationEmail(
                        siparis.Eposta,
                        siparis.MusteriAdSoyad,
                        siparis.SiparisNo,
                        kargoFirmasi,
                        kargoTakipNo);

                    return kargoMailGitti
                        ? (true, string.Empty)
                        : (false, "Kargo e-postasÄ± SMTP tarafÄ±ndan gÃ¶nderilemedi.");
                }

                var yeniDurumLabel = SiparisDurumHelper.GetLabel(yeniDurum);
                var oncekiDurumLabel = SiparisDurumHelper.GetLabel(eskiDurum);
                var takipLink = Url.Action("SiparisDetay", "Profil", new { id = siparis.Id }, Request.Scheme) ?? string.Empty;
                var urunSatirlari = await BuildOrderItemsTableRowsAsync(siparis.Id);

                await _emailService.SendTemplateMailAsync(
                    siparis.Eposta,
                    $"Sipari\u015F durumunuz g\u00FCncellendi: {yeniDurumLabel}",
                    siparis.MusteriAdSoyad,
                    BuildStatusEmailContent(siparis, oncekiDurumLabel, yeniDurumLabel, yeniDurum, urunSatirlari),
                    takipLink,
                    "Sipari\u015Fimi G\u00F6r\u00FCnt\u00FCle");

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task<KargoFirmasi?> ResolveKargoFirmasiAsync(Siparis siparis, int? selectedId = null)
        {
            if (selectedId.HasValue)
            {
                var selected = await _context.KargoFirmalari
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == selectedId.Value && !x.SilindiMi && x.AktifMi);

                if (selected != null)
                {
                    return selected;
                }
            }

            if (siparis.KargoFirmasiId.HasValue)
            {
                var existing = await _context.KargoFirmalari
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == siparis.KargoFirmasiId.Value && !x.SilindiMi);

                if (existing != null)
                {
                    return existing;
                }
            }

            return await _context.KargoFirmalari
                .AsNoTracking()
                .Where(x => !x.SilindiMi && x.AktifMi)
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Ad)
                .FirstOrDefaultAsync();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return string.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static string BuildStatusEmailContent(Siparis siparis, string oncekiDurum, string yeniDurum, int durum, string urunSatirlari)
        {
            var siparisNo = System.Net.WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(siparis.SiparisNo) ? $"#{siparis.Id}" : siparis.SiparisNo);
            var oncekiDurumText = System.Net.WebUtility.HtmlEncode(oncekiDurum);
            var yeniDurumText = System.Net.WebUtility.HtmlEncode(yeniDurum);
            var durumMesaji = durum switch
            {
                SiparisDurumHelper.UretimHazirlaniyor => "SipariÅŸiniz Ã¼retim planÄ±na alÄ±ndÄ±. ÃœrÃ¼nleriniz Ã¶zenle hazÄ±rlanÄ±yor.",
                SiparisDurumHelper.Paketleniyor => "ÃœrÃ¼nleriniz hazÄ±rlandÄ± ve korunaklÄ± paketleme aÅŸamasÄ±na geÃ§ti.",
                SiparisDurumHelper.TeslimEdildi => "SipariÅŸiniz teslim edildi olarak gÃ¼ncellendi. GÃ¼le gÃ¼le kullanmanÄ±zÄ± dileriz.",
                SiparisDurumHelper.IptalEdildi => "SipariÅŸiniz iptal edildi olarak gÃ¼ncellendi. Detaylar iÃ§in bizimle iletiÅŸime geÃ§ebilirsiniz.",
                SiparisDurumHelper.IadeTalebi => "Ä°ade talebiniz alÄ±ndÄ± ve ekibimiz tarafÄ±ndan inceleniyor.",
                SiparisDurumHelper.IadeOnaylandi => "Ä°ade talebiniz onaylandÄ±. Sonraki adÄ±mlar iÃ§in sizinle iletiÅŸime geÃ§eceÄŸiz.",
                SiparisDurumHelper.IadeTamamlandi => "Ä°ade sÃ¼reciniz tamamlandÄ±.",
                _ => "SipariÅŸinizin durumu gÃ¼ncellendi."
            };

            return $@"
                <p>SipariÅŸ numaranÄ±z <strong>{siparisNo}</strong> iÃ§in durum gÃ¼ncellemesi yapÄ±ldÄ±.</p>
                <p><strong>Ã–nceki durum:</strong> {oncekiDurumText}<br>
                <strong>Yeni durum:</strong> {yeniDurumText}</p>
                <p>{durumMesaji}</p>";
        }

        private async Task<string> BuildOrderItemsTableRowsAsync(int siparisId)
        {
            var detaylar = await _context.SiparisDetaylari
                .AsNoTracking()
                .Include(x => x.Urun)
                .Include(x => x.UrunSecenek)
                .Where(x => x.SiparisId == siparisId && !x.SilindiMi)
                .ToListAsync();

            var rows = new StringBuilder();
            foreach (var item in detaylar)
            {
                var urunAdi = System.Net.WebUtility.HtmlEncode(item.Urun?.Baslik ?? "ÃœrÃ¼n");
                var detayMetni = System.Net.WebUtility.HtmlEncode(BuildOrderLineDetail(item));
                var detayHtml = string.IsNullOrWhiteSpace(detayMetni)
                    ? string.Empty
                    : $"<div style='margin-top:4px; font-size:12px; color:#6f6a5e;'>{detayMetni}</div>";
                var notSatiri = !string.IsNullOrWhiteSpace(item.MusteriNotu)
                    ? $"<div style='margin-top:4px; font-size:12px; color:#b58735; font-style:italic;'>Not: {System.Net.WebUtility.HtmlEncode(item.MusteriNotu)}</div>"
                    : string.Empty;

                rows.Append($@"
                    <tr>
                        <td style='padding:10px; border-top:1px solid #e5e2dc; color:#47473d;'>
                            <div>{urunAdi}</div>
                            {detayHtml}
                            {notSatiri}
                        </td>
                        <td style='padding:10px; border-top:1px solid #e5e2dc; text-align:center; color:#47473d;'>{item.Adet}</td>
                        <td style='padding:10px; border-top:1px solid #e5e2dc; text-align:right; color:#313511; font-weight:600;'>{(item.BirimFiyat * item.Adet):N2} TL</td>
                    </tr>");
            }

            return rows.ToString();
        }

        private static string BuildOrderLineDetail(SiparisDetay item)
        {
            var details = new List<string>();
            var variant = item.UrunSecenek;
            if (variant != null)
            {
                var variantText = string.IsNullOrWhiteSpace(variant.VaryantBasligi)
                    ? variant.Olcu
                    : variant.VaryantBasligi;

                if (!string.IsNullOrWhiteSpace(variantText) &&
                    !variantText.Contains("Standart", StringComparison.OrdinalIgnoreCase))
                {
                    details.Add(variantText);
                }
            }

            if (!string.IsNullOrWhiteSpace(item.CerceveModeli) && item.CerceveModeli != "Ã‡erÃ§evesiz")
            {
                details.Add($"Ã‡erÃ§eve: {item.CerceveModeli}");
            }

            return string.Join(" | ", details);
        }

        private static bool CanMoveOrderStatusForward(int currentStatus, int nextStatus)
        {
            if (currentStatus == nextStatus)
            {
                return true;
            }

            if (currentStatus == SiparisDurumHelper.IptalEdildi ||
                currentStatus == SiparisDurumHelper.TeslimEdildi ||
                SiparisDurumHelper.IsReturn(currentStatus))
            {
                return false;
            }

            if (nextStatus == SiparisDurumHelper.IptalEdildi)
            {
                return currentStatus != SiparisDurumHelper.KargoyaVerildi &&
                       currentStatus != SiparisDurumHelper.TeslimEdildi;
            }

            if (!TryGetOperationalStatusRank(currentStatus, out var currentRank) ||
                !TryGetOperationalStatusRank(nextStatus, out var nextRank))
            {
                return false;
            }

            return nextRank > currentRank;
        }

        private static bool TryGetOperationalStatusRank(int status, out int rank)
        {
            rank = status switch
            {
                SiparisDurumHelper.SiparisAlindi => 1,
                SiparisDurumHelper.UretimHazirlaniyor => 2,
                SiparisDurumHelper.Paketleniyor => 3,
                SiparisDurumHelper.KargoyaVerildi => 4,
                SiparisDurumHelper.TeslimEdildi => 5,
                _ => 0
            };

            return rank > 0;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FaturaYukle(IFormFile faturaDosyasi, int siparisId)
        {
            if (faturaDosyasi == null || faturaDosyasi.Length == 0)
            {
                TempData["Hata"] = "LÃ¼tfen bir dosya seÃ§in.";
                return RedirectToAction("Detay", new { id = siparisId });
            }

            // Sadece PDF kontrolÃ¼ - hem iÃ§erik tipi hem uzantÄ±
            var allowedContentTypes = new[] { "application/pdf" };
            var allowedExtensions = new[] { ".pdf" };
            var fileExtension = Path.GetExtension(faturaDosyasi.FileName).ToLowerInvariant();
            var contentType = faturaDosyasi.ContentType?.ToLowerInvariant();

            if (!allowedContentTypes.Contains(contentType) || !allowedExtensions.Contains(fileExtension))
            {
                TempData["Hata"] = "Sadece PDF dosyalarÄ± yÃ¼klenebilir.";
                return RedirectToAction("Detay", new { id = siparisId });
            }

            // Dosya boyutu kontrolÃ¼ (5MB max)
            const long maxFileSize = 5 * 1024 * 1024;
            if (faturaDosyasi.Length > maxFileSize)
            {
                TempData["Hata"] = "Dosya boyutu maksimum 5 MB olabilir.";
                return RedirectToAction("Detay", new { id = siparisId });
            }

            var siparis = await _context.Siparisler.FindAsync(siparisId);
            if (siparis == null)
            {
                TempData["Hata"] = "SipariÅŸ bulunamadÄ±.";
                return RedirectToAction("Index");
            }

            // Upload klasÃ¶rÃ¼ oluÅŸtur
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "invoices");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // GÃ¼venli dosya adÄ±: siparisId_benzersizGuid.pdf
            var safeFileName = $"{siparisId}_{Guid.NewGuid():N}.pdf";
            var filePath = Path.Combine(uploadsPath, safeFileName);
            var relativePath = $"/uploads/invoices/{safeFileName}";

            // Eski fatura varsa sil
            if (!string.IsNullOrWhiteSpace(siparis.FaturaDosyaYolu))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath, siparis.FaturaDosyaYolu.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    try { System.IO.File.Delete(oldFilePath); }
                    catch { /* Silinemezse ignore */ }
                }
            }

            // DosyayÄ± kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await faturaDosyasi.CopyToAsync(stream);
            }

            // SipariÅŸ fatura bilgilerini gÃ¼ncelle
            siparis.FaturaDosyaYolu = relativePath;
            siparis.FaturaDosyaAdi = faturaDosyasi.FileName;
            siparis.FaturaYuklendiMi = true;
            siparis.FaturaYuklenmeTarihi = DateTime.UtcNow;

            // E-posta ile fatura gÃ¶nder
            var mailGonderildi = false;
            if (!string.IsNullOrWhiteSpace(siparis.Eposta))
            {
                mailGonderildi = await _emailService.SendInvoiceEmailAsync(
                    siparis.Eposta,
                    siparis.MusteriAdSoyad,
                    siparis.SiparisNo ?? siparis.Id.ToString(),
                    filePath);

                if (mailGonderildi)
                {
                    siparis.FaturaMailGonderildiMi = true;
                    siparis.FaturaMailGonderimTarihi = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Basarili"] = mailGonderildi
                ? "Fatura baÅŸarÄ±yla yÃ¼klendi ve mÃ¼ÅŸteriye e-posta gÃ¶nderildi."
                : "Fatura yÃ¼klendi ancak e-posta gÃ¶nderilemedi.";

            return RedirectToAction("Detay", new { id = siparisId });
        }

        [HttpGet]
        public async Task<IActionResult> FaturaIndir(int id)
        {
            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null || string.IsNullOrWhiteSpace(siparis.FaturaDosyaYolu))
            {
                return NotFound("Fatura bulunamadÄ±.");
            }

            var filePath = Path.Combine(_env.WebRootPath, siparis.FaturaDosyaYolu.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Fatura dosyasÄ± bulunamadÄ±.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", siparis.FaturaDosyaAdi ?? $"fatura_{id}.pdf");
        }

        /// <summary>
        /// Reçeteyi onaylar.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceteOnayla(int id)
        {
            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                TempData["Hata"] = "Sipariş bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            siparis.ReceteOnayDurumu = 1;
            siparis.ReceteRedSebebi = null;
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Reçete onaylandı.";
            return RedirectToAction(nameof(Detay), new { id });
        }

        /// <summary>
        /// Reçeteyi reddeder.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceteReddet(int id, string redSebebi)
        {
            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                TempData["Hata"] = "Sipariş bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(redSebebi))
            {
                TempData["Hata"] = "Red sebebi zorunludur.";
                return RedirectToAction(nameof(Detay), new { id });
            }

            siparis.ReceteOnayDurumu = 2;
            siparis.ReceteRedSebebi = redSebebi.Trim();
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Reçete reddedildi: " + redSebebi.Trim();
            return RedirectToAction(nameof(Detay), new { id });
        }

        /// <summary>
        /// Siparis verisinden QuestPDF ile anlik PDF faturasi olusturup indirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FaturaPdf(int id)
        {
            try
            {
                var siparis = await _context.Siparisler
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (siparis == null)
                {
                    return NotFound("Siparis bulunamadi.");
                }

                var pdfBytes = await _faturaPdfService.GenerateInvoicePdfAsync(id);

                var siparisNo = string.IsNullOrWhiteSpace(siparis.SiparisNo)
                    ? $"fatura_{id}"
                    : $"fatura_{siparis.SiparisNo}";

                return File(pdfBytes, "application/pdf", $"{siparisNo}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF fatura olusturma hatasi. SiparisId={SiparisId}", id);
                TempData["Hata"] = "Fatura olusturulurken bir hata olustu.";
                return RedirectToAction(nameof(Detay), new { id });
            }
        }
    }
}
