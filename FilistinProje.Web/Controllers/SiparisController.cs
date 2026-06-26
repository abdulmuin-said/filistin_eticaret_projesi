using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;


namespace FilistinProje.Web.Controllers
{
    public class SiparisController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IService<Adres> _adresService;
        private readonly KanvasDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISepetService _sepetService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly ILogger<SiparisController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IKargoHesaplamaServisi _kargoHesaplama;
        private readonly IDosyaServisi _dosyaServisi;

        public SiparisController(
            UserManager<AppUser> userManager,
            IService<Adres> adresService,
            KanvasDbContext context,
            IEmailService emailService,
            ISepetService sepetService,
            ISiteSettingsService siteSettingsService,
            ILogger<SiparisController> logger,
            IWebHostEnvironment env,
            IStringLocalizer<SharedResource> localizer,
            IKargoHesaplamaServisi kargoHesaplama,
            IDosyaServisi dosyaServisi)
        {
            _userManager = userManager;
            _adresService = adresService;
            _context = context;
            _emailService = emailService;
            _sepetService = sepetService;
            _siteSettingsService = siteSettingsService;
            _logger = logger;
            _env = env;
            _localizer = localizer;
            _kargoHesaplama = kargoHesaplama;
            _dosyaServisi = dosyaServisi;
        }

        [HttpGet]
        public async Task<IActionResult> Odeme()
        {
            var userId = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
            var sessionId = HttpContext.Session.Id;

            var sepetItems = await _sepetService.GetSepetItemsAsync(userId, sessionId);
            if (sepetItems == null || !sepetItems.Any())
            {
                return RedirectToAction("Index", "Sepet");
            }

            await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);

            var model = new Siparis();
            
            // Kargo firmasi secimi kaldirildi - kargo hesaplama simdilik sehre gore yapilacak

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var tumAdresler = await _adresService.GetAllAsync();
                    ViewBag.KayitliAdresler = tumAdresler.Where(x => x.AppUserId == user.Id).ToList();

