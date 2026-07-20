using System.Text;
using FilistinProje.Core.Helpers;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Services;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IDosyaServisi _dosyaServisi;
        private readonly ILogger<SiparisController> _logger;

        private readonly Microsoft.Extensions.Localization.IStringLocalizer<FilistinProje.Web.Resources.SharedResource> _localizer; public SiparisController(
            IService<Siparis> siparisService,
            IService<SiparisDetay> detayService,
            IService<UrunSecenek> secenekService,
            IService<Urun> urunService,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            IFaturaPdfService faturaPdfService,
            KanvasDbContext context,
            IDosyaServisi dosyaServisi,
            ILogger<SiparisController> logger, Microsoft.Extensions.Localization.IStringLocalizer<FilistinProje.Web.Resources.SharedResource> localizer)
        {
            _siparisService = siparisService;
            _detayService = detayService;
            _secenekService = secenekService;
            _urunService = urunService;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
            _faturaPdfService = faturaPdfService;
            _context = context;
            
            _dosyaServisi = dosyaServisi;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string search, int? durum, int? receteDurum, int page = 1, int pageSize = 20)
        {
            page = Math.Max(page, 1);
            var allowedPageSizes = new[] { 20, 50, 100 };
            if (!allowedPageSizes.Contains(pageSize))
            {
                pageSize = 20;
            }

            var tumSiparisler = _context.Siparisler
                .AsNoTracking()
                .Where(x => !x.SilindiMi);
            var sorgu = tumSiparisler;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLowerInvariant();
                sorgu = sorgu.Where(x =>
                    x.Id.ToString().Contains(search) ||
                    (!string.IsNullOrWhiteSpace(x.SiparisNo) && x.SiparisNo.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(x.MusteriAdSoyad) && x.MusteriAdSoyad.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(x.Telefon) && x.Telefon.Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(x.Eposta) && x.Eposta.ToLower().Contains(search)));
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
            ViewBag.ReceteBekleyenCount = await tumSiparisler.CountAsync(x => x.ReceteOnayDurumu == 0 && !string.IsNullOrWhiteSpace(x.ReceteDosyaYolu));
            ViewBag.ReceteOnayliCount = await tumSiparisler.CountAsync(x => x.ReceteOnayDurumu == 1);
            ViewBag.ReceteRedliCount = await tumSiparisler.CountAsync(x => x.ReceteOnayDurumu == 2);
            ViewBag.TumuCount = await tumSiparisler.CountAsync();
            ViewBag.YeniCount = await tumSiparisler.CountAsync(x => x.Durum == SiparisDurumHelper.SiparisAlindi);
            ViewBag.HazirlaniyorCount = await tumSiparisler.CountAsync(x => x.Durum == SiparisDurumHelper.UretimHazirlaniyor);
            ViewBag.PaketleniyorCount = await tumSiparisler.CountAsync(x => x.Durum == SiparisDurumHelper.Paketleniyor);
            ViewBag.KargodaCount = await tumSiparisler.CountAsync(x => x.Durum == SiparisDurumHelper.KargoyaVerildi);
            ViewBag.TeslimCount = await tumSiparisler.CountAsync(x => x.Durum == SiparisDurumHelper.TeslimEdildi);
            ViewBag.FaturaYuklenmemisCount = await tumSiparisler.CountAsync(x => !x.FaturaYuklendiMi);
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.PageSizeOptions = allowedPageSizes;
            ViewBag.TotalCount = await sorgu.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / pageSize);
            if (ViewBag.TotalPages < 1)
            {
                ViewBag.TotalPages = 1;
            }

            page = Math.Min(page, (int)ViewBag.TotalPages);
            ViewBag.Page = page;

            var sayfaSiparisleri = await sorgu
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var sayfaSiparisIdleri = sayfaSiparisleri.Select(x => x.Id).ToList();
            ViewBag.SiparisDetayOzetleri = await _context.SiparisDetaylari
                .AsNoTracking()
                .Where(x => sayfaSiparisIdleri.Contains(x.SiparisId))
                .GroupBy(x => x.SiparisId)
                .Select(x => new
                {
                    SiparisId = x.Key,
                    UrunSayisi = x.Count(),
                    Adet = x.Sum(v => v.Adet)
                })
                .ToDictionaryAsync(
                    x => x.SiparisId,
                    x => $"{x.UrunSayisi} Ã¼rÃ¼n / {x.Adet} adet");

            return View(sayfaSiparisleri);
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
                    Secenek = string.IsNullOrWhiteSpace(secenek?.VaryantBasligi) ? "المتغير الافتراضي" : secenek.VaryantBasligi,
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
                TempData["Mesaj"] = "لم يتم العثور على الطلب.";
                TempData["Durum"] = "danger";
                return RedirectToAction("Detay", new { id });
            }

            var eskiDurum = siparis.Durum;
            var temizKargoNo = kargoNo?.Trim() ?? string.Empty;
            var firma = await ResolveKargoFirmasiAsync(siparis, kargoFirmasiId);

            if (durum != eskiDurum && !CanMoveOrderStatusForward(eskiDurum, durum))
            {
                TempData["Mesaj"] = $"لا يمكن تراجع حالة الطلب من '{SiparisDurumHelper.GetLabel(eskiDurum)}' إلى '{SiparisDurumHelper.GetLabel(durum)}'.";
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
                TempData["Mesaj"] = "?? ????? ??????? ????? ?????.";
                TempData["Mesaj"] = "تم تحديث معلومات الطلب.";
                return RedirectToAction("Detay", new { id });
            }

            var mailSonucu = await SendStatusNotificationAsync(siparis, eskiDurum, durum, firma?.Ad ?? string.Empty, temizKargoNo);
            if (mailSonucu.Success)
            {
                TempData["Mesaj"] = "تم تحديث الطلب. تم إرسال إشعار للعميل.";
                TempData["Durum"] = "success";
            }
            else
            {
                TempData["Mesaj"] = $"تم تحديث الطلب. فشل إرسال الإشعار: {mailSonucu.Message}";
                TempData["Durum"] = "warning";
            }

            return RedirectToAction("Detay", new { id });
        }

        public async Task<IActionResult> ExcelExport()
        {
            var siparisler = await _siparisService.GetAllAsync();
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("الطلبات");

            var headers = new[]
            {
                "رقم الطلب",
                "العميل",
                "البريد الإلكتروني",
                "الهاتف",
                "المدينة",
                "المنطقة",
                "التاريخ",
                "المبلغ",
                "الحالة",
                "شركة الشحن",
                "رقم التتبع"
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
            worksheet.Column(8).Style.Numberformat.Format = "#,##0.00 ?";
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
                TempData["Mesaj"] = "اختر طلباً للطباعة.";
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
                TempData["Mesaj"] = "لا توجد طلبات قابلة للطباعة.";
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
                TempData["Mesaj"] = "اختر طلباً للموافقة.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id) && x.Durum == SiparisDurumHelper.SiparisAlindi)
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "لا توجد طلبات جديدة للموافقة.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            foreach (var siparis in siparisler)
            {
                siparis.Durum = SiparisDurumHelper.UretimHazirlaniyor;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{siparisler.Count} طلب قيد التحضير.";
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
                TempData["Mesaj"] = "اختر طلباً للشحن.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var validDurumlar = new[] { SiparisDurumHelper.UretimHazirlaniyor, SiparisDurumHelper.Paketleniyor };
            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id) && validDurumlar.Contains(x.Durum))
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "لا توجد طلبات قيد التحضير للشحن.";
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

            TempData["Mesaj"] = $"{siparisler.Count} طلب تم شحنه.";
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
                TempData["Mesaj"] = "اختر طلباً للتسليم.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum = SiparisDurumHelper.KargoyaVerildi });
            }

            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id) && x.Durum == SiparisDurumHelper.KargoyaVerildi)
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "لا توجد طلبات في الشحن للتسليم.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index), new { durum = SiparisDurumHelper.KargoyaVerildi });
            }

            foreach (var siparis in siparisler)
            {
                siparis.Durum = SiparisDurumHelper.TeslimEdildi;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{siparisler.Count} طلب تم تسليمه.";
            TempData["Durum"] = "success";

            return RedirectToAction(nameof(Index), new
            {
                durum = SiparisDurumHelper.KargoyaVerildi,
                toast = Uri.EscapeDataString($"{siparisler.Count} طلب تم تسليمه"),
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
                TempData["Mesaj"] = "اختر طلباً واحداً على الأقل لتحديث الحالة.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var siparisler = await _context.Siparisler
                .Where(x => siparisIds.Contains(x.Id))
                .ToListAsync();

            if (!siparisler.Any())
            {
                TempData["Mesaj"] = "لم يتم العثور على طلبات للتحديث.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var durumAd = SiparisDurumHelper.GetLabel(yeniDurum);
            var guncellenecekler = siparisler
                .Where(x => CanMoveOrderStatusForward(x.Durum, yeniDurum))
                .ToList();

            if (!guncellenecekler.Any())
            {
                TempData["Mesaj"] = $"لا يمكن تحديث الحالة إلى '{durumAd}'.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            foreach (var siparis in guncellenecekler)
            {
                siparis.Durum = yeniDurum;
            }

            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{guncellenecekler.Count} طلب تم تحديثه إلى '{durumAd}'.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Index), new { toast = Uri.EscapeDataString($"{guncellenecekler.Count} طلب محدّث"), toastType = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluIptal(string siparisIds)
        {
            if (string.IsNullOrWhiteSpace(siparisIds))
            {
                TempData["Mesaj"] = "اختر طلباً واحداً على الأقل للإلغاء.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var idList = siparisIds.Split(',').Select(x => int.TryParse(x.Trim(), out var id) ? id : 0).Where(x => x > 0).Distinct().ToList();

            if (!idList.Any())
            {
                TempData["Mesaj"] = "اختر طلباً واحداً على الأقل للإلغاء.";
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
                TempData["Mesaj"] = "لا توجد طلبات قابلة للإلغاء.";
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

            TempData["Mesaj"] = $"{siparisler.Count} طلب تم إلغاؤه وإرسال إشعار للعملاء.";
            TempData["Durum"] = "success";
            
            return RedirectToAction(nameof(Index), new { toast = Uri.EscapeDataString($"{siparisler.Count} طلب ملغى"), toastType = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SiparisIptal(int id)
        {
            var siparis = await _context.Siparisler.FindAsync(id);
            
            if (siparis == null)
            {
                TempData["Mesaj"] = "لم يتم العثور على الطلب.";
                TempData["Durum"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            if (siparis.Durum == SiparisDurumHelper.KargoyaVerildi)
            {
                TempData["Mesaj"] = "لا يمكن إلغاء طلب تم شحنه!";
                TempData["Durum"] = "danger";
                return RedirectToAction(nameof(Detay), new { id });
            }

            siparis.Durum = SiparisDurumHelper.IptalEdildi;
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(siparis.Eposta))
            {
                await SendIptalEmailAsync(siparis);
            }

                TempData["Mesaj"] = "تم إلغاء الطلب وإرسال إشعار للعميل.";
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
                        <h2 style='color: #dc3545;'>تم إلغاء طلبك</h2>
                        <p>مرحبًا <strong>{siparis.MusteriAdSoyad}</strong>،</p>
                        <p>تم إلغاء طلبك:</p>
                        <div style='background: #f8f9fa; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                            <p><strong>رقم الطلب:</strong> {siparisNo}</p>
                            <p><strong>تاريخ الإلغاء:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        </div>
                        <p>إذا كنت قد أتممت الدفع، سيتم استرداد المبلغ خلال 3-5 أيام عمل.</p>
                        <p>إذا كان لديك أي استفسار، يرجى التواصل معنا.</p>
                        <p style='margin-top: 30px;'>مع تحياتنا،<br/><strong>7ANRPS48</strong></p>
                    </div>";

                await _emailService.SendMailAsync(siparis.Eposta, $"طلب {siparisNo} - إلغاء", icerik);
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
                return (false, "البريد الإلكتروني للعميل فارغ.");
            }

            if (!IsValidEmail(siparis.Eposta))
            {
                return (false, "البريد الإلكتروني للعميل غير صالح.");
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
                        : (false, "تعذّر إرسال بريد الشحن عبر SMTP.");
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
                SiparisDurumHelper.UretimHazirlaniyor => "?? ????? ???? ?? ??? ???????. ???? ????? ??????? ??????.",
                SiparisDurumHelper.Paketleniyor => "?? ????? ??????? ??????? ??? ????? ??????? ?????.",
                SiparisDurumHelper.TeslimEdildi => "?? ????? ???? ??????. ????? ?? ????????? ??????.",
                SiparisDurumHelper.IptalEdildi => "?? ????? ???? ?????. ????? ??????? ???? ????????.",
                SiparisDurumHelper.IadeTalebi => "?? ?????? ??? ??????? ????? ?? ??? ??? ???????? ?? ??? ??????.",
                SiparisDurumHelper.IadeOnaylandi => "??? ???????? ??? ??? ??????? ????? ??. ??????? ??? ??????? ???????.",
                SiparisDurumHelper.IadeTamamlandi => "?????? ????? ??????? ?????? ??.",
                _ => "?? ????? ???? ????."
            };

            return $@"
                <p>?? ????? ???? ???? ??? <strong>{siparisNo}</strong>.</p>
                <p><strong>?????? ???????:</strong> {oncekiDurumText}<br>
                <strong>?????? ???????:</strong> {yeniDurumText}</p>
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
                var urunAdi = System.Net.WebUtility.HtmlEncode(item.Urun?.Baslik ?? "منتج");
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
                        <td style='padding:10px; border-top:1px solid #e5e2dc; text-align:right; color:#313511; font-weight:600;'>{(item.BirimFiyat * item.Adet):N2} ?</td>
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

            if (!string.IsNullOrWhiteSpace(item.CerceveModeli) && item.CerceveModeli != "Çerçevesiz")
            {
                details.Add($"إطار: {item.CerceveModeli}");
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
                TempData["Hata"] = "يرجى اختيار ملف.";
                return RedirectToAction("Detay", new { id = siparisId });
            }

            var siparis = await _context.Siparisler.FindAsync(siparisId);
            if (siparis == null)
            {
                TempData["Hata"] = _localizer["Admin_Sonuc_bulunamadi"].Value;
                return RedirectToAction("Index");
            }

            var kayit = await _dosyaServisi.HassasBelgeKaydetAsync(faturaDosyasi, HassasBelgeKategorisi.Fatura);
            if (!kayit.Success)
            {
                TempData["Hata"] = kayit.ErrorMessage ?? "فشل حفظ الفاتورة بأمان.";
                return RedirectToAction("Detay", new { id = siparisId });
            }

            var oncekiReferans = siparis.FaturaDosyaYolu;
            var privateReference = DosyaServisi.BuildPrivateReference(HassasBelgeKategorisi.Fatura, kayit.BelgeAdi);
            var filePath = Path.Combine(
                _dosyaServisi.GetPrivateStorageRoot(),
                "hassas",
                DosyaServisi.GetCategorySegment(HassasBelgeKategorisi.Fatura),
                kayit.BelgeAdi);

            siparis.FaturaDosyaYolu = privateReference;
            siparis.FaturaDosyaAdi = $"fatura_{siparis.Id}.pdf";
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                _dosyaServisi.HassasBelgeSil(HassasBelgeKategorisi.Fatura, kayit.BelgeAdi);
                throw;
            }

            if (!string.IsNullOrWhiteSpace(oncekiReferans) &&
                !string.Equals(oncekiReferans, privateReference, StringComparison.Ordinal))
            {
                _dosyaServisi.Sil(oncekiReferans);
            }

            TempData["Basarili"] = mailGonderildi
                ? "تم رفع الفاتورة وإرسالها للعميل."
                : "تم رفع الفاتورة لكن فشل إرسال البريد.";

            return RedirectToAction("Detay", new { id = siparisId });
        }

        [HttpGet]
        public async Task<IActionResult> FaturaIndir(int id)
        {
            return RedirectToAction("Fatura", "Belge", new { area = "", siparisId = id, indir = true });
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
                TempData["Hata"] = "لم يتم العثور على الطلب.";
                return RedirectToAction(nameof(Index));
            }

            siparis.ReceteOnayDurumu = 1;
            siparis.ReceteRedSebebi = null;
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "تمت الموافقة على الوصفة.";
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
                TempData["Hata"] = "لم يتم العثور على الطلب.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(redSebebi))
            {
                TempData["Hata"] = "سبب الرفض إلزامي.";
                return RedirectToAction(nameof(Detay), new { id });
            }

            siparis.ReceteOnayDurumu = 2;
            siparis.ReceteRedSebebi = redSebebi.Trim();
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "تم رفض الوصفة: " + redSebebi.Trim();
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
                TempData["Hata"] = "حدث خطأ أثناء إنشاء الفاتورة.";
                return RedirectToAction(nameof(Detay), new { id });
            }
        }
    }
}






