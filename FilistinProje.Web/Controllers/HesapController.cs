using System.Globalization;
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
        public IActionResult GirisYap(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("login")]
        [HttpPost("/Hesap/GirisYap")]
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
                        await sepetService.MergeSepetlerAsync(sessionId, user.Id);

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
                    _logger.LogError(ex, "Parola sÄ±fÄ±rlama e-postasÄ± gÃ¶nderilemedi. UserId={UserId}", user.Id);
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


