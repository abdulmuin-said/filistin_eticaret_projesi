using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net;

namespace FilistinProje.Web.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        private const int IadeHakkiGun = 14;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly KanvasDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly ILogger<ProfilController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProfilController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            KanvasDbContext context,
            IEmailService emailService,
            ISiteSettingsService siteSettingsService,
            ILogger<ProfilController> logger,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
            _siteSettingsService = siteSettingsService;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparislerim = await GetOwnedOrdersQuery(user)
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .ToListAsync();

            ViewBag.SiparisSayisi = siparislerim.Count;
            var toplamHarcama = siparislerim
                .Where(x => x.Durum is >= 1 and < 4 && x.ToplamTutar > 0 && x.ToplamTutar <= 1_000_000)
                .Sum(x => x.ToplamTutar);
            ViewBag.ToplamHarcama = toplamHarcama.ToString("N2", new System.Globalization.CultureInfo("tr-TR"));

            // Favori bilgileri
            var favoriler = await _context.Favoriler
                .AsNoTracking()
                .Include(f => f.Urun)
                .Where(f => f.AppUserId == user.Id && !f.SilindiMi)
                .OrderByDescending(f => f.OlusturulmaTarihi)
                .ToListAsync();

            ViewBag.FavoriSayisi = favoriler.Count;
            ViewBag.SonFavoriler = favoriler.Take(4).ToList();

            return View(user);
        }

        public async Task<IActionResult> Siparislerim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparislerim = await GetOwnedOrdersQuery(user)
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .ToListAsync();

            return View(siparislerim);
        }

        public async Task<IActionResult> SiparisDetay(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparis = await GetOwnedOrdersQuery(user)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (siparis == null)
            {
                return RedirectToAction("ErisimEngellendi", "Hesap");
            }

            var iade = await _context.IadeTalepleri
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SiparisId == id && !x.SilindiMi);

            if (iade != null)
            {
                ViewBag.IadeDurumu = iade.Durum switch
                {
                    0 => _localizer["Profil_ReturnStatusUnderReview"].Value,
                    1 => _localizer["Profil_ReturnStatusApproved"].Value,
                    2 => _localizer["Profil_ReturnStatusCompleted"].Value,
                    _ => _localizer["Profil_ReturnStatusRejected"].Value
                };
            }

            var siparisKalemleri = await _context.SiparisDetaylari
                .AsNoTracking()
                .Include(x => x.Urun)
                .Include(x => x.UrunSecenek)
                .Where(x => x.SiparisId == id && !x.SilindiMi)
                .ToListAsync();

            ViewBag.Urunler = siparisKalemleri
                .Where(x => x.Urun != null)
                .Select(x => new
                {
                    Resim = !string.IsNullOrWhiteSpace(x.UrunSecenek?.GorselUrl) ? x.UrunSecenek.GorselUrl : x.Urun!.AnaGorselUrl,
                    Baslik = x.Urun.Baslik,
                    Olcu = x.UrunSecenek?.Olcu ?? string.Empty,
                    Cerceve = x.UrunSecenek?.CerceveTipi ?? string.Empty,
                    Secenek = x.UrunSecenek == null
                        ? _localizer["Profil_StandardProduct"].Value
                        : (string.IsNullOrWhiteSpace(x.UrunSecenek.VaryantBasligi) ? _localizer["Profil_DefaultVariant"].Value : x.UrunSecenek.VaryantBasligi),
                    SecenekDetay = x.UrunSecenek?.VaryantOzeti ?? string.Empty,
                    CerceveModeli = x.CerceveModeli,
                    Adet = x.Adet,
                    Fiyat = x.BirimFiyat,
                    Toplam = x.Adet * x.BirimFiyat,
                    UrunId = x.Urun!.Id,
                    MusteriNotu = x.MusteriNotu
                })
                .ToList();

            return View(siparis);
        }

        [HttpGet]
        public async Task<IActionResult> IadeOlustur(int siparisId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparis = await GetOwnedOrdersQuery(user)
                .FirstOrDefaultAsync(x => x.Id == siparisId);

            if (siparis == null)
            {
                return NotFound();
            }

            if (siparis.Durum != 3)
            {
                TempData["Hata"] = _localizer["Profil_ReturnOnlyDelivered"].Value;
                return RedirectToAction(nameof(SiparisDetay), new { id = siparisId });
            }

            if (!IadeSuresiDevamEdiyor(siparis))
            {
                TempData["Hata"] = string.Format(_localizer["Profil_ReturnPeriodExpired"].Value, IadeHakkiGun);
                return RedirectToAction(nameof(SiparisDetay), new { id = siparisId });
            }

            var mevcutTalep = await _context.IadeTalepleri
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SiparisId == siparisId && !x.SilindiMi);

            if (mevcutTalep != null)
            {
                TempData["Bilgi"] = _localizer["Profil_ReturnAlreadyExists"].Value;
                return RedirectToAction(nameof(SiparisDetay), new { id = siparisId });
            }

            return View(siparis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IadeOlustur([Bind("SiparisId,Neden,Aciklama,IBAN")] IadeTalebi talep)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparis = await GetOwnedOrdersQuery(user)
                .FirstOrDefaultAsync(x => x.Id == talep.SiparisId);

            if (siparis == null)
            {
                return RedirectToAction("ErisimEngellendi", "Hesap");
            }

            if (siparis.Durum != 3)
            {
                TempData["Hata"] = _localizer["Profil_ReturnOnlyDeliveredPost"].Value;
                return RedirectToAction(nameof(SiparisDetay), new { id = talep.SiparisId });
            }

            if (!IadeSuresiDevamEdiyor(siparis))
            {
                TempData["Hata"] = string.Format(_localizer["Profil_ReturnPeriodExpired"].Value, IadeHakkiGun);
                return RedirectToAction(nameof(SiparisDetay), new { id = talep.SiparisId });
            }

            talep.Neden = talep.Neden?.Trim() ?? string.Empty;
            talep.Aciklama = talep.Aciklama?.Trim();
            talep.IBAN = talep.IBAN?.Trim();

            if (string.IsNullOrWhiteSpace(talep.Neden))
            {
                TempData["Hata"] = _localizer["Profil_ReturnReasonRequired"].Value;
                return RedirectToAction(nameof(IadeOlustur), new { siparisId = talep.SiparisId });
            }

            var mevcutTalep = await _context.IadeTalepleri
                .FirstOrDefaultAsync(x => x.SiparisId == talep.SiparisId && !x.SilindiMi);

            if (mevcutTalep != null)
            {
                TempData["Bilgi"] = _localizer["Profil_ReturnRequestExists"].Value;
                return RedirectToAction(nameof(SiparisDetay), new { id = talep.SiparisId });
            }

            talep.MusteriId = user.Id;
            talep.OlusturulmaTarihi = DateTime.UtcNow;
            talep.Durum = 0;
            talep.SilindiMi = false;

            _context.IadeTalepleri.Add(talep);
            siparis.Durum = 5;

            await _context.SaveChangesAsync();
            await SendReturnRequestEmailsAsync(talep, siparis);

            TempData["Basari"] = _localizer["Profil_ReturnRequestReceived"].Value;
            return RedirectToAction(nameof(SiparisDetay), new { id = talep.SiparisId });
        }

        public async Task<IActionResult> Adreslerim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var adreslerim = await _context.Adresler
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id && !x.SilindiMi)
                .OrderByDescending(x => x.OlusturulmaTarihi)
                .ToListAsync();

            return View(adreslerim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdresEkle(Adres adres)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            adres.AppUserId = user.Id;
            adres.Baslik = adres.Baslik?.Trim() ?? string.Empty;
            adres.AdSoyad = adres.AdSoyad?.Trim() ?? string.Empty;
            adres.Telefon = adres.Telefon?.Trim() ?? string.Empty;
            adres.Sehir = adres.Sehir?.Trim() ?? string.Empty;
            adres.Ilce = adres.Ilce?.Trim() ?? string.Empty;
            adres.AcikAdres = adres.AcikAdres?.Trim() ?? string.Empty;
            adres.OlusturulmaTarihi = DateTime.UtcNow;
            adres.SilindiMi = false;

            _context.Adresler.Add(adres);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = _localizer["Profil_AddressAdded"].Value;
            return RedirectToAction(nameof(Adreslerim));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdresSil(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var adres = await _context.Adresler
                .FirstOrDefaultAsync(x => x.Id == id && x.AppUserId == user.Id && !x.SilindiMi);

            if (adres != null)
            {
                adres.SilindiMi = true;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = _localizer["Profil_AddressDeleted"].Value;
            }

            return RedirectToAction(nameof(Adreslerim));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SiparisIptal(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var siparis = await GetOwnedOrdersQuery(user)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (siparis == null)
            {
                TempData["Hata"] = _localizer["Profil_OrderNotFound"].Value;
                return RedirectToAction(nameof(Siparislerim));
            }

            // Sadece SiparisAlindi (0) veya UretimHazirlaniyor (1) durumundaki siparişler iptal edilebilir
            if (siparis.Durum != SiparisDurumHelper.SiparisAlindi && siparis.Durum != SiparisDurumHelper.UretimHazirlaniyor)
            {
                TempData["Hata"] = _localizer["Profil_OrderCannotCancel"].Value;
                return RedirectToAction(nameof(SiparisDetay), new { id });
            }

            // İptal süresi kontrolü
            var settings = _siteSettingsService.GetSettings();
            if (settings.IptalSuresiSaat > 0)
            {
                var gecenSure = (DateTime.UtcNow - siparis.OlusturulmaTarihi.ToUniversalTime()).TotalHours;
                if (gecenSure > settings.IptalSuresiSaat)
                {
                    TempData["Hata"] = string.Format(
                        _localizer["Profil_CancelPeriodExpired"].Value,
                        settings.IptalSuresiSaat);
                    return RedirectToAction(nameof(SiparisDetay), new { id });
                }
            }
            else if (settings.IptalSuresiSaat == 0)
            {
                // 0 = iptal tamamen kapalı
                TempData["Hata"] = _localizer["Profil_CancelNotAllowed"].Value;
                return RedirectToAction(nameof(SiparisDetay), new { id });
            }

            siparis.Durum = SiparisDurumHelper.IptalEdildi;
            await _context.SaveChangesAsync();

            TempData["Basari"] = _localizer["Profil_OrderCancelled"].Value;
            return RedirectToAction(nameof(SiparisDetay), new { id });
        }

        [HttpGet]
        public IActionResult HesabiSil()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HesabiSilOnayla(string sifre)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, sifre);
            if (!passwordCheck)
            {
                ViewBag.Hata = _localizer["Profil_WrongPassword"].Value;
                return View("HesabiSil");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                TempData["Bilgi"] = _localizer["Profil_AccountDeleted"].Value;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hata = _localizer["Profil_AccountDeleteFailed"].Value;
            return View("HesabiSil");
        }

        private IQueryable<Siparis> GetOwnedOrdersQuery(AppUser user)
        {
            var normalizedEmail = user.Email ?? string.Empty;
            return _context.Siparisler.Where(x =>
                !x.SilindiMi &&
                (x.AppUserId == user.Id ||
                 (string.IsNullOrEmpty(x.AppUserId) && x.Eposta == normalizedEmail)));
        }

        private static bool IadeSuresiDevamEdiyor(Siparis siparis)
        {
            return DateTime.UtcNow <= siparis.OlusturulmaTarihi.ToUniversalTime().AddDays(IadeHakkiGun);
        }

        private async Task SendReturnRequestEmailsAsync(IadeTalebi talep, Siparis siparis)
        {
            var settings = _siteSettingsService.GetSettings();
            if (!settings.IadeTalebiMailBildirimi)
            {
                return;
            }

            var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
            var siteUrl = _siteSettingsService.BuildAbsoluteUrl(string.Empty);
            var orderNumber = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(siparis.SiparisNo) ? $"#{siparis.Id}" : siparis.SiparisNo);
            var customerName = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(siparis.MusteriAdSoyad) ? _localizer["Siparis_EmailDearCustomer"].Value : siparis.MusteriAdSoyad);
            var returnReason = WebUtility.HtmlEncode(talep.Neden);
            var returnDescription = string.IsNullOrWhiteSpace(talep.Aciklama)
                ? string.Empty
                : $"<p><strong>{_localizer["Siparis_EmailDescriptionLabel"].Value}</strong> {WebUtility.HtmlEncode(talep.Aciklama)}</p>";
            var productRows = await BuildReturnRequestProductRowsAsync(siparis.Id);
            var profileUrl = Url.Action(nameof(SiparisDetay), "Profil", new { id = siparis.Id }, Request.Scheme)
                ?? $"{siteUrl}/Profil/SiparisDetay/{siparis.Id}";

            if (!string.IsNullOrWhiteSpace(siparis.Eposta))
            {
                try
                {
                    var customerContent = $@"
                        <p>{_localizer["Profil_EmailHello"].Value} <strong>{customerName}</strong>,</p>
                        <p>{string.Format(_localizer["Profil_EmailReturnReceivedBody"].Value, $"<strong>{orderNumber}</strong>")}</p>
                        <p><strong>{_localizer["Profil_EmailReturnReasonLabel"].Value}</strong> {returnReason}</p>
                        {returnDescription}
                        <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:10px; background:#fff; margin:16px 0;'>
                            <thead>
                                <tr style='background:#fafaf8;'>
                                    <th style='padding:10px; text-align:left; color:#313511;'>Urun</th>
                                    <th style='padding:10px; text-align:center; color:#313511;'>Adet</th>
                                    <th style='padding:10px; text-align:right; color:#313511;'>Tutar</th>
                                </tr>
                            </thead>
                            <tbody>{productRows}</tbody>
                        </table>
                        <p>{_localizer["Profil_EmailReturnTrackBody"].Value}</p>";

                    await _emailService.SendTemplateMailAsync(
                        siparis.Eposta,
                        $"{brandName} - {_localizer["Profil_EmailReturnReceivedSubject"].Value} ({orderNumber})",
                        customerName,
                        customerContent,
                        profileUrl,
                        _localizer["Profil_ViewOrder"].Value);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Musteri iade talebi maili gonderilemedi. SiparisNo={SiparisNo}, Email={Email}", siparis.SiparisNo, siparis.Eposta);
                }
            }

            var recipientEmail = string.IsNullOrWhiteSpace(settings.BildirimAliciEmail)
                ? settings.Email
                : settings.BildirimAliciEmail;

            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return;
            }

            try
            {
                var adminUrl = Url.Action("Detay", "Iade", new { area = "Admin", id = talep.Id }, Request.Scheme)
                    ?? $"{siteUrl}/Admin/Iade/Detay/{talep.Id}";
                var customerEmail = WebUtility.HtmlEncode(siparis.Eposta ?? "-");
                var customerPhone = WebUtility.HtmlEncode(siparis.Telefon ?? "-");

                var adminContent = $@"
                    <h3>{_localizer["Profil_EmailAdminNewReturnHeading"].Value}</h3>
                    <p><strong>{_localizer["Profil_EmailSiparisNo"].Value}</strong> {orderNumber}</p>
                    <p><strong>{_localizer["Profil_EmailMusteri"].Value}</strong> {customerName}</p>
                    <p><strong>{_localizer["Profil_EmailEposta"].Value}</strong> {customerEmail}</p>
                    <p><strong>{_localizer["Profil_EmailTelefon"].Value}</strong> {customerPhone}</p>
                    <p><strong>{_localizer["Profil_EmailReturnReasonLabel"].Value}</strong> {returnReason}</p>
                    {returnDescription}
                    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:10px; background:#fff; margin:16px 0;'>
                        <thead>
                            <tr style='background:#fafaf8;'>
                                <th style='padding:10px; text-align:left; color:#313511;'>{_localizer["Siparis_EmailUrunLabel"].Value}</th>
                                <th style='padding:10px; text-align:center; color:#313511;'>{_localizer["Siparis_EmailAdetLabel"].Value}</th>
                                <th style='padding:10px; text-align:right; color:#313511;'>{_localizer["Siparis_EmailTutarLabel"].Value}</th>
                            </tr>
                        </thead>
                        <tbody>{productRows}</tbody>
                    </table>";

                await _emailService.SendTemplateMailAsync(
                    recipientEmail,
                    $"{brandName} - {_localizer["Profil_EmailNewReturnSubject"].Value} {siparis.SiparisNo}",
                    "Operasyon Ekibi",
                    adminContent,
                    adminUrl,
                    _localizer["Profil_ReviewReturn"].Value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Admin iade talebi maili gonderilemedi. SiparisNo={SiparisNo}, TalepId={TalepId}", siparis.SiparisNo, talep.Id);
            }
        }

        private async Task<string> BuildReturnRequestProductRowsAsync(int siparisId)
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
                var productName = WebUtility.HtmlEncode(item.Urun?.Baslik ?? _localizer["Siparis_EmailProduct"].Value);
                var variant = WebUtility.HtmlEncode(item.UrunSecenek?.VaryantOzeti ?? item.CerceveModeli ?? string.Empty);
                var note = string.IsNullOrWhiteSpace(item.MusteriNotu)
                    ? string.Empty
                    : $"<div style='margin-top:4px; font-size:12px; color:#b58735;'>{_localizer["Profil_EmailNoteLabel"].Value} {WebUtility.HtmlEncode(item.MusteriNotu)}</div>";

                rows.Append($@"
                    <tr>
                        <td style='padding:12px; border-bottom:1px solid #e5e2dc; color:#47473d;'>
                            <div>{productName}</div>
                            {(string.IsNullOrWhiteSpace(variant) ? string.Empty : $"<div style='margin-top:4px; font-size:12px; color:#6f6a5e;'>{variant}</div>")}
                            {note}
                        </td>
                        <td style='padding:12px; border-bottom:1px solid #e5e2dc; text-align:center; color:#47473d;'>{item.Adet}</td>
                        <td style='padding:12px; border-bottom:1px solid #e5e2dc; text-align:right; color:#313511; font-weight:600;'>{(item.Adet * item.BirimFiyat):N2} {currencySymbol}</td>
                    </tr>");
            }

            return rows.Length > 0
                ? rows.ToString()
                : $"<tr><td colspan='3' style='padding:12px; color:#6f6a5e;'>{_localizer["Profil_ProductDetailNotFound"].Value}</td></tr>";
        }

        private string GetCurrencySymbol()
        {
            var settings = _siteSettingsService.GetSettings();
            return string.IsNullOrWhiteSpace(settings.ParaBirimi) ? "₪" : settings.ParaBirimi;
        }
    }
}
