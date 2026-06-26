using System.Globalization;
using FilistinProje.Core.Enums;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Models;
using FilistinProje.Web.Resources;
using FilistinProje.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Controllers
{
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("auth")]
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

        public HesapController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            KanvasDbContext context,
            IDosyaServisi dosyaServisi,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            IAdminSessionStateService adminSessionStateService,
            IAdminSecurityAuditService adminSecurityAuditService,
            IStringLocalizer<SharedResource> localizer)
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
        }

        [HttpGet]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KayitOl(KayitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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
                    "تحقق من عنوان بريدك الإلكتروني",
                    $"تم إنشاء حسابك في {brandName}. يرجى التحقق من عنوان بريدك الإلكتروني لاستخدام حسابك بأمان.",
                    "تحقق من حسابي"
                ),
                _ => (
                    "Hesabınızı doğrulayın",
                    $"{brandName} hesabınız oluşturuldu. Hesabınızı güvenli şekilde kullanabilmek için e-posta adresinizi doğrulamanız gerekiyor.",
                    "Hesabımı Doğrula"
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

        [HttpGet]
        public IActionResult EpostaOnayBilgilendirme()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EpostaDogrula(string userId, string token)
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

        [HttpGet]
        public IActionResult GirisYap(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifremiUnuttum(string eposta)
        {
            ViewBag.Eposta = eposta;
            if (string.IsNullOrWhiteSpace(eposta))
            {
                ViewBag.Hata = _localizer["Hesap_EnterRegisteredEmail"].Value;
                TempData["Hata"] = ViewBag.Hata;
                return View();
            }

            var user = await _userManager.FindByEmailAsync(eposta);
            if (user == null)
            {
                ViewBag.Mesaj = _localizer["Hesap_PasswordResetLinkSent"].Value;
                TempData["Basari"] = ViewBag.Mesaj;
                return View();
            }

            user.SifreSifirlamaToken = Guid.NewGuid().ToString();
            user.SifreSifirlamaGecerlilik = DateTime.UtcNow.AddHours(1);
            await _userManager.UpdateAsync(user);

            var link = Url.Action("SifreSifirla", "Hesap", new { token = user.SifreSifirlamaToken }, Request.Scheme) ?? string.Empty;

            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var (subject, content, buttonText) = culture switch
            {
                "en" => (
                    "Password reset request",
                    $"We received a password reset request for your account. If you did not make this request, you can ignore this email.",
                    "Reset My Password"
                ),
                "ar" => (
                    "طلب إعادة تعيين كلمة المرور",
                    "لقد تلقينا طلباً لإعادة تعيين كلمة المرور لحسابك. إذا لم تقم بهذا الطلب، يمكنك تجاهل هذه الرسالة.",
                    "إعادة تعيين كلمة المرور"
                ),
                _ => (
                    "Şifre sıfırlama talebi",
                    "Hesabınız için bir şifre sıfırlama talebi aldık. Bu işlemi siz yapmadıysanız mesajı yok sayabilirsiniz.",
                    "Şifremi Sıfırla"
                )
            };

            try
            {
                await _emailService.SendTemplateMailAsync(
                    user.Email ?? string.Empty,
                    subject,
                    user.AdSoyad,
                    content,
                    link,
                    buttonText,
                    culture);
            }
            catch (Exception ex)
            {
                ViewBag.Hata = _localizer["Hesap_PasswordResetFailed"].Value + ex.Message;
                TempData["Hata"] = ViewBag.Hata;
                return View();
            }

            ViewBag.Mesaj = _localizer["Hesap_PasswordResetSuccess"].Value;
            TempData["Basari"] = ViewBag.Mesaj;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SifreSifirla(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("GirisYap");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.SifreSifirlamaToken == token && x.SifreSifirlamaGecerlilik > DateTime.UtcNow);
            if (user == null)
            {
                ViewBag.Hata = _localizer["Hesap_InvalidOrExpiredLink"].Value;
                TempData["Hata"] = ViewBag.Hata;
                return RedirectToAction("SifremiUnuttum");
            }

            return View(new SifreSifirlaViewModel { Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreSifirla(SifreSifirlaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.SifreSifirlamaToken == model.Token && x.SifreSifirlamaGecerlilik > DateTime.UtcNow);
            if (user == null)
            {
                ViewBag.Hata = _localizer["Hesap_LinkExpired"].Value;
                TempData["Hata"] = ViewBag.Hata;
                return View(model);
            }

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.YeniSifre);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            user.SifreSifirlamaToken = null;
            user.SifreSifirlamaGecerlilik = null;
            await _userManager.UpdateAsync(user);

            TempData["Basari"] = _localizer["Hesap_PasswordUpdated"].Value;
            return RedirectToAction("GirisYap");
        }

        public IActionResult ErisimEngellendi()
        {
            return View();
        }
    }
}
