using FilistinProje.Core.DTOs;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using FilistinProje.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;


namespace FilistinProje.Web.Controllers
{
    [Route("checkout")]
    public class SiparisController : Controller
    {
        private const string UploadTokenHashSessionKey = "CheckoutUploadTokenHash";
        private const string UploadTokenValueSessionKey = "CheckoutUploadTokenValue";
        private const string UploadTokenExpirySessionKey = "CheckoutUploadTokenExpiry";
        private const string UploadCountSessionKey = "CheckoutUploadCount";
        private const string UploadBytesSessionKey = "CheckoutUploadBytes";
        private const string UploadedReferencesSessionKey = "CheckoutUploadedReferences";
        private const int MaxCheckoutUploadCount = 4;
        private const long MaxCheckoutUploadBytes = 32L * 1024 * 1024;
        private readonly UserManager<AppUser> _userManager;
        private readonly IService<Adres> _adresService;
        private readonly KanvasDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISepetService _sepetService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly ILogger<SiparisController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IKargoHesaplamaServisi _kargoHesaplama;
        private readonly IDosyaServisi _dosyaServisi;
        private readonly IPurchaseOrderService _purchaseOrderService;

        public SiparisController(
            UserManager<AppUser> userManager,
            IService<Adres> adresService,
            KanvasDbContext context,
            IEmailService emailService,
            ISepetService sepetService,
            ISiteSettingsService siteSettingsService,
            ILogger<SiparisController> logger,
            IStringLocalizer<SharedResource> localizer,
            IKargoHesaplamaServisi kargoHesaplama,
            IDosyaServisi dosyaServisi,
            IPurchaseOrderService purchaseOrderService)
        {
            _userManager = userManager;
            _adresService = adresService;
            _context = context;
            _emailService = emailService;
            _sepetService = sepetService;
            _siteSettingsService = siteSettingsService;
            _logger = logger;
            _localizer = localizer;
            _kargoHesaplama = kargoHesaplama;
            _dosyaServisi = dosyaServisi;
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet("")]
        [HttpGet("/Siparis/Odeme")]
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

            var dto = new CheckoutRequestDto();

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var tumAdresler = await _adresService.GetAllAsync();
                    ViewBag.KayitliAdresler = tumAdresler.Where(x => x.AppUserId == user.Id).ToList();