                    model.MusteriAdSoyad = user.AdSoyad;
                    model.Eposta = user.Email ?? string.Empty;
                    model.Telefon = user.PhoneNumber ?? string.Empty;
                    model.Sehir = user.Sehir;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odeme(Siparis siparis, string? yeniAdresBasligi, bool adresiKaydet, bool sozlesmeOnaylandi, string odemeYontemi)
        {
            var userId = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
            var sessionId = HttpContext.Session.Id;

            var sepetItems = await _sepetService.GetSepetItemsAsync(userId, sessionId);
            if (sepetItems == null || !sepetItems.Any())
            {
                return RedirectToAction("Index", "Sepet");
            }

            NormalizeCheckoutInput(siparis);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    siparis.Eposta = currentUser.Email ?? siparis.Eposta;
                }
            }

            if (!sozlesmeOnaylandi)
            {
                ModelState.AddModelError("sozlesmeOnaylandi", _localizer["Siparis_TermsRequired"].Value);
            }

            var urunIds = sepetItems.Select(x => x.UrunId).ToList();
            var receteZorunluMu = await _context.Urunler
                .Where(u => urunIds.Contains(u.Id) && !u.SilindiMi)
                .AnyAsync(u => u.Kategori != null && u.Kategori.ReceteGerekliMi);

            if (receteZorunluMu && !IsSafeUploadedPath(siparis.ReceteDosyaYolu, "uploads/receteler", allowPdf: true))
            {
                ModelState.AddModelError("ReceteDosyaYolu", _localizer["Siparis_PrescriptionRequired"].Value);
            }

            if (!string.IsNullOrWhiteSpace(siparis.KimlikFotoYolu) && !IsSafeUploadedPath(siparis.KimlikFotoYolu, "uploads/kimlikler", allowPdf: false))
            {
                ModelState.AddModelError(nameof(Siparis.KimlikFotoYolu), _localizer["Siparis_FileUploadError"].Value);
            }

            if (!ValidateCheckoutInput(siparis) || !ModelState.IsValid)
            {
                await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);
                ViewBag.FormHata = _localizer["Siparis_FormValidationError"].Value;
                return View(siparis);
            }

            decimal hamTutar = await _sepetService.GetSepetToplamiAsync(userId, sessionId);
            var settings = _siteSettingsService.GetSettings();

            siparis.SiparisNo = await GenerateUniqueOrderNumberAsync();
            siparis.EmailHashKodu = Guid.NewGuid().ToString("N")[..16];
            siparis.OlusturulmaTarihi = DateTime.UtcNow;
            siparis.Durum = 0;
            siparis.SilindiMi = false;
            siparis.AppUserId = userId;
            siparis.KargoTakipNo ??= string.Empty;

            if (odemeYontemi == "KapidaOdeme" && settings.KapidaOdemeAktifMi)
            {
                siparis.OdemeYontemi = "KapidaOdeme";
                siparis.KapidaOdemeHizmetBedeli = settings.KapidaOdemeHizmetBedeli;
            }
            else
            {
                siparis.OdemeYontemi = "BankaHavalesi";
                siparis.KapidaOdemeHizmetBedeli = 0;
            }

            var kullaniciNotu = siparis.Aciklama;
            siparis.Aciklama = siparis.OdemeYontemi == "KapidaOdeme"
                ? _localizer["Siparis_PayOnDeliveryPending"].Value
                : _localizer["Siparis_PaymentPending"].Value;

            if (!string.IsNullOrWhiteSpace(kullaniciNotu))
            {
                siparis.Aciklama += " | Not: " + TrimToMaxLength(kullaniciNotu, 1000);
            }
            siparis.ToplamTutar = hamTutar;
            siparis.IndirimTutari = 0;
            siparis.KuponKodu = null;

            string? kuponKodu = HttpContext.Session.GetString("UygulananKupon");
            if (!string.IsNullOrEmpty(kuponKodu))
            {
                var kupon = await _context.Kuponlar.FirstOrDefaultAsync(x => x.Kod == kuponKodu && !x.SilindiMi);
                if (kupon != null &&
                    kupon.AktifMi &&
                    kupon.SonKullanmaTarihi > DateTime.UtcNow &&
                    (kupon.KullanimLimiti <= 0 || kupon.KullanilanMiktar < kupon.KullanimLimiti) &&
                    hamTutar >= kupon.MinSepetTutari)
                {
                    var indirim = CalculateCouponDiscount(kupon, hamTutar);
                    siparis.IndirimTutari = indirim;
                    siparis.KuponKodu = kupon.Kod;
                    siparis.ToplamTutar = Math.Max(0, siparis.ToplamTutar - indirim);
                }
            }

            decimal sepetToplamiIndirimli = hamTutar - siparis.IndirimTutari;
            if (siparis.OdemeYontemi == "KapidaOdeme" && sepetToplamiIndirimli > settings.KapidaOdemeLimiti)
            {
                ModelState.AddModelError("odemeYontemi", string.Format(_localizer["Siparis_CODLimitExceeded"].Value, settings.KapidaOdemeLimiti.ToString("N0"), settings.ParaBirimi));
                await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);
                ViewBag.FormHata = string.Format(_localizer["Siparis_CODLimitExceeded"].Value, settings.KapidaOdemeLimiti.ToString("N0"), settings.ParaBirimi);
                return View(siparis);
            }

            // --- TOPTANCI MINIMUM SİPARİŞ KONTROLÜ ---
            if (User.IsInRole("Wholesale") && settings.ToptanciMinSiparisTutari > 0 && sepetToplamiIndirimli < settings.ToptanciMinSiparisTutari)
            {
                var minOrderMsg = string.Format(_localizer["Siparis_WholesaleMinOrder"].Value, settings.ToptanciMinSiparisTutari.ToString("N0"), settings.ParaBirimi);
                ModelState.AddModelError("SepetToplami", minOrderMsg);
                await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);
                ViewBag.FormHata = minOrderMsg;
                return View(siparis);
            }

            // --- KARGO HESAPLAMA VE KAYDETME ---
            decimal kargoUcreti = 0;

            if (siparis.TeslimatTipi == "MagazadanTeslim")
            {
                siparis.KargoFirmasiId = null;
                siparis.KargoFirmasi = null;
                siparis.Sehir = _localizer["StorePickupCity"];
                siparis.Ilce = _localizer["StorePickupDistrict"];
                siparis.AcikAdres = _localizer["StorePickupAddress"];
                siparis.KargoUcreti = 0;
                adresiKaydet = false;
            }
            else
            {
                kargoUcreti = await _kargoHesaplama.HesaplaAsync(
                    siparis.Sehir,
                    siparis.ToplamTutar,
                    settings.UcretsizKargoLimiti);

                siparis.KargoFirmasi = "";
                siparis.KargoFirmasiId = null;
                siparis.KargoUcreti = kargoUcreti;
            }

            siparis.ToplamTutar = siparis.ToplamTutar + kargoUcreti + siparis.KapidaOdemeHizmetBedeli;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (adresiKaydet && !string.IsNullOrEmpty(userId))
                {
                    _context.Adresler.Add(new Adres
                    {
                        AppUserId = userId,
                        Baslik = string.IsNullOrWhiteSpace(yeniAdresBasligi) ? "Yeni Adresim" : yeniAdresBasligi.Trim(),
                        AdSoyad = siparis.MusteriAdSoyad,
                        Telefon = siparis.Telefon,
                        Sehir = siparis.Sehir,
                        Ilce = siparis.Ilce,
                        AcikAdres = siparis.AcikAdres,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        SilindiMi = false
                    });
                }

                _context.Siparisler.Add(siparis);
                await _context.SaveChangesAsync();

                foreach (var item in sepetItems)
                {
                    var gercekSecenekId = await ResolveOrderVariantIdAsync(item);

                    _context.SiparisDetaylari.Add(new SiparisDetay
                    {
                        SiparisId = siparis.Id,
                        UrunSecenekId = gercekSecenekId,
                        Adet = item.Adet,
                        BirimFiyat = item.Fiyat,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        UrunId = item.UrunId,
                        CerceveModeli = item.CerceveModeli,
                        MusteriNotu = item.MusteriNotu,
                        HediyePaketi = item.HediyePaketi,
                        HediyePaketFiyati = item.HediyePaketFiyati,
                        SilindiMi = false
                    });
                }

                if (!string.IsNullOrWhiteSpace(siparis.KuponKodu))
                {
                    var kupon = await _context.Kuponlar.FirstOrDefaultAsync(x => x.Kod == siparis.KuponKodu);
                    if (kupon != null)
                    {
                        kupon.KullanilanMiktar++;
                    }
                }

                await ClearOrderCartAsync(siparis);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            HttpContext.Session.Remove("UygulananKupon");

            _logger.LogInformation(
                "Siparis odeme bekliyor durumunda olusturuldu. SiparisNo={SiparisNo}, Tutar={Tutar}",
                siparis.SiparisNo,
                siparis.ToplamTutar);

            await SendAdminOrderNotificationEmailAsync(siparis);
            await SendCustomerOrderConfirmationEmailAsync(siparis);

            return RedirectToAction(nameof(Beklemede), new { siparisNo = siparis.SiparisNo });
        }

        [HttpGet]
        public async Task<IActionResult> KargoHesapla(string sehir)
        {
            try
            {
                var settings = _siteSettingsService.GetSettings();

                var userId = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
                var sessionId = HttpContext.Session.Id;
                decimal siparisToplami = await _sepetService.GetSepetToplamiAsync(userId, sessionId);

                string? kuponKodu = HttpContext.Session.GetString("UygulananKupon");
                if (!string.IsNullOrEmpty(kuponKodu))
                {
                    var kupon = await _context.Kuponlar.FirstOrDefaultAsync(x => x.Kod == kuponKodu && !x.SilindiMi);
                    if (kupon != null && kupon.AktifMi && kupon.SonKullanmaTarihi > DateTime.UtcNow)
                    {
                        var indirim = kupon.Tip == 0
                            ? siparisToplami * (kupon.Deger / 100)
                            : kupon.Deger;
                        siparisToplami = Math.Max(0, siparisToplami - Math.Min(siparisToplami, Math.Max(0, indirim)));
                    }
                }

                decimal kargoUcreti = await _kargoHesaplama.HesaplaAsync(
                    sehir, siparisToplami, settings.UcretsizKargoLimiti);

                return Json(new
                {
                    success = true,
                    kargoUcreti = kargoUcreti,
                    ucretsizKargoLimiti = settings.UcretsizKargoLimiti,
                    sepetToplami = siparisToplami
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kargo hesaplama hatası");
                return Json(new { success = false, message = "Kargo hesaplanamadı." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("general")]
        public async Task<IActionResult> YukleKimlikFoto(IFormFile? dosya)
        {
            try
            {
                if (dosya == null || dosya.Length == 0)
                {
                    return Json(new { success = false, message = _localizer["Siparis_FileNotSelected"].Value });
                }

                var sonuc = await _dosyaServisi.KaydetAsync(dosya, "uploads/kimlikler");
                if (!sonuc.Success)
                {
                    return Json(new { success = false, message = sonuc.ErrorMessage });
                }

                return Json(new { success = true, url = sonuc.Url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kimlik fotografi yuklenirken hata olustu");
                return Json(new { success = false, message = _localizer["Siparis_FileUploadError"].Value });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("general")]
        public async Task<IActionResult> YukleRecete(IFormFile? dosya)
        {
            try
            {
                if (dosya == null || dosya.Length == 0)
                {
                    return Json(new { success = false, message = _localizer["Siparis_FileNotSelected"].Value });
                }

                var sonuc = await _dosyaServisi.KaydetAsync(dosya, "uploads/receteler", pdfDestegi: true);
                if (!sonuc.Success)
                {
                    return Json(new { success = false, message = sonuc.ErrorMessage });
                }

                return Json(new { success = true, url = sonuc.Url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reçete yüklenirken hata oluştu");
                return Json(new { success = false, message = _localizer["Siparis_FileUploadError"].Value });
            }
        }

        public IActionResult Beklemede(string siparisNo)
        {
            ViewBag.SiparisNo = siparisNo;
            return View();
        }

        public IActionResult Basarili(string siparisNo)
        {
            ViewBag.SiparisNo = siparisNo;
            return View();
        }

        public IActionResult Basarisiz()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> FaturaIndir(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                return NotFound(_localizer["Siparis_OrderNotFound"].Value);
            }

            // Güvenlik: Sadece kendi siparişinin faturasını indirebilir
            if (siparis.AppUserId != user.Id)
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(siparis.FaturaDosyaYolu))
            {
                return NotFound(_localizer["Siparis_InvoiceNotUploaded"].Value);
            }

            var filePath = Path.Combine(_env.WebRootPath, siparis.FaturaDosyaYolu.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(_localizer["Siparis_InvoiceFileNotFound"].Value);
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", siparis.FaturaDosyaAdi ?? $"fatura_{id}.pdf");
        }

        private async Task PrepareCheckoutViewDataAsync(string? userId, string sessionId, List<SepetItem>? sepetItems = null)
        {
            sepetItems ??= await _sepetService.GetSepetItemsAsync(userId, sessionId);
            
            var urunIds = sepetItems.Select(x => x.UrunId).ToList();
            var receteZorunluMu = await _context.Urunler
                .Where(u => urunIds.Contains(u.Id) && !u.SilindiMi)
                .AnyAsync(u => u.Kategori != null && u.Kategori.ReceteGerekliMi);
            ViewBag.ReceteZorunluMu = receteZorunluMu;

            var araToplam = sepetItems.Sum(x => x.Toplam);
            decimal indirimTutari = 0;
            var kuponKodu = HttpContext.Session.GetString("UygulananKupon");

            if (!string.IsNullOrEmpty(kuponKodu))
            {
                var kupon = await _context.Kuponlar.FirstOrDefaultAsync(x => x.Kod == kuponKodu && !x.SilindiMi);
                if (kupon != null &&
                    kupon.AktifMi &&
                    kupon.SonKullanmaTarihi > DateTime.UtcNow &&
                    (kupon.KullanimLimiti <= 0 || kupon.KullanilanMiktar < kupon.KullanimLimiti) &&
                    araToplam >= kupon.MinSepetTutari)
                {
                    indirimTutari = CalculateCouponDiscount(kupon, araToplam);
                }
                else
                {
                    HttpContext.Session.Remove("UygulananKupon");
                    kuponKodu = null;
                }
            }

            var settings = _siteSettingsService.GetSettings();

            decimal sepetToplamiIndirimli = araToplam - indirimTutari;
            decimal gosterilecekKargoBedeli = sepetToplamiIndirimli >= settings.UcretsizKargoLimiti
                ? 0
                : await _kargoHesaplama.HesaplaAsync("", sepetToplamiIndirimli, settings.UcretsizKargoLimiti);

            ViewBag.AraToplam = araToplam;
            ViewBag.IndirimTutari = indirimTutari;
            ViewBag.KuponKodu = kuponKodu;
            ViewBag.UcretsizKargoLimiti = settings.UcretsizKargoLimiti;
            ViewBag.GosterilecekKargoBedeli = gosterilecekKargoBedeli;
            ViewBag.ToplamTutar = Math.Max(0, sepetToplamiIndirimli + gosterilecekKargoBedeli);
            ViewBag.SepetItems = sepetItems;

            // Şehir listesini KargoBolgeSehirler'den çek
            ViewBag.Sehirler = await _context.KargoBolgeSehirler
                .IgnoreQueryFilters()
                .Where(s => !s.SilindiMi)
                .Select(s => s.SehirAdi)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            // Şehir listesini KargoBolgeSehirler'den çek
            ViewBag.Sehirler = await _context.KargoBolgeSehirler
                .IgnoreQueryFilters()
                .Where(s => !s.SilindiMi)
                .Select(s => s.SehirAdi)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            ViewBag.BankaHesaplari = await _context.BankaHesaplari
                .Where(x => !x.SilindiMi && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.BankaAdi)
                .ToListAsync();

            ViewBag.KapidaOdemeLimiti = settings.KapidaOdemeLimiti;
            ViewBag.KapidaOdemeAktifMi = settings.KapidaOdemeAktifMi && (sepetToplamiIndirimli <= settings.KapidaOdemeLimiti);
            ViewBag.KapidaOdemeHizmetBedeli = settings.KapidaOdemeHizmetBedeli;
            ViewBag.ToptanciMinSiparisTutari = settings.ToptanciMinSiparisTutari;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var tumAdresler = await _adresService.GetAllAsync();
                ViewBag.KayitliAdresler = tumAdresler.Where(x => x.AppUserId == userId).ToList();
            }
        }

        private static void NormalizeCheckoutInput(Siparis siparis)
        {
            siparis.MusteriAdSoyad = siparis.MusteriAdSoyad?.Trim() ?? string.Empty;
            siparis.Eposta = siparis.Eposta?.Trim() ?? string.Empty;
            siparis.Sehir = siparis.Sehir?.Trim() ?? string.Empty;
            siparis.Ilce = siparis.Ilce?.Trim() ?? string.Empty;
            siparis.AcikAdres = siparis.AcikAdres?.Trim() ?? string.Empty;

            var temizTel = new string((siparis.Telefon ?? string.Empty).Where(char.IsDigit).ToArray());
            if (!temizTel.StartsWith("0") && temizTel.Length == 10)
            {
                temizTel = "0" + temizTel;
            }

            siparis.Telefon = temizTel;
        }

        private bool ValidateCheckoutInput(Siparis siparis)
        {
            siparis.TeslimatTipi = siparis.TeslimatTipi == "MagazadanTeslim" ? "MagazadanTeslim" : "AdreseTeslim";

            if (string.IsNullOrWhiteSpace(siparis.MusteriAdSoyad))
            {
                ModelState.AddModelError(nameof(Siparis.MusteriAdSoyad), _localizer["Siparis_NameRequired"].Value);
            }

            if (string.IsNullOrWhiteSpace(siparis.Eposta) || !IsValidEmail(siparis.Eposta))
            {
                ModelState.AddModelError(nameof(Siparis.Eposta), _localizer["Siparis_EmailRequired"].Value);
            }

            if (siparis.Telefon.Length != 11 || !siparis.Telefon.StartsWith("0"))
            {
                ModelState.AddModelError(nameof(Siparis.Telefon), _localizer["Siparis_PhoneRequired"].Value);
            }

            if (siparis.MusteriAdSoyad.Length > 150 || siparis.Ilce.Length > 100 || siparis.AcikAdres.Length > 500)
            {
                ModelState.AddModelError(string.Empty, _localizer["Siparis_FormValidationError"].Value);
            }

            if (siparis.TeslimatTipi != "MagazadanTeslim")
            {
                if (string.IsNullOrWhiteSpace(siparis.Sehir))
                {
                    ModelState.AddModelError(nameof(Siparis.Sehir), _localizer["Siparis_CityRequired"].Value);
                }

                if (string.IsNullOrWhiteSpace(siparis.Ilce))
                {
                    ModelState.AddModelError(nameof(Siparis.Ilce), _localizer["Siparis_DistrictRequired"].Value);
                }

                if (string.IsNullOrWhiteSpace(siparis.AcikAdres) || siparis.AcikAdres.Length < 10)
                {
                    ModelState.AddModelError(nameof(Siparis.AcikAdres), _localizer["Siparis_AddressRequired"].Value);
                }
            }

            return ModelState.IsValid;
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

        private static bool IsSafeUploadedPath(string? path, string expectedFolder, bool allowPdf)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var normalizedPath = path.Trim().Replace('\\', '/');
            var normalizedFolder = "/" + expectedFolder.Trim('/').Replace('\\', '/') + "/";
            if (!normalizedPath.StartsWith(normalizedFolder, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.Contains("..", StringComparison.Ordinal) ||
                Uri.TryCreate(normalizedPath, UriKind.Absolute, out _))
            {
                return false;
            }

            var extension = Path.GetExtension(normalizedPath);
            var allowedExtensions = allowPdf
                ? new[] { ".jpg", ".jpeg", ".png", ".webp", ".pdf" }
                : new[] { ".jpg", ".jpeg", ".png", ".webp" };

            return allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        private static string TrimToMaxLength(string value, int maxLength)
        {
            var trimmed = value.Trim();
            return trimmed.Length > maxLength ? trimmed[..maxLength] : trimmed;
        }

        private async Task<string> GenerateUniqueOrderNumberAsync()
        {
            for (var attempt = 0; attempt < 5; attempt++)
            {
                var candidate = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + Random.Shared.Next(100, 999);
                if (!await _context.Siparisler.AnyAsync(x => x.SiparisNo == candidate))
                {
                    return candidate;
                }
            }

            return $"{DateTime.UtcNow:yyyyMMddHHmmssfff}{Guid.NewGuid():N}"[..24];
        }

        private async Task<int?> ResolveOrderVariantIdAsync(SepetItem item)
        {
            if (item.UrunSecenekId.HasValue)
            {
                return item.UrunSecenekId.Value;
            }

            var varsayilan = await _context.UrunSecenekleri
                .AsNoTracking()
                .Where(x =>
                    x.UrunId == item.UrunId &&
                    !x.SilindiMi &&
                    x.AktifMi &&
                    (!x.TukeninceGizle || x.StokAdedi > 0 || x.OnSipariseAcikMi))
                .OrderByDescending(x => x.VarsayilanMi)
                .ThenBy(x => x.Sira)
                .ThenBy(x => x.SatisFiyati)
                .FirstOrDefaultAsync();

            return varsayilan?.Id;
        }

        private static decimal CalculateCouponDiscount(Kupon kupon, decimal sepetTutari)
        {
            var discount = kupon.Tip == 0
                ? sepetTutari * (kupon.Deger / 100)
                : kupon.Deger;

            return Math.Round(Math.Min(sepetTutari, Math.Max(0, discount)), 2);
        }

        private async Task SendAdminOrderNotificationEmailAsync(Siparis siparis)
        {
            var settings = _siteSettingsService.GetSettings();
            if (!settings.YeniSiparisMailBildirimi)
            {
                return;
            }

            var recipientEmail = string.IsNullOrWhiteSpace(settings.BildirimAliciEmail)
                ? settings.Email
                : settings.BildirimAliciEmail;

            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return;
            }

            var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
            var currencySymbol = GetCurrencySymbol();
            var detailUrl = Url.Action("Detay", "Siparis", new { area = "Admin", id = siparis.Id }, Request.Scheme)
                ?? $"{Request.Scheme}://{Request.Host}/Admin/Siparis/Detay/{siparis.Id}";

            var customerName = System.Net.WebUtility.HtmlEncode(siparis.MusteriAdSoyad ?? string.Empty);
            var customerEmail = System.Net.WebUtility.HtmlEncode(siparis.Eposta ?? string.Empty);
            var orderNumber = System.Net.WebUtility.HtmlEncode(siparis.SiparisNo ?? string.Empty);
            var orderItemsHtml = await BuildOrderItemsTableRowsAsync(siparis.Id);

            var body = $@"
                <h3>{_localizer["Siparis_EmailNewOrderSubject"].Value}</h3>
                <p><strong>{_localizer["Profil_EmailSiparisNo"].Value}</strong> {orderNumber}</p>
                <p><strong>{_localizer["Profil_EmailMusteri"].Value}</strong> {customerName}</p>
                <p><strong>{_localizer["Profil_EmailEposta"].Value}</strong> {customerEmail}</p>
                <p><strong>&Ouml;deme Durumu:</strong> Beklemede</p>
                <p><strong>{_localizer["Siparis_EmailTutarLabel"].Value}:</strong> {siparis.ToplamTutar:N2} {currencySymbol}</p>
                <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:10px; background:#fff; margin:16px 0;'>
                    <thead>
                        <tr style='background:#fafaf8;'>
                            <th style='padding:10px; text-align:left; color:#313511;'>{_localizer["Siparis_EmailUrunLabel"].Value}</th>
                            <th style='padding:10px; text-align:center; color:#313511;'>{_localizer["Siparis_EmailAdetLabel"].Value}</th>
                            <th style='padding:10px; text-align:right; color:#313511;'>{_localizer["Siparis_EmailTutarLabel"].Value}</th>
                        </tr>
                    </thead>
                    <tbody>{orderItemsHtml}</tbody>
                </table>
                <p><a href=""{detailUrl}"">{_localizer["Siparis_EmailViewOrderAdmin"].Value}</a></p>";

            try
            {
                await _emailService.SendTemplateMailAsync(
                    recipientEmail,
                    $"{brandName} - {_localizer["Siparis_EmailNewOrderSubject"].Value} {siparis.SiparisNo}",
                    "Operasyon Ekibi",
                    body);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Yeni siparis mail bildirimi gonderilemedi. SiparisNo={SiparisNo}", siparis.SiparisNo);
            }
        }

        private async Task SendCustomerOrderConfirmationEmailAsync(Siparis siparis)
        {
            if (string.IsNullOrWhiteSpace(siparis.Eposta))
            {
                _logger.LogWarning("Musteri emaili eksik, siparis onay maili gonderilemedi. SiparisNo={SiparisNo}", siparis.SiparisNo);
                return;
            }

            var settings = _siteSettingsService.GetSettings();
            var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
            var currencySymbol = GetCurrencySymbol();
            var siteUrl = _siteSettingsService.BuildAbsoluteUrl(string.Empty);

            var detaylar = await _context.SiparisDetaylari
                .Include(x => x.Urun)
                .Include(x => x.UrunSecenek)
                .Where(x => x.SiparisId == siparis.Id && !x.SilindiMi)
                .ToListAsync();

            var urunListesi = new System.Text.StringBuilder();
            foreach (var item in detaylar)
            {
                var urunAdi = System.Net.WebUtility.HtmlEncode(item.Urun?.Baslik ?? _localizer["Siparis_EmailProduct"].Value);
                var detayMetni = System.Net.WebUtility.HtmlEncode(BuildOrderLineDetail(item));
                var adet = item.Adet;
                var fiyat = item.BirimFiyat * item.Adet;
                var notSatiri = !string.IsNullOrWhiteSpace(item.MusteriNotu)
                    ? $"<div style='margin-top:4px; font-size:12px; color:#b58735; font-style:italic;'>Not: {System.Net.WebUtility.HtmlEncode(item.MusteriNotu)}</div>"
                    : string.Empty;
                urunListesi.Append($@"
                    <tr>
                        <td style='padding:12px; border-bottom:1px solid #e5e2dc; color:#47473d;'>
                            <div>{urunAdi}</div>
                            {(string.IsNullOrWhiteSpace(detayMetni) ? string.Empty : $"<div style='margin-top:4px; font-size:12px; color:#6f6a5e;'>{detayMetni}</div>")}
                            {notSatiri}
                        </td>
                        <td style='padding:12px; border-bottom:1px solid #e5e2dc; text-align:center; color:#47473d;'>{adet}</td>
                        <td style='padding:12px; border-bottom:1px solid #e5e2dc; text-align:right; color:#313511; font-weight:600;'>{fiyat:N2} {currencySymbol}</td>
                    </tr>");
            }

            var musteriAdSoyad = System.Net.WebUtility.HtmlEncode(siparis.MusteriAdSoyad ?? _localizer["Siparis_EmailDearCustomer"].Value);
            var siparisNo = System.Net.WebUtility.HtmlEncode(siparis.SiparisNo ?? "");
            var toplamTutar = siparis.ToplamTutar;
            var teslimatAdresi = $"{siparis.AcikAdres}, {siparis.Ilce}/{siparis.Sehir}";
            var teslimatBilgi = System.Net.WebUtility.HtmlEncode(teslimatAdresi);

            var content = $@"
                <p style='margin-bottom:20px;'>{_localizer["Siparis_EmailGreeting"].Value} <strong>{musteriAdSoyad}</strong>,</p>
                <p style='margin-bottom:20px;'>{_localizer["Siparis_EmailOrderReceivedBody"].Value}</p>
                
                <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:12px; background:#fff; margin:20px 0;'>
                    <thead>
                        <tr style='background:#fafaf8;'>
                            <th style='padding:12px; border-bottom:2px solid #e5e2dc; text-align:left; color:#313511; font-size:13px;'>{_localizer["Siparis_EmailUrunLabel"].Value}</th>
                            <th style='padding:12px; border-bottom:2px solid #e5e2dc; text-align:center; color:#313511; font-size:13px;'>{_localizer["Siparis_EmailAdetLabel"].Value}</th>
                            <th style='padding:12px; border-bottom:2px solid #e5e2dc; text-align:right; color:#313511; font-size:13px;'>{_localizer["Siparis_EmailTutarLabel"].Value}</th>
                        </tr>
                    </thead>
                    <tbody>
                        {urunListesi}
                    </tbody>
                    <tfoot>
                        <tr style='background:#fafaf8;'>
                            <td colspan='2' style='padding:14px; border-top:2px solid #e5e2dc; text-align:right; color:#313511; font-weight:700;'>{_localizer["Siparis_EmailTotalLabel"].Value}</td>
                            <td style='padding:14px; border-top:2px solid #e5e2dc; text-align:right; color:#b58735; font-size:18px; font-weight:700;'>{toplamTutar:N2} {currencySymbol}</td>
                        </tr>
                    </tfoot>
                </table>
                
                <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:12px; background:#fffaf0; margin:20px 0;'>
                    <tr>
                        <td style='padding:16px; color:#47473d;'>
                            <strong style='color:#313511;'>{_localizer["Siparis_EmailSiparisNoLabel"].Value}</strong> <span style='font-size:16px; color:#b58735; font-weight:700;'>{siparisNo}</span>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:16px; border-top:1px solid #e5e2dc; color:#47473d;'>
                            <strong style='color:#313511;'>{_localizer["Siparis_EmailTeslimatAdresiLabel"].Value}</strong> {teslimatBilgi}
                        </td>
                    </tr>
                </table>
                
                <p style='margin-top:24px; color:#47473d; font-size:14px;'>
                    {_localizer["Siparis_EmailTrackOrderText"].Value} <a href='{siteUrl}/Profil/Siparislerim' style='color:#313511; text-decoration:underline;'>{_localizer["Siparis_EmailTrackOrder"].Value}</a>.
                </p>
                <p style='margin-top:16px; color:#47473d; font-size:14px;'>
                    {_localizer["Siparis_EmailContactText"].Value} <a href='{siteUrl}/Kurumsal/Iletisim' style='color:#313511; text-decoration:underline;'>{_localizer["Kurumsal_ContactUs"].Value}</a>.
                </p>
                <p style='margin-top:24px; color:#999; font-size:13px;'>
                    {_localizer["Siparis_EmailAutoFooter"].Value}
                </p>";

            try
            {
                await _emailService.SendTemplateMailAsync(
                    siparis.Eposta,
                    $"{brandName} - {_localizer["Siparis_EmailOrderReceivedSubject"].Value} ({siparisNo})",
                    musteriAdSoyad,
                    content,
                    "",
                    "");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Musteri siparis onay maili gonderilemedi. SiparisNo={SiparisNo}, Email={Email}", siparis.SiparisNo, siparis.Eposta);
            }
        }

        private async Task<string> BuildOrderItemsTableRowsAsync(int siparisId)
        {
            var currencySymbol = GetCurrencySymbol();
            var detaylar = await _context.SiparisDetaylari
                .Include(x => x.Urun)
                .Include(x => x.UrunSecenek)
                .Where(x => x.SiparisId == siparisId && !x.SilindiMi)
                .ToListAsync();

            var rows = new System.Text.StringBuilder();
            foreach (var item in detaylar)
            {
                var urunAdi = System.Net.WebUtility.HtmlEncode(item.Urun?.Baslik ?? _localizer["Siparis_EmailProduct"].Value);
                var detayMetni = System.Net.WebUtility.HtmlEncode(BuildOrderLineDetail(item));
                var detayHtml = string.IsNullOrWhiteSpace(detayMetni)
                    ? string.Empty
                    : $"<div style='margin-top:4px; font-size:12px; color:#6f6a5e;'>{detayMetni}</div>";

                rows.Append($@"
                    <tr>
                        <td style='padding:10px; border-top:1px solid #e5e2dc; color:#47473d;'>
                            <div>{urunAdi}</div>
                            {detayHtml}
                        </td>
                        <td style='padding:10px; border-top:1px solid #e5e2dc; text-align:center; color:#47473d;'>{item.Adet}</td>
                        <td style='padding:10px; border-top:1px solid #e5e2dc; text-align:right; color:#313511; font-weight:600;'>{(item.BirimFiyat * item.Adet):N2} {currencySymbol}</td>
                    </tr>");
            }

            return rows.ToString();
        }

        private string GetCurrencySymbol()
        {
            var settings = _siteSettingsService.GetSettings();
            return string.IsNullOrWhiteSpace(settings.ParaBirimi) ? "₪" : settings.ParaBirimi;
        }

        private string BuildOrderLineDetail(SiparisDetay item)
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
                details.Add(string.Format(_localizer["Siparis_EmailFrame"].Value, item.CerceveModeli));
            }

            return string.Join(" | ", details);
        }

        private async Task ClearOrderCartAsync(Siparis siparis)
        {
            Sepet? sepet = null;

            if (!string.IsNullOrWhiteSpace(siparis.AppUserId))
            {
                sepet = await _context.Sepetler
                    .Include(x => x.SepetItems.Where(i => !i.SilindiMi))
                    .FirstOrDefaultAsync(x => x.AppUserId == siparis.AppUserId && !x.SilindiMi);
            }
            else
            {
                var sessionId = HttpContext.Session.Id;
                sepet = await _context.Sepetler
                    .Include(x => x.SepetItems.Where(i => !i.SilindiMi))
                    .FirstOrDefaultAsync(x =>
                        x.SessionId == sessionId &&
                        string.IsNullOrEmpty(x.AppUserId) &&
                        !x.SilindiMi);
            }

            if (sepet == null)
            {
                return;
            }

            foreach (var item in sepet.SepetItems.Where(x => !x.SilindiMi))
            {
                item.SilindiMi = true;
            }

            sepet.SonGuncellemeTarihi = DateTime.UtcNow;
        }
    }
}
