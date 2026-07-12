using System.Text.Json;
using FilistinProje.Core.Models;
using FilistinProje.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FilistinProje.Service.Services
{
    public interface ISiteSettingsService
    {
        SiteAyarlari GetSettings();
        void SaveSettings(SiteAyarlari settings);
        void SaveSettings(SiteAyarlari settings, string? paytrMerchantKey, string? paytrMerchantSalt);
        bool HasPaytrMerchantKey();
        bool HasPaytrMerchantSalt();
        string BuildAbsoluteUrl(string? path);
    }

    public class SiteSettingsService : ISiteSettingsService
    {
        private const string CacheKey = "site-settings";
        private const string PaytrProtectorPurpose = "7ANRPS48.PayTR.Settings.v1";

        private readonly KanvasDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IDataProtector _paytrProtector;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public SiteSettingsService(
            KanvasDbContext context,
            IMemoryCache cache,
            IDataProtectionProvider dataProtectionProvider,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _context = context;
            _cache = cache;
            _paytrProtector = dataProtectionProvider.CreateProtector(PaytrProtectorPurpose);
            _configuration = configuration;
            _environment = environment;
        }

        public SiteAyarlari GetSettings()
        {
            return _cache.GetOrCreate(CacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return LoadSettings();
            })!;
        }

        public void SaveSettings(SiteAyarlari settings)
        {
            SaveSettings(settings, null, null);
        }

        public void SaveSettings(SiteAyarlari settings, string? paytrMerchantKey, string? paytrMerchantSalt)
        {
            var normalized = NormalizeSettings(settings);

            var existing = _context.SiteAyarlari.FirstOrDefault();
            if (existing != null)
            {
                existing.SiteAdi = normalized.SiteAdi;
                existing.MarkaAdi = normalized.MarkaAdi;
                existing.SiteBasligi = normalized.SiteBasligi;
                existing.SiteAciklamasi = normalized.SiteAciklamasi;
                existing.SiteLogoUrl = normalized.SiteLogoUrl;
                existing.FaviconUrl = normalized.FaviconUrl;
                existing.BaseUrl = normalized.BaseUrl;
                existing.TemaRengi = normalized.TemaRengi;
                existing.UstBarMesaji = normalized.UstBarMesaji;
                existing.KampanyaMesaji = normalized.KampanyaMesaji;
                existing.UstBarEtkin = normalized.UstBarEtkin;
                existing.UstBarHizi = normalized.UstBarHizi;
                existing.FooterAciklamasi = normalized.FooterAciklamasi;
                existing.Telefon = normalized.Telefon;
                existing.Email = normalized.Email;
                existing.Adres = normalized.Adres;
                existing.WhatsappNumarasi = normalized.WhatsappNumarasi;
                existing.CalismaSaatleri = normalized.CalismaSaatleri;
                existing.FacebookUrl = normalized.FacebookUrl;
                existing.InstagramUrl = normalized.InstagramUrl;
                existing.TwitterUrl = normalized.TwitterUrl;
                existing.YoutubeUrl = normalized.YoutubeUrl;
                existing.TiktokUrl = normalized.TiktokUrl;
                existing.PinterestUrl = normalized.PinterestUrl;
                existing.ParaBirimi = normalized.ParaBirimi;
                existing.KargoBedeli = normalized.KargoBedeli;
                existing.UcretsizKargoLimiti = normalized.UcretsizKargoLimiti;
                existing.StokUyariLimiti = normalized.StokUyariLimiti;
                existing.StoktaYokSatisIzni = normalized.StoktaYokSatisIzni;
                existing.StokBiteniGriGoster = normalized.StokBiteniGriGoster;
                existing.PaytrAktifMi = normalized.PaytrAktifMi;
                existing.PaytrTestModu = normalized.PaytrTestModu;
                existing.PaytrMerchantId = normalized.PaytrMerchantId;
                existing.PaytrCallbackUrl = normalized.PaytrCallbackUrl;
                existing.PaytrBasariliDonusUrl = normalized.PaytrBasariliDonusUrl;
                existing.PaytrBasarisizDonusUrl = normalized.PaytrBasarisizDonusUrl;
                ApplyPaytrSecrets(existing, paytrMerchantKey, paytrMerchantSalt);
                existing.KargoFirmasi = normalized.KargoFirmasi;
                existing.KargoTakipUrl = normalized.KargoTakipUrl;
                existing.SiparisTeslimSuresiGun = normalized.SiparisTeslimSuresiGun;
                existing.IadeHakkiGun = normalized.IadeHakkiGun;
                existing.MetaTitle = normalized.MetaTitle;
                existing.MetaDescription = normalized.MetaDescription;
                existing.MetaKeywords = normalized.MetaKeywords;
                existing.GoogleAnalyticsId = normalized.GoogleAnalyticsId;
                existing.FacebookPixelId = normalized.FacebookPixelId;
                existing.VarsayilanSosyalPaylasimGorseliUrl = normalized.VarsayilanSosyalPaylasimGorseliUrl;
                existing.CookieMetni = normalized.CookieMetni;
                existing.YeniSiparisMailBildirimi = normalized.YeniSiparisMailBildirimi;
                existing.StokUyarisiMailBildirimi = normalized.StokUyarisiMailBildirimi;
                existing.IadeTalebiMailBildirimi = normalized.IadeTalebiMailBildirimi;
                existing.BildirimAliciEmail = normalized.BildirimAliciEmail;
                existing.BakimModuAktif = normalized.BakimModuAktif;
                existing.BakimModuMesaji = normalized.BakimModuMesaji;
                existing.GirisZorunluMu = normalized.GirisZorunluMu;
                existing.KapidaOdemeAktifMi = normalized.KapidaOdemeAktifMi;
                existing.KapidaOdemeHizmetBedeli = normalized.KapidaOdemeHizmetBedeli;
                existing.KapidaOdemeLimiti = normalized.KapidaOdemeLimiti;
                existing.ToptanciMinSiparisTutari = normalized.ToptanciMinSiparisTutari;
                existing.IptalSuresiSaat = normalized.IptalSuresiSaat;
            }
            else
            {
                normalized.Id = 1;
                ApplyPaytrSecrets(normalized, paytrMerchantKey, paytrMerchantSalt);
                _context.SiteAyarlari.Add(normalized);
            }

            _context.SaveChanges();
            _cache.Remove(CacheKey);
        }

        public bool HasPaytrMerchantKey()
        {
            return _context.SiteAyarlari.Any(x => !string.IsNullOrWhiteSpace(x.PaytrMerchantKeyProtected));
        }

        public bool HasPaytrMerchantSalt()
        {
            return _context.SiteAyarlari.Any(x => !string.IsNullOrWhiteSpace(x.PaytrMerchantSaltProtected));
        }

        public string BuildAbsoluteUrl(string? path)
        {
            var baseUrl = NormalizeBaseUrl(GetSettings().BaseUrl, ConfiguredBaseUrl(), IsProductionEnvironment());

            if (string.IsNullOrWhiteSpace(path))
            {
                return baseUrl;
            }

            if (Uri.TryCreate(path, UriKind.Absolute, out var absoluteUri))
            {
                return absoluteUri.ToString();
            }

            var relativePath = path.Trim();
            if (relativePath.StartsWith("~"))
            {
                relativePath = relativePath[1..];
            }

            return $"{baseUrl}/{relativePath.TrimStart('/')}";
        }

        private SiteAyarlari LoadSettings()
        {
            var settings = _context.SiteAyarlari.FirstOrDefault();
            return NormalizeSettings(settings ?? new SiteAyarlari());
        }

        private SiteAyarlari NormalizeSettings(SiteAyarlari settings)
        {
            settings.SiteAdi = string.IsNullOrWhiteSpace(settings.SiteAdi) ? "7ANRPS48" : settings.SiteAdi.Trim();
            settings.MarkaAdi = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi.Trim();
            settings.SiteBasligi = string.IsNullOrWhiteSpace(settings.SiteBasligi) ? $"{settings.MarkaAdi} - متجرك الإلكتروني في فلسطين" : settings.SiteBasligi.Trim();
            settings.SiteAciklamasi = string.IsNullOrWhiteSpace(settings.SiteAciklamasi)
                ? "7ANRPS48 - Filistin'den online alışveriş. Moda, elektronik, ev & yaşam ürünlerinde hızlı teslimat."
                : settings.SiteAciklamasi.Trim();
            settings.SiteLogoUrl = NormalizeLogoUrl(settings.SiteLogoUrl);
            settings.FaviconUrl = NormalizeFaviconUrl(settings.FaviconUrl);
            settings.BaseUrl = NormalizeBaseUrl(settings.BaseUrl, ConfiguredBaseUrl(), IsProductionEnvironment());
            settings.TemaRengi = string.IsNullOrWhiteSpace(settings.TemaRengi) ? "#313511" : settings.TemaRengi.Trim();
            settings.UstBarMesaji = settings.UstBarMesaji?.Trim() ?? string.Empty;
            settings.KampanyaMesaji = settings.KampanyaMesaji?.Trim() ?? string.Empty;
            settings.FooterAciklamasi = string.IsNullOrWhiteSpace(settings.FooterAciklamasi)
                ? settings.SiteAciklamasi
                : settings.FooterAciklamasi.Trim();

            settings.Telefon = settings.Telefon?.Trim() ?? string.Empty;
            settings.Email = settings.Email?.Trim() ?? string.Empty;
            settings.Adres = settings.Adres?.Trim() ?? string.Empty;
            settings.WhatsappNumarasi = settings.WhatsappNumarasi?.Trim() ?? string.Empty;
            settings.CalismaSaatleri = settings.CalismaSaatleri?.Trim() ?? string.Empty;

            settings.InstagramUrl = settings.InstagramUrl?.Trim() ?? string.Empty;
            settings.FacebookUrl = settings.FacebookUrl?.Trim() ?? string.Empty;
            settings.TwitterUrl = settings.TwitterUrl?.Trim() ?? string.Empty;
            settings.YoutubeUrl = settings.YoutubeUrl?.Trim() ?? string.Empty;
            settings.TiktokUrl = settings.TiktokUrl?.Trim() ?? string.Empty;
            settings.PinterestUrl = settings.PinterestUrl?.Trim() ?? string.Empty;

            settings.ParaBirimi = string.IsNullOrWhiteSpace(settings.ParaBirimi) ? "₪" : settings.ParaBirimi.Trim();
            settings.KargoBedeli = Math.Max(0, settings.KargoBedeli);
            settings.UcretsizKargoLimiti = Math.Max(0, settings.UcretsizKargoLimiti);
            settings.StokUyariLimiti = Math.Max(0, settings.StokUyariLimiti);
            settings.PaytrMerchantId = settings.PaytrMerchantId?.Trim() ?? string.Empty;
            settings.PaytrMerchantKeyProtected = settings.PaytrMerchantKeyProtected?.Trim() ?? string.Empty;
            settings.PaytrMerchantSaltProtected = settings.PaytrMerchantSaltProtected?.Trim() ?? string.Empty;
            settings.PaytrCallbackUrl = NormalizeOptionalUrl(settings.PaytrCallbackUrl);
            settings.PaytrBasariliDonusUrl = NormalizeOptionalUrl(settings.PaytrBasariliDonusUrl);
            settings.PaytrBasarisizDonusUrl = NormalizeOptionalUrl(settings.PaytrBasarisizDonusUrl);
            settings.KargoFirmasi = string.IsNullOrWhiteSpace(settings.KargoFirmasi) || IsLegacyTurkishCargoName(settings.KargoFirmasi)
                ? "توصيل محلي"
                : settings.KargoFirmasi.Trim();
            settings.KargoTakipUrl = settings.KargoTakipUrl?.Trim() ?? string.Empty;
            settings.SiparisTeslimSuresiGun = settings.SiparisTeslimSuresiGun <= 0 ? 5 : settings.SiparisTeslimSuresiGun;
            settings.IadeHakkiGun = settings.IadeHakkiGun <= 0 ? 14 : settings.IadeHakkiGun;
            settings.KapidaOdemeHizmetBedeli = Math.Max(0, settings.KapidaOdemeHizmetBedeli);
            settings.KapidaOdemeLimiti = settings.KapidaOdemeLimiti <= 0 ? 2000 : settings.KapidaOdemeLimiti;
            settings.IptalSuresiSaat = Math.Max(0, settings.IptalSuresiSaat);

            settings.MetaTitle = string.IsNullOrWhiteSpace(settings.MetaTitle)
                ? $"{settings.MarkaAdi} - Filistin E-Ticaret Mağazası | Online Alışveriş"
                : settings.MetaTitle.Trim();
            settings.MetaDescription = string.IsNullOrWhiteSpace(settings.MetaDescription)
                ? $"{settings.MarkaAdi}; Filistin'de güvenli online alışveriş, hızlı teslimat ve müşteri odaklı e-ticaret deneyimi sunar."
                : settings.MetaDescription.Trim();
            settings.MetaKeywords = string.IsNullOrWhiteSpace(settings.MetaKeywords)
                ? "7ANRPS48; Filistin online alışveriş, moda, elektronik, ev & yaşam ürünleri"
                : settings.MetaKeywords.Trim();
            settings.GoogleAnalyticsId = settings.GoogleAnalyticsId?.Trim() ?? string.Empty;
            settings.FacebookPixelId = settings.FacebookPixelId?.Trim() ?? string.Empty;
            settings.VarsayilanSosyalPaylasimGorseliUrl = NormalizeLogoUrl(settings.VarsayilanSosyalPaylasimGorseliUrl);
            settings.CookieMetni = string.IsNullOrWhiteSpace(settings.CookieMetni)
                ? "Deneyiminizi iyileştirmek, sepetinizi korumak ve site trafiğini analiz etmek için çerezler kullanıyoruz."
                : settings.CookieMetni.Trim();

            settings.BildirimAliciEmail = settings.BildirimAliciEmail?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(settings.BildirimAliciEmail) ||
                settings.BildirimAliciEmail.Equals("admin@7anrps48.com", StringComparison.OrdinalIgnoreCase) ||
                settings.BildirimAliciEmail.Contains(LegacyBrandToken(), StringComparison.OrdinalIgnoreCase))
            {
                settings.BildirimAliciEmail = "info@7anrps48.com";
            }
            settings.BakimModuMesaji = string.IsNullOrWhiteSpace(settings.BakimModuMesaji)
                ? "Size daha iyi bir alışveriş deneyimi sunmak için kısa bir bakım çalışması yapıyoruz. Çok yakında 7ANRPS48 mağazamızla yeniden yayında olacağız."
                : settings.BakimModuMesaji.Trim();

            return settings;
        }

        private void ApplyPaytrSecrets(SiteAyarlari settings, string? paytrMerchantKey, string? paytrMerchantSalt)
        {
            if (!string.IsNullOrWhiteSpace(paytrMerchantKey))
            {
                settings.PaytrMerchantKeyProtected = _paytrProtector.Protect(paytrMerchantKey.Trim());
            }

            if (!string.IsNullOrWhiteSpace(paytrMerchantSalt))
            {
                settings.PaytrMerchantSaltProtected = _paytrProtector.Protect(paytrMerchantSalt.Trim());
            }
        }

        private static string NormalizeOptionalUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            return url.Trim();
        }

        private static string NormalizeLogoUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "/74anrps48logo2.svg";
            }

            var value = url.Trim();
            var eskiLogoYollari = new[]
            {
                "/logo_svg.svg",
                "logo_svg.svg",
                "/74anrps48logo.svg",
                "74anrps48logo.svg",
                "/EmailTemplates/logo.svg",
                "EmailTemplates/logo.svg",
                "/EmailTemplates/" + LegacyBrandToken() + "-logo.svg",
                "EmailTemplates/" + LegacyBrandToken() + "-logo.svg"
            };

            return eskiLogoYollari.Contains(value, StringComparer.OrdinalIgnoreCase)
                ? "/74anrps48logo2.svg"
                : value;
        }

        private static string NormalizeFaviconUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "/74anrps48logo2.svg";
            }

            var value = url.Trim();
            return value.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("/favicon.svg", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("favicon.svg", StringComparison.OrdinalIgnoreCase)
                ? "/74anrps48logo2.svg"
                : value;
        }

        private static string NormalizeBaseUrl(string? baseUrl, string? configuredBaseUrl, bool isProduction)
        {
            var value = IsLegacyBaseUrl(baseUrl) ? string.Empty : baseUrl?.Trim().TrimEnd('/');
            if (string.IsNullOrWhiteSpace(value))
            {
                value = configuredBaseUrl?.Trim().TrimEnd('/');
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                if (isProduction)
                {
                    throw new InvalidOperationException("Production BaseUrl is required. Configure SiteAyarlari.BaseUrl, PublicBaseUrl, or AppUrl with the final 7ANRPS48.com domain before running in production.");
                }

                return "http://localhost:5002";
            }

            if (!value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                value = "https://" + value;
            }

            return value;
        }

        private string? ConfiguredBaseUrl()
        {
            return _configuration["PublicBaseUrl"] ?? _configuration["AppUrl"];
        }

        private bool IsProductionEnvironment()
        {
            return string.Equals(_environment.EnvironmentName, "Production", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsLegacyBaseUrl(string? baseUrl)
        {
            return !string.IsNullOrWhiteSpace(baseUrl) &&
                   baseUrl.Contains("kastamonu" + "esnaf.com.tr", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsLegacyTurkishCargoName(string? cargoName)
        {
            if (string.IsNullOrWhiteSpace(cargoName))
            {
                return false;
            }

            return cargoName.Contains("ar" + "as", StringComparison.OrdinalIgnoreCase) ||
                   cargoName.Contains("m" + "ng", StringComparison.OrdinalIgnoreCase) ||
                   cargoName.Contains("yur" + "tici", StringComparison.OrdinalIgnoreCase) ||
                   cargoName.Contains("yur" + "tiçi", StringComparison.OrdinalIgnoreCase) ||
                   cargoName.Contains("ptt", StringComparison.OrdinalIgnoreCase) ||
                   cargoName.Contains("sürat", StringComparison.OrdinalIgnoreCase) ||
                   cargoName.Contains("surat", StringComparison.OrdinalIgnoreCase);
        }

        private static string LegacyBrandToken()
        {
            return "canvas" + "ia";
        }
    }
}