                    dto.MusteriAdSoyad = user.AdSoyad;
                    dto.Eposta = user.Email ?? string.Empty;
                    dto.Telefon = user.PhoneNumber ?? string.Empty;
                    dto.Sehir = user.Sehir;
                }
            }

            return View(dto);
        }

        [HttpPost("")]
        [HttpPost("/Siparis/Odeme")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odeme(CheckoutRequestDto dto)
        {
            var userId = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
            var sessionId = HttpContext.Session.Id;

            var sepetItems = await _sepetService.GetSepetItemsAsync(userId, sessionId);
            if (sepetItems == null || !sepetItems.Any())
            {
                return RedirectToAction("Index", "Sepet");
            }

            // === B27: Bind attribute ile sadece güvenli alanlar alındı (DTO).
            if (!dto.SozlesmeOnaylandi)
            {
                ModelState.AddModelError(nameof(dto.SozlesmeOnaylandi), _localizer["Siparis_TermsRequired"].Value);
            }

            var urunIds = sepetItems.Select(x => x.UrunId).Distinct().ToList();
            var receteZorunluMu = await _context.Urunler
                .Where(u => urunIds.Contains(u.Id) && !u.SilindiMi)
                .AnyAsync(u => u.Kategori != null && u.Kategori.ReceteGerekliMi);

            if (receteZorunluMu && !IsSafeUploadedPath(dto.ReceteDosyaYolu, HassasBelgeKategorisi.Recete, allowPdf: true, sessionId))
            {
                ModelState.AddModelError(nameof(dto.ReceteDosyaYolu), _localizer["Siparis_PrescriptionRequired"].Value);
            }

            if (!string.IsNullOrWhiteSpace(dto.KimlikFotoYolu) && !IsSafeUploadedPath(dto.KimlikFotoYolu, HassasBelgeKategorisi.Kimlik, allowPdf: false, sessionId))
            {
                ModelState.AddModelError(nameof(dto.KimlikFotoYolu), _localizer["Siparis_FileUploadError"].Value);
            }

            NormalizeCheckoutInput(dto);

            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    dto.Eposta = currentUser.Email ?? dto.Eposta;
                }
            }

            if (!ValidateCheckoutInput(dto) || !ModelState.IsValid)
            {
                await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);
                ViewBag.FormHata = _localizer["Siparis_FormValidationError"].Value;
                return View(dto);
            }

            var temporaryReceteReference = dto.ReceteDosyaYolu;
            var temporaryKimlikReference = dto.KimlikFotoYolu;
            var recetePromoted = PromoteCheckoutDocument(
                temporaryReceteReference, sessionId, HassasBelgeKategorisi.Recete, out var receteReference);
            string? kimlikReference = null;
            var kimlikPromoted = recetePromoted && PromoteCheckoutDocument(
                temporaryKimlikReference, sessionId, HassasBelgeKategorisi.Kimlik, out kimlikReference);

            if (!recetePromoted || !kimlikPromoted)
            {
                RollBackPromotedDocument(receteReference, sessionId, HassasBelgeKategorisi.Recete);
                ModelState.AddModelError(string.Empty, _localizer["Siparis_FileUploadError"].Value);
                await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);
                ViewBag.FormHata = _localizer["Siparis_FileUploadError"].Value;
                return View(dto);
            }

            dto.ReceteDosyaYolu = receteReference;
            dto.KimlikFotoYolu = kimlikReference;

            var placeOrderResult = await _purchaseOrderService.PlaceOrderAsync(new PlaceOrderRequest
            {
                Checkout = dto,
                SepetItems = sepetItems,
                AppUserId = userId,
                SessionId = sessionId,
                IsWholesale = User.IsInRole("Wholesale"),
                KuponKodu = HttpContext.Session.GetString("UygulananKupon"),
                PaymentPendingMessage = _localizer["Siparis_PaymentPending"].Value,
                PayOnDeliveryPendingMessage = _localizer["Siparis_PayOnDeliveryPending"].Value,
                StorePickupCity = _localizer["StorePickupCity"].Value,
                StorePickupDistrict = _localizer["StorePickupDistrict"].Value,
                StorePickupAddress = _localizer["StorePickupAddress"].Value
            });

            if (!placeOrderResult.Succeeded)
            {
                RollBackPromotedDocument(receteReference, sessionId, HassasBelgeKategorisi.Recete);
                RollBackPromotedDocument(kimlikReference, sessionId, HassasBelgeKategorisi.Kimlik);
                dto.ReceteDosyaYolu = temporaryReceteReference;
                dto.KimlikFotoYolu = temporaryKimlikReference;

                if (placeOrderResult.Status == PlaceOrderStatus.InvalidCoupon)
                {
                    HttpContext.Session.Remove("UygulananKupon");
                }

                var errorMessage = BuildPlaceOrderErrorMessage(placeOrderResult);
                ModelState.AddModelError(GetPlaceOrderModelStateKey(placeOrderResult.Status), errorMessage);
                await PrepareCheckoutViewDataAsync(userId, sessionId, sepetItems);
                ViewBag.FormHata = errorMessage;
                return View(dto);
            }

            HttpContext.Session.Remove("UygulananKupon");
            ClearCheckoutUploadCapability();

            // Fiyat değişti ise kullanıcıya bildir (B3: sessizce farklı tahsil etmemek)
            if (placeOrderResult.Pricing?.FiyatDegistiMi == true)
            {
                TempData["Siparis_FiyatDegisti"] = string.Format(
                    _localizer["Siparis_PriceChangedNotice"].Value,
                    placeOrderResult.Pricing.FiyatDegisiklikleri.Count);
            }

            var siparis = await _context.Siparisler
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == placeOrderResult.SiparisId);

            if (siparis == null)
            {
                _logger.LogWarning(
                    "Siparis commit edildi ancak commit sonrasi email icin tekrar okunamadi. SiparisId={SiparisId}, SiparisNo={SiparisNo}",
                    placeOrderResult.SiparisId,
                    placeOrderResult.SiparisNo);
                return RedirectToAction(nameof(Beklemede), new { siparisNo = placeOrderResult.SiparisNo });
            }

            _logger.LogInformation(
                "Siparis odeme bekliyor durumunda olusturuldu. SiparisNo={SiparisNo}, Tutar={Tutar}",
                siparis.SiparisNo, siparis.ToplamTutar);

            await SendAdminOrderNotificationEmailAsync(siparis);
            await SendCustomerOrderConfirmationEmailAsync(siparis);

            return RedirectToAction(nameof(Beklemede), new { siparisNo = siparis.SiparisNo });
        }

        [HttpGet("shipping-cost")] public async Task<IActionResult> KargoHesapla(string sehir)
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
                return Json(new { success = false, message = "تعذّر حساب تكلفة الشحن." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("checkout-upload")]
        public async Task<IActionResult> YukleKimlikFoto(IFormFile? dosya)
        {
            try
            {
                if (dosya == null || dosya.Length == 0)
                {
                    return Json(new { success = false, message = _localizer["Siparis_FileNotSelected"].Value });
                }

                if (!await ValidateCheckoutUploadCapabilityAsync(dosya.Length))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { success = false, message = _localizer["Siparis_FileUploadError"].Value });
                }

                var sonuc = await _dosyaServisi.GeciciHassasBelgeKaydetAsync(
                    dosya,
                    HassasBelgeKategorisi.Kimlik,
                    GetCheckoutStorageKey(HttpContext.Session.Id));
                if (!sonuc.Success)
                {
                    return Json(new { success = false, message = sonuc.ErrorMessage });
                }

                RegisterSuccessfulCheckoutUpload(sonuc.Url!, dosya.Length);
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
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("checkout-upload")]
        public async Task<IActionResult> YukleRecete(IFormFile? dosya)
        {
            try
            {
                if (dosya == null || dosya.Length == 0)
                {
                    return Json(new { success = false, message = _localizer["Siparis_FileNotSelected"].Value });
                }

                if (!await ValidateCheckoutUploadCapabilityAsync(dosya.Length))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { success = false, message = _localizer["Siparis_FileUploadError"].Value });
                }

                var sonuc = await _dosyaServisi.GeciciHassasBelgeKaydetAsync(
                    dosya,
                    HassasBelgeKategorisi.Recete,
                    GetCheckoutStorageKey(HttpContext.Session.Id));
                if (!sonuc.Success)
                {
                    return Json(new { success = false, message = sonuc.ErrorMessage });
                }

                RegisterSuccessfulCheckoutUpload(sonuc.Url!, dosya.Length);
                return Json(new { success = true, url = sonuc.Url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reçete yüklenirken hata oluştu");
                return Json(new { success = false, message = _localizer["Siparis_FileUploadError"].Value });
            }
        }

        [HttpGet("pending")]
        [HttpGet("/Siparis/Beklemede")]
        public IActionResult Beklemede(string siparisNo)
        {
            ViewBag.SiparisNo = siparisNo;
            return View();
        }

        [HttpGet("success")]
        [HttpGet("/Siparis/Basarili")]
        public IActionResult Basarili(string siparisNo)
        {
            ViewBag.SiparisNo = siparisNo;
            return View();
        }

        [HttpGet("failed")]
        [HttpGet("/Siparis/Basarisiz")]
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

            return RedirectToAction("Fatura", "Belge", new { siparisId = id, indir = true });
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

            var sehirKayitlari = await _context.KargoBolgeSehirler
                .IgnoreQueryFilters()
                .Where(s => !s.SilindiMi)
                .Select(s => new { s.SehirAdi, s.SehirAdiEn, s.SehirAdiAr })
                .ToListAsync();
            var dil = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            ViewBag.Sehirler = sehirKayitlari
                .Select(s => dil == "ar"
                    ? (string.IsNullOrWhiteSpace(s.SehirAdiAr) ? s.SehirAdiEn ?? s.SehirAdi : s.SehirAdiAr)
                    : (string.IsNullOrWhiteSpace(s.SehirAdiEn) ? s.SehirAdi : s.SehirAdiEn))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.CurrentCulture)
                .ToList();

            ViewBag.BankaHesaplari = await _context.BankaHesaplari
                .Where(x => !x.SilindiMi && x.AktifMi)
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.BankaAdi)
                .ToListAsync();

            ViewBag.BankaHavaleAktifMi = ViewBag.BankaHesaplari is List<BankaHesap> hesaplar && hesaplar.Any();

            ViewBag.KapidaOdemeLimiti = settings.KapidaOdemeLimiti;
            ViewBag.KapidaOdemeAktifMi = settings.KapidaOdemeAktifMi && (sepetToplamiIndirimli <= settings.KapidaOdemeLimiti);
            ViewBag.KapidaOdemeHizmetBedeli = settings.KapidaOdemeHizmetBedeli;
            ViewBag.ToptanciMinSiparisTutari = settings.ToptanciMinSiparisTutari;
            ViewBag.CheckoutUploadToken = IssueCheckoutUploadCapability();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var tumAdresler = await _adresService.GetAllAsync();
                ViewBag.KayitliAdresler = tumAdresler.Where(x => x.AppUserId == userId).ToList();
            }
        }

        private static void NormalizeCheckoutInput(CheckoutRequestDto dto)
        {
            dto.MusteriAdSoyad = dto.MusteriAdSoyad?.Trim() ?? string.Empty;
            dto.Eposta = dto.Eposta?.Trim() ?? string.Empty;
            dto.Sehir = dto.Sehir?.Trim() ?? string.Empty;
            dto.Ilce = dto.Ilce?.Trim() ?? string.Empty;
            dto.AcikAdres = dto.AcikAdres?.Trim() ?? string.Empty;
            dto.Telefon = dto.Telefon?.Trim() ?? string.Empty;
            dto.Aciklama = dto.Aciklama?.Trim();
            dto.ReceteDosyaYolu = dto.ReceteDosyaYolu?.Trim();
            dto.KimlikFotoYolu = dto.KimlikFotoYolu?.Trim();
        }

        private bool ValidateCheckoutInput(CheckoutRequestDto dto)
        {
            dto.TeslimatTipi = dto.TeslimatTipi == "MagazadanTeslim" ? "MagazadanTeslim" : "AdreseTeslim";

            if (string.IsNullOrWhiteSpace(dto.MusteriAdSoyad))
            {
                ModelState.AddModelError(nameof(dto.MusteriAdSoyad), _localizer["Siparis_NameRequired"].Value);
            }

            if (string.IsNullOrWhiteSpace(dto.Eposta) || !IsValidEmail(dto.Eposta))
            {
                ModelState.AddModelError(nameof(dto.Eposta), _localizer["Siparis_EmailRequired"].Value);
            }

            if (PhoneNumberNormalizer.TryNormalize(dto.Telefon, out var normalizedPhone))
            {
                dto.Telefon = normalizedPhone;
            }
            else
            {
                ModelState.AddModelError(nameof(dto.Telefon), _localizer["Siparis_PhoneRequired"].Value);
            }

            if (dto.MusteriAdSoyad.Length > 150 || (dto.Ilce?.Length ?? 0) > 100 || (dto.AcikAdres?.Length ?? 0) > 500)
            {
                ModelState.AddModelError(string.Empty, _localizer["Siparis_FormValidationError"].Value);
            }

            if (dto.TeslimatTipi != "MagazadanTeslim")
            {
                if (string.IsNullOrWhiteSpace(dto.Sehir))
                {
                    ModelState.AddModelError(nameof(dto.Sehir), _localizer["Siparis_CityRequired"].Value);
                }

                if (string.IsNullOrWhiteSpace(dto.Ilce))
                {
                    ModelState.AddModelError(nameof(dto.Ilce), _localizer["Siparis_DistrictRequired"].Value);
                }

                if (string.IsNullOrWhiteSpace(dto.AcikAdres) || (dto.AcikAdres?.Length ?? 0) < 10)
                {
                    ModelState.AddModelError(nameof(dto.AcikAdres), _localizer["Siparis_AddressRequired"].Value);
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

        private bool IsSafeUploadedPath(
            string? path,
            HassasBelgeKategorisi expectedCategory,
            bool allowPdf,
            string sessionId)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (DosyaServisi.TryParseTemporaryReference(path, out var kategori, out var storageKey, out var belgeAdi))
            {
                if (kategori != expectedCategory ||
                    !string.Equals(storageKey, GetCheckoutStorageKey(sessionId), StringComparison.Ordinal) ||
                    !GetUploadedReferences().Contains(path, StringComparer.Ordinal))
                {
                    return false;
                }

                var privateExtension = Path.GetExtension(belgeAdi);
                var allowedPrivateExtensions = allowPdf
                    ? new[] { ".jpg", ".jpeg", ".png", ".webp", ".pdf" }
                    : new[] { ".jpg", ".jpeg", ".png", ".webp" };

                return allowedPrivateExtensions.Contains(privateExtension, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        private string IssueCheckoutUploadCapability()
        {
            var existingToken = HttpContext.Session.GetString(UploadTokenValueSessionKey);
            var expiryRaw = HttpContext.Session.GetString(UploadTokenExpirySessionKey);
            if (!string.IsNullOrWhiteSpace(existingToken) &&
                long.TryParse(expiryRaw, out var existingExpiry) &&
                DateTimeOffset.UtcNow.ToUnixTimeSeconds() <= existingExpiry)
            {
                return existingToken;
            }

            var token = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
            HttpContext.Session.SetString(UploadTokenValueSessionKey, token);
            HttpContext.Session.SetString(UploadTokenHashSessionKey, Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))));
            HttpContext.Session.SetString(UploadTokenExpirySessionKey, DateTimeOffset.UtcNow.AddMinutes(20).ToUnixTimeSeconds().ToString());
            HttpContext.Session.SetInt32(UploadCountSessionKey, 0);
            HttpContext.Session.SetString(UploadBytesSessionKey, "0");
            HttpContext.Session.SetString(UploadedReferencesSessionKey, "[]");
            return token;
        }

        private async Task<bool> ValidateCheckoutUploadCapabilityAsync(long incomingBytes)
        {
            var token = Request.Headers["X-Checkout-Upload-Token"].ToString();
            var storedHashHex = HttpContext.Session.GetString(UploadTokenHashSessionKey);
            var expiryRaw = HttpContext.Session.GetString(UploadTokenExpirySessionKey);
            if (string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(storedHashHex) ||
                !long.TryParse(expiryRaw, out var expiry) ||
                DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiry)
            {
                return false;
            }

            byte[] storedHash;
            try
            {
                storedHash = Convert.FromHexString(storedHashHex);
            }
            catch (FormatException)
            {
                return false;
            }

            if (!CryptographicOperations.FixedTimeEquals(
                    storedHash,
                    SHA256.HashData(Encoding.UTF8.GetBytes(token))))
            {
                return false;
            }

            var count = HttpContext.Session.GetInt32(UploadCountSessionKey) ?? 0;
            _ = long.TryParse(HttpContext.Session.GetString(UploadBytesSessionKey), out var usedBytes);
            if (count >= MaxCheckoutUploadCount ||
                incomingBytes <= 0 ||
                usedBytes + incomingBytes > MaxCheckoutUploadBytes)
            {
                return false;
            }

            var userId = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
            var cart = await _sepetService.GetSepetItemsAsync(userId, HttpContext.Session.Id);
            return cart.Count > 0;
        }

        private void RegisterSuccessfulCheckoutUpload(string reference, long bytes)
        {
            var references = GetUploadedReferences();
            references.Add(reference);
            HttpContext.Session.SetString(
                UploadedReferencesSessionKey,
                JsonSerializer.Serialize(references.Distinct(StringComparer.Ordinal)));
            HttpContext.Session.SetInt32(
                UploadCountSessionKey,
                (HttpContext.Session.GetInt32(UploadCountSessionKey) ?? 0) + 1);
            _ = long.TryParse(HttpContext.Session.GetString(UploadBytesSessionKey), out var usedBytes);
            HttpContext.Session.SetString(UploadBytesSessionKey, (usedBytes + bytes).ToString());
        }

        private List<string> GetUploadedReferences()
        {
            var raw = HttpContext.Session.GetString(UploadedReferencesSessionKey);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return new List<string>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<string>>(raw) ?? new List<string>();
            }
            catch (JsonException)
            {
                return new List<string>();
            }
        }

        private bool PromoteCheckoutDocument(
            string? temporaryReference,
            string sessionId,
            HassasBelgeKategorisi category,
            out string? privateReference)
        {
            privateReference = null;
            if (string.IsNullOrWhiteSpace(temporaryReference))
            {
                return true;
            }

            return _dosyaServisi.GeciciBelgeyiKaliciYap(
                temporaryReference,
                GetCheckoutStorageKey(sessionId),
                category,
                out privateReference);
        }

        private void RollBackPromotedDocument(
            string? privateReference,
            string sessionId,
            HassasBelgeKategorisi category)
        {
            if (string.IsNullOrWhiteSpace(privateReference))
            {
                return;
            }

            if (!_dosyaServisi.KaliciBelgeyiGeciciyeGeriAl(
                    privateReference,
                    GetCheckoutStorageKey(sessionId),
                    category,
                    out _))
            {
                _logger.LogError(
                    "Checkout belge terfisi geri alinamadi. Kategori={Category}",
                    category);
            }
        }

        private void ClearCheckoutUploadCapability()
        {
            HttpContext.Session.Remove(UploadTokenValueSessionKey);
            HttpContext.Session.Remove(UploadTokenHashSessionKey);
            HttpContext.Session.Remove(UploadTokenExpirySessionKey);
            HttpContext.Session.Remove(UploadCountSessionKey);
            HttpContext.Session.Remove(UploadBytesSessionKey);
            HttpContext.Session.Remove(UploadedReferencesSessionKey);
        }

        private static string GetCheckoutStorageKey(string sessionId)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(sessionId)))[..32];
        }

        private string BuildPlaceOrderErrorMessage(PlaceOrderResult result)
        {
            if (result.Status == PlaceOrderStatus.StockShortage && result.StockShortages.Count > 0)
            {
                var ilk = result.StockShortages[0];
                return string.Format(
                    _localizer["Siparis_StockShortage"].Value,
                    ilk.UrunBaslik,
                    ilk.IstenenAdet,
                    ilk.MevcutStok);
            }

            var key = string.IsNullOrWhiteSpace(result.MessageKey)
                ? "Siparis_OrderFailed"
                : result.MessageKey;

            var localized = _localizer[key].Value;
            return result.MessageArgs.Length > 0
                ? string.Format(localized, result.MessageArgs)
                : localized;
        }

        private static string GetPlaceOrderModelStateKey(PlaceOrderStatus status)
        {
            return status switch
            {
                PlaceOrderStatus.StockShortage => "stock",
                PlaceOrderStatus.NoActiveBankAccount => nameof(CheckoutRequestDto.OdemeYontemi),
                PlaceOrderStatus.CodLimitExceeded => nameof(CheckoutRequestDto.OdemeYontemi),
                PlaceOrderStatus.WholesaleMinimumNotMet => "SepetToplami",
                PlaceOrderStatus.ShippingNotConfigured => nameof(CheckoutRequestDto.Sehir),
                PlaceOrderStatus.InvalidCoupon => "KuponKodu",
                _ => string.Empty
            };
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
            return string.IsNullOrWhiteSpace(settings.ParaBirimi) ? "â‚ª" : settings.ParaBirimi;
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
    }
}


