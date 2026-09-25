using System.Globalization;
using System.Security.Claims;
using System.Text;
using FilistinProje.Core.Enums;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Models;
using FilistinProje.Web.Resources;
using FilistinProje.Web.Security;
using FilistinProje.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Controllers
{
    [Route("account")]
    public class HesapController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly KanvasDbContext _context;
        private readonly IDosyaServisi _dosyaServisi;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IAdminSessionStateService _adminSessionStateService;
        private readonly IAdminSecurityAuditService _adminSecurityAuditService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<HesapController> _logger;

        public HesapController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            KanvasDbContext context,
            IDosyaServisi dosyaServisi,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            IAdminSessionStateService adminSessionStateService,
            IAdminSecurityAuditService adminSecurityAuditService,
            IStringLocalizer<SharedResource> localizer,
            ILogger<HesapController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dosyaServisi = dosyaServisi;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
            _adminSessionStateService = adminSessionStateService;
            _adminSecurityAuditService = adminSecurityAuditService;
            _localizer = localizer;
            _logger = logger;
        }

        [HttpGet("register")]
        [HttpGet("/Hesap/KayitOl")]
        [HttpGet("/account/KayitOl")]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpPost("register")]
        [HttpPost("/Hesap/KayitOl")]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> KayitOl(KayitViewModel model)
        {
            if (!PhoneNumberNormalizer.TryNormalize(model.Telefon, out var normalizedPhone))
            {
                ModelState.AddModelError(nameof(model.Telefon), _localizer["Siparis_PhoneRequired"].Value);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Telefon = normalizedPhone;

            var kimlikFotoSonuc = await _dosyaServisi.KaydetAsync(model.KimlikFoto!, "uploads/kimlikler");
            if (!kimlikFotoSonuc.Success)
            {
                ModelState.AddModelError(nameof(model.KimlikFoto), kimlikFotoSonuc.ErrorMessage ?? _localizer["Siparis_FileUploadError"].Value);
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Eposta,
                Email = model.Eposta,
                AdSoyad = model.AdSoyad,
                KimlikNo = model.KimlikNo,
                DogumTarihi = model.DogumTarihi.HasValue ? DateTime.SpecifyKind(model.DogumTarihi.Value, DateTimeKind.Utc) : null,
                PhoneNumber = model.Telefon,
                Adres = model.Adres,
                Sehir = model.Sehir,
                KimlikFotografYolu = kimlikFotoSonuc.Url
            };

            var result = await _userManager.CreateAsync(user, model.Sifre);
            if (!result.Succeeded)
            {
                _dosyaServisi.Sil(kimlikFotoSonuc.Url ?? string.Empty);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, AdminSecurityRoles.Uye);

            if (model.ToptanciMi)
            {
                user.WholesaleStatus = WholesaleStatus.Pending;
                user.BasvuruTarihi = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            var settings = _siteSettingsService.GetSettings();
            var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("EpostaDogrula", "Hesap", new { userId = user.Id, token }, Request.Scheme) ?? string.Empty;

            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var (subject, content, buttonText) = culture switch
            {
                "en" => (
                    "Verify your email address",
                    $"Your {brandName} account has been created. Please verify your email address to use your account securely.",
                    "Verify My Account"
                ),
                "ar" => (
                    "ØªØ­Ù‚Ù‚ Ù…Ù† Ø¹Ù†ÙˆØ§Ù† Ø¨Ø±ÙŠØ¯Ùƒ Ø§Ù„Ø¥Ù„ÙƒØªØ±ÙˆÙ†ÙŠ",
                    $"ØªÙ… Ø¥Ù†Ø´Ø§Ø¡ Ø­Ø³Ø§Ø¨Ùƒ ÙÙŠ {brandName}. ÙŠØ±Ø¬Ù‰ Ø§Ù„ØªØ­Ù‚Ù‚ Ù…Ù† Ø¹Ù†ÙˆØ§Ù† Ø¨Ø±ÙŠØ¯Ùƒ Ø§Ù„Ø¥Ù„ÙƒØªØ±ÙˆÙ†ÙŠ Ù„Ø§Ø³ØªØ®Ø¯Ø§Ù… Ø­Ø³Ø§Ø¨Ùƒ Ø¨Ø£Ù…Ø§Ù†.",
                    "ØªØ­Ù‚Ù‚ Ù…Ù† Ø­Ø³Ø§Ø¨ÙŠ"
                ),
                _ => (
                    "تحقق من عنوان بريدك الإلكتروني",
                    $"تم إنشاء حسابك في {brandName}. يرجى التحقق من عنوان بريدك الإلكتروني لاستخدام حسابك بأمان.",
                    "تحقق من حسابي"
                )
            };

            try
            {
                await _emailService.SendTemplateMailAsync(
                    user.Email ?? string.Empty,
                    subject,
                    user.AdSoyad,
                    content,
                    confirmationLink,
                    buttonText,
                    culture);

                TempData["Basari"] = _localizer["Hesap_AccountCreatedEmailSent"].Value;
            }
            catch (Exception ex)
            {
                TempData["Hata"] = _localizer["Hesap_AccountCreatedEmailFailed"].Value + ex.Message;
            }

            return RedirectToAction("EpostaOnayBilgilendirme");
        }

        [HttpGet("email-verification-info")] public IActionResult EpostaOnayBilgilendirme()
        {
            return View();
        }

        [HttpGet("verify-email")] public async Task<IActionResult> EpostaDogrula(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(_localizer["Hesap_UserNotFound"].Value);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("DogrulamaBasarili");
            }

            return Content(_localizer["Hesap_EmailVerificationError"].Value);
        }

        [HttpGet("login")]
        [HttpGet("/Hesap/GirisYap")]
        [HttpGet("/account/GirisYap")]
        public IActionResult GirisYap(string? returnUrl = null, string? remoteError = null)
        {
            if (!string.IsNullOrWhiteSpace(remoteError))
            {
                ViewBag.Hata = _localizer["Hesap_GoogleAuthFailed"].Value;
                TempData["Hata"] = ViewBag.Hata;
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("login")]
        [HttpPost("/Hesap/GirisYap")]
        [HttpPost("/account/GirisYap")]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> GirisYap(string eposta, string sifre, string? returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(eposta);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ViewBag.Hata = _localizer["Hesap_PleaseVerifyEmail"].Value;
                    TempData["Hata"] = ViewBag.Hata;
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, sifre, isPersistent: true, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    try
                    {
                        var sessionId = HttpContext.Session.Id;
                        var sepetService = HttpContext.RequestServices.GetRequiredService<ISepetService>();
                        var mergeResult = await sepetService.MergeSepetlerDetailedAsync(sessionId, user.Id);
                        if (!mergeResult.Basarili && !string.IsNullOrWhiteSpace(mergeResult.HataMesaji))
                        {
                            TempData["SepetUyari"] = mergeResult.HataMesaji;
                        }

                        HttpContext.Session.Clear();
                        Response.Cookies.Delete(".AspNetCore.Session");
                    }
                    catch
                    {
                    }

                    HttpContext.Session.Remove(AdminSessionConstants.SessionKey);

                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any(AdminSecurityRoles.IsAdminRole))
                    {
                        var roleLabel = AdminSecurityRoles.GetPrimaryRoleLabel(roles);
                        var sessionState = await _adminSessionStateService.RegisterSessionAsync(
                            user,
                            roleLabel,
                            HttpContext.Connection.RemoteIpAddress?.ToString());

                        HttpContext.Session.SetString(AdminSessionConstants.SessionKey, sessionState.CurrentSessionToken);

                        await _adminSecurityAuditService.LogAsync(
                            HttpContext,
                            "admin_login_success",
                            "Admin hesabi basariyla giris yapti.",
                            "/Admin",
                            user.Id,
                            user.UserName ?? user.Email);
                    }

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Hata = _localizer["Hesap_InvalidEmailOrPassword"].Value;
            TempData["Hata"] = ViewBag.Hata;
            return View();
        }

        [HttpGet("external-login")]
        [HttpPost("external-login")]
        [HttpGet("/Hesap/ExternalLogin")]
        [HttpPost("/Hesap/ExternalLogin")]
        public async Task<IActionResult> ExternalLogin(string provider = "Google", string? returnUrl = null)
        {
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            if (!schemes.Any(s => string.Equals(s.Name, provider, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("External login provider '{Provider}' is not configured or registered.", provider);
                TempData["Hata"] = _localizer["Hesap_GoogleNotConfigured"].Value;
                return RedirectToAction(nameof(GirisYap), new { returnUrl });
            }

            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Hesap", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        [HttpGet("/Hesap/ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            try
            {
                if (remoteError != null)
                {
                    _logger.LogWarning("External login error from remote provider: {RemoteError}", remoteError);
                    TempData["Hata"] = remoteError;
                    return RedirectToAction(nameof(GirisYap), new { returnUrl });
                }

                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    _logger.LogWarning("GetExternalLoginInfoAsync returned null. Check correlation cookie or HTTPS reverse proxy headers.");
                    TempData["Hata"] = _localizer["Hesap_GoogleAuthFailed"].Value;
                    return RedirectToAction(nameof(GirisYap), new { returnUrl });
                }

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrWhiteSpace(email))
                {
                    TempData["Hata"] = _localizer["Hesap_GoogleEmailMissing"].Value;
                    return RedirectToAction(nameof(GirisYap), new { returnUrl });
                }

                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
                AppUser? user = null;
                bool isNewUser = false;

                if (signInResult.Succeeded)
                {
                    user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey)
                           ?? await _userManager.FindByEmailAsync(email);
                    isNewUser = false;
                }
                else
                {
                    user = await _userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        // Existing user found: sign in directly without completing profile
                        isNewUser = false;
                        await _userManager.AddLoginAsync(user, info);
                        if (!user.EmailConfirmed)
                        {
                            user.EmailConfirmed = true;
                            await _userManager.UpdateAsync(user);
                        }
                        await _signInManager.SignInAsync(user, isPersistent: true);
                    }
                    else
                    {
                        // New user: create account and redirect to profile completion
                        isNewUser = true;
                        var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email.Split('@')[0];
                        user = new AppUser
                        {
                            UserName = email,
                            Email = email,
                            AdSoyad = name,
                            EmailConfirmed = true
                        };

                        var createResult = await _userManager.CreateAsync(user);
                        if (!createResult.Succeeded)
                        {
                            TempData["Hata"] = string.Join(" ", createResult.Errors.Select(e => e.Description));
                            return RedirectToAction(nameof(GirisYap), new { returnUrl });
                        }

                        await _userManager.AddToRoleAsync(user, AdminSecurityRoles.Uye);
                        await _userManager.AddLoginAsync(user, info);
                        await _signInManager.SignInAsync(user, isPersistent: true);
                    }
                }

                if (user != null)
                {
                    try
                    {
                        var sessionId = HttpContext.Session.Id;
                        var sepetService = HttpContext.RequestServices.GetRequiredService<ISepetService>();
                        await sepetService.MergeSepetlerDetailedAsync(sessionId, user.Id);
                    }
                    catch {}

                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any(AdminSecurityRoles.IsAdminRole))
                    {
                        var roleLabel = AdminSecurityRoles.GetPrimaryRoleLabel(roles);
                        var sessionState = await _adminSessionStateService.RegisterSessionAsync(
                            user,
                            roleLabel,
                            HttpContext.Connection.RemoteIpAddress?.ToString());

                        HttpContext.Session.SetString(AdminSessionConstants.SessionKey, sessionState.CurrentSessionToken);

                        await _adminSecurityAuditService.LogAsync(
                            HttpContext,
                            "admin_login_success",
                            "Admin account logged in successfully with Google.",
                            "/Admin",
                            user.Id,
                            user.UserName ?? user.Email);
                    }

                    // Only new users need profile completion
                    if (isNewUser)
                    {
                        return RedirectToAction(nameof(ProfilTamamla), new { returnUrl });
                    }
                }

                // Mevcut hesap ile giriş yapıldıysa doğrudan hedef sayfaya gidilir (kayıt tamamlama ekranı açılmaz)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ExternalLoginCallback");
                TempData["Hata"] = _localizer["Hesap_GoogleAuthFailed"].Value;
                return RedirectToAction(nameof(GirisYap), new { returnUrl });
            }
        }

        [HttpGet("complete-profile")]
        [HttpGet("/Hesap/ProfilTamamla")]
        [HttpGet("/account/ProfilTamamla")]
        public async Task<IActionResult> ProfilTamamla(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction(nameof(GirisYap), new { returnUrl });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(GirisYap));
            }

            bool isProfileComplete = !string.IsNullOrWhiteSpace(user.KimlikNo)
                                  && !string.IsNullOrWhiteSpace(user.PhoneNumber)
                                  && !string.IsNullOrWhiteSpace(user.Sehir)
                                  && !string.IsNullOrWhiteSpace(user.Adres)
                                  && !string.IsNullOrWhiteSpace(user.KimlikFotografYolu);

            if (isProfileComplete)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Profil");
            }

            var model = new ProfilTamamlaViewModel
            {
                AdSoyad = user.AdSoyad ?? string.Empty,
                Eposta = user.Email ?? string.Empty,
                KimlikNo = user.KimlikNo ?? string.Empty,
                DogumTarihi = user.DogumTarihi,
                Telefon = user.PhoneNumber ?? string.Empty,
                Sehir = user.Sehir ?? string.Empty,
                Adres = user.Adres ?? string.Empty,
                MevcutKimlikFotoUrl = user.KimlikFotografYolu,
                ReturnUrl = returnUrl,
                ToptanciMi = user.BasvuruTarihi.HasValue || user.WholesaleStatus == WholesaleStatus.Approved
            };

            return View(model);
        }

        [HttpPost("complete-profile")]
        [HttpPost("/Hesap/ProfilTamamla")]
        [HttpPost("/account/ProfilTamamla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilTamamla(ProfilTamamlaViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction(nameof(GirisYap));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(GirisYap));
            }

            if (!PhoneNumberNormalizer.TryNormalize(model.Telefon, out var normalizedPhone))
            {
                ModelState.AddModelError(nameof(model.Telefon), _localizer["Siparis_PhoneRequired"].Value);
            }
            else
            {
                model.Telefon = normalizedPhone;
            }

            if (string.IsNullOrWhiteSpace(user.KimlikFotografYolu) && model.KimlikFoto == null)
            {
                ModelState.AddModelError(nameof(model.KimlikFoto), _localizer["Validation_IdentityPhotoRequired"].Value);
            }

            if (!ModelState.IsValid)
            {
                model.AdSoyad = user.AdSoyad ?? string.Empty;
                model.Eposta = user.Email ?? string.Empty;
                model.MevcutKimlikFotoUrl = user.KimlikFotografYolu;
                return View(model);
            }

            if (model.KimlikFoto != null && model.KimlikFoto.Length > 0)
            {
                var fotoSonuc = await _dosyaServisi.KaydetAsync(model.KimlikFoto, "uploads/kimlikler");
                if (!fotoSonuc.Success)
                {
                    ModelState.AddModelError(nameof(model.KimlikFoto), fotoSonuc.ErrorMessage ?? _localizer["Siparis_FileUploadError"].Value);
                    model.AdSoyad = user.AdSoyad ?? string.Empty;
                    model.Eposta = user.Email ?? string.Empty;
                    model.MevcutKimlikFotoUrl = user.KimlikFotografYolu;
                    return View(model);
                }
                user.KimlikFotografYolu = fotoSonuc.Url;
            }

            if (!string.IsNullOrWhiteSpace(model.AdSoyad))
            {
                user.AdSoyad = model.AdSoyad.Trim();
            }
            user.KimlikNo = model.KimlikNo?.Trim() ?? string.Empty;
            user.DogumTarihi = model.DogumTarihi.HasValue ? DateTime.SpecifyKind(model.DogumTarihi.Value, DateTimeKind.Utc) : null;
            user.PhoneNumber = model.Telefon;
            user.Sehir = model.Sehir?.Trim() ?? string.Empty;
            user.Adres = model.Adres?.Trim() ?? string.Empty;

            // Toptancı başvurusu: kutucuk işaretlendiyse ve daha önce başvurulmadıysa
            if (model.ToptanciMi && !user.BasvuruTarihi.HasValue && user.WholesaleStatus != WholesaleStatus.Approved)
            {
                user.WholesaleStatus = WholesaleStatus.Pending;
                user.BasvuruTarihi = DateTime.UtcNow;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.AdSoyad = user.AdSoyad ?? string.Empty;
                model.Eposta = user.Email ?? string.Empty;
                model.MevcutKimlikFotoUrl = user.KimlikFotografYolu;
                return View(model);
            }

            TempData["Basari"] = _localizer["CompleteProfile_Success"].Value;

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Profil");
        }

        [HttpPost("logout")]
        [HttpPost("/Hesap/CikisYap")]
        [ValidateAntiForgeryToken]
        [HttpGet("/Hesap/CikisYap")]
        public async Task<IActionResult> CikisYap()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any(AdminSecurityRoles.IsAdminRole))
                    {
                        await _adminSecurityAuditService.LogAsync(
                            HttpContext,
                            "admin_logout",
                            "Admin oturumu kapatildi.",
                            "/Admin",
                            user.Id,
                            user.UserName ?? user.Email);

                        await _adminSessionStateService.ClearSessionAsync(user.Id);
                    }
                }
            }

            HttpContext.Session.Remove(AdminSessionConstants.SessionKey);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("forgot-password")] [HttpGet("/Hesap/SifremiUnuttum")] [HttpGet("/account/SifremiUnuttum")] public IActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> SifremiUnuttum(string eposta)
        {
            ViewBag.Eposta = eposta?.Trim();
            var user = string.IsNullOrWhiteSpace(eposta)
                ? null
                : await _userManager.FindByEmailAsync(eposta.Trim());

            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var (subject, content, buttonText) = culture switch
            {
                "en" => (
                    "Password reset request",
                    $"We received a password reset request for your account. If you did not make this request, you can ignore this email.",
                    "Reset My Password"
                ),
                "ar" => (
                    "Ø·Ù„Ø¨ Ø¥Ø¹Ø§Ø¯Ø© ØªØ¹ÙŠÙŠÙ† ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ±",
                    "Ù„Ù‚Ø¯ ØªÙ„Ù‚ÙŠÙ†Ø§ Ø·Ù„Ø¨Ø§Ù‹ Ù„Ø¥Ø¹Ø§Ø¯Ø© ØªØ¹ÙŠÙŠÙ† ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± Ù„Ø­Ø³Ø§Ø¨Ùƒ. Ø¥Ø°Ø§ Ù„Ù… ØªÙ‚Ù… Ø¨Ù‡Ø°Ø§ Ø§Ù„Ø·Ù„Ø¨ØŒ ÙŠÙ…ÙƒÙ†Ùƒ ØªØ¬Ø§Ù‡Ù„ Ù‡Ø°Ù‡ Ø§Ù„Ø±Ø³Ø§Ù„Ø©.",
                    "Ø¥Ø¹Ø§Ø¯Ø© ØªØ¹ÙŠÙŠÙ† ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ±"
                ),
                _ => (
                    "طلب إعادة تعيين كلمة المرور",
                    "لقد تلقينا طلباً لإعادة تعيين كلمة المرور لحسابك. إذا لم تقم بهذا الطلب، يمكنك تجاهل هذه الرسالة.",
                    "إعادة تعيين كلمة المرور"
                )
            };

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                try
                {
                    var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(identityToken));
                    var link = Url.Action(
                        "SifreSifirla",
                        "Hesap",
                        new { userId = user.Id, token = encodedToken },
                        Request.Scheme) ?? string.Empty;

                    await _emailService.SendTemplateMailAsync(
                        user.Email,
                        subject,
                        user.AdSoyad,
                        content,
                        link,
                        buttonText,
                        culture);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send password reset email. UserId={UserId}", user.Id);
                }
            }

            ViewBag.Mesaj = _localizer["Hesap_PasswordResetLinkSent"].Value;
            TempData["Basari"] = ViewBag.Mesaj;
            return View();
        }

        [HttpGet("reset-password")] public async Task<IActionResult> SifreSifirla(string userId, string token)
        {
            var decodedToken = TryDecodePasswordResetToken(token);
            if (string.IsNullOrWhiteSpace(userId) || decodedToken == null)
            {
                TempData["Hata"] = _localizer["Hesap_InvalidOrExpiredLink"].Value;
                return RedirectToAction("SifremiUnuttum");
            }

            var user = await _userManager.FindByIdAsync(userId);
            var valid = user != null && await _userManager.VerifyUserTokenAsync(
                user,
                TokenOptions.DefaultProvider,
                UserManager<AppUser>.ResetPasswordTokenPurpose,
                decodedToken);

            if (!valid)
            {
                TempData["Hata"] = _localizer["Hesap_InvalidOrExpiredLink"].Value;
                return RedirectToAction("SifremiUnuttum");
            }

            return View(new SifreSifirlaViewModel { UserId = userId, Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> SifreSifirla(SifreSifirlaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            var decodedToken = TryDecodePasswordResetToken(model.Token);
            if (user == null || decodedToken == null)
            {
                ModelState.AddModelError(string.Empty, _localizer["Hesap_LinkExpired"].Value);
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.YeniSifre);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            TempData["Basari"] = _localizer["Hesap_PasswordUpdated"].Value;
            return RedirectToAction("GirisYap");
        }

        private static string? TryDecodePasswordResetToken(string? encodedToken)
        {
            if (string.IsNullOrWhiteSpace(encodedToken) || encodedToken.Length > 4096)
            {
                return null;
            }

            try
            {
                return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedToken));
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public IActionResult ErisimEngellendi()
        {
            return View();
        }
    }
}


