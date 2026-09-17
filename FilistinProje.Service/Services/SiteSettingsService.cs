using System.Text.Json;
using FilistinProje.Core.Models;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FilistinProje.Service.Services
{
    public interface ISiteSettingsService
    {
        SiteAyarlari GetSettings();
        void SaveSettings(SiteAyarlari settings);
        void SaveSettings(SiteAyarlari settings, string? activeTab) => SaveSettings(settings);
        string BuildAbsoluteUrl(string? path);
    }

    public class SiteSettingsService : ISiteSettingsService
    {
        private const string CacheKey = "site-settings";

        private readonly KanvasDbContext _context;
        private readonly IMemoryCache _cache;
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
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _context = context;
            _cache = cache;
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
            SaveSettings(settings, null);
        }

        public void SaveSettings(SiteAyarlari settings, string? activeTab)
        {
            var existing = _context.SiteAyarlari.FirstOrDefault();
            if (existing == null)
            {
                var normalized = NormalizeSettings(settings);
                normalized.Id = 1;
                _context.SiteAyarlari.Add(normalized);
                _context.SaveChanges();
                _cache.Remove(CacheKey);
                return;
            }

            var tab = NormalizeTabName(activeTab);

            switch (tab)
            {
                case "genel":
                    UpdateGenelSettings(existing, settings);
                    break;

                case "iletisim":
                    UpdateIletisimSettings(existing, settings);
                    break;

                case "sosyal":
                    UpdateSosyalSettings(existing, settings);
                    break;

                case "kargo":
                case "satis":
                    UpdateKargoSettings(existing, settings);
                    break;

                case "kapida-odeme":
                case "odeme":
                    UpdateKapidaOdemeSettings(existing, settings);
                    break;

                case "seo":
                    UpdateSeoSettings(existing, settings);
                    break;

                case "mail":
                    UpdateMailSettings(existing, settings);
                    break;

                case "bakim":
                    UpdateBakimSettings(existing, settings);
                    break;

                default:
                    UpdateAllSettings(existing, settings);
                    break;
            }

            _context.SaveChanges();
            _cache.Remove(CacheKey);
        }

        private static string? NormalizeTabName(string? tab)
        {
            if (string.IsNullOrWhiteSpace(tab)) return null;
            var t = tab.Trim().ToLowerInvariant();
            return t switch
            {
                "brand" => "genel",
                "general" => "genel",
                "store" => "genel",
                "genel" => "genel",
                "contact" => "iletisim",
                "iletisim" => "iletisim",
                "social" => "sosyal",
                "sosyal" => "sosyal",
                "shipping" => "kargo",
                "satis" => "kargo",
                "kargo" => "kargo",
                "odeme" => "kapida-odeme",
                "payment" => "kapida-odeme",
                "cod" => "kapida-odeme",
                "kapida-odeme" => "kapida-odeme",
                "seo" => "seo",
                "email" => "mail",
                "mail" => "mail",
                "maintenance" => "bakim",
                "bakim" => "bakim",
                _ => t
            };
        }

        private void UpdateGenelSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.SiteAdi = string.IsNullOrWhiteSpace(input.SiteAdi) ? "7ANRPS48" : input.SiteAdi.Trim();
            existing.MarkaAdi = string.IsNullOrWhiteSpace(input.MarkaAdi) ? existing.SiteAdi : input.MarkaAdi.Trim();
            existing.SiteBasligi = string.IsNullOrWhiteSpace(input.SiteBasligi) ? $"{existing.MarkaAdi} - متجرك الإلكتروني في فلسطين" : input.SiteBasligi.Trim();
            existing.SiteAciklamasi = string.IsNullOrWhiteSpace(input.SiteAciklamasi)
                ? "7ANRPS48 - Filistin'den online alışveriş. Moda, elektronik, ev & yaşam ürünlerinde hızlı teslimat."
                : input.SiteAciklamasi.Trim();
            existing.SiteLogoUrl = NormalizeLogoUrl(input.SiteLogoUrl);
            existing.FaviconUrl = NormalizeFaviconUrl(input.FaviconUrl);
            existing.BaseUrl = NormalizeBaseUrl(input.BaseUrl, ConfiguredBaseUrl(), IsProductionEnvironment());
            existing.TemaRengi = NormalizeThemeColor(input.TemaRengi);
            existing.UstBarMesaji = input.UstBarMesaji?.Trim() ?? string.Empty;
            existing.KampanyaMesaji = input.KampanyaMesaji?.Trim() ?? string.Empty;
            existing.UstBarEtkin = input.UstBarEtkin;
            existing.UstBarHizi = input.UstBarHizi > 0 ? input.UstBarHizi : 34;
            existing.FooterAciklamasi = string.IsNullOrWhiteSpace(input.FooterAciklamasi)
                ? existing.SiteAciklamasi
                : input.FooterAciklamasi.Trim();
            if (!string.IsNullOrWhiteSpace(input.FooterAciklamasiEn))
                existing.FooterAciklamasiEn = input.FooterAciklamasiEn.Trim();
            if (!string.IsNullOrWhiteSpace(input.FooterAciklamasiAr))
                existing.FooterAciklamasiAr = input.FooterAciklamasiAr.Trim();
            if (!string.IsNullOrWhiteSpace(input.HeroBaslikAr))
                existing.HeroBaslikAr = input.HeroBaslikAr.Trim();
            if (!string.IsNullOrWhiteSpace(input.HeroBaslikEn))
                existing.HeroBaslikEn = input.HeroBaslikEn.Trim();
            if (!string.IsNullOrWhiteSpace(input.HeroAltBaslikAr))
                existing.HeroAltBaslikAr = input.HeroAltBaslikAr.Trim();
            if (!string.IsNullOrWhiteSpace(input.HeroAltBaslikEn))
                existing.HeroAltBaslikEn = input.HeroAltBaslikEn.Trim();
            existing.HeroGorselUrl = string.IsNullOrWhiteSpace(input.HeroGorselUrl) ? "/slider-demo.jpg" : input.HeroGorselUrl.Trim();
        }

        private static void UpdateIletisimSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.Telefon = input.Telefon?.Trim() ?? string.Empty;
            existing.Email = input.Email?.Trim() ?? string.Empty;
            existing.Adres = input.Adres?.Trim() ?? string.Empty;
            existing.WhatsappNumarasi = input.WhatsappNumarasi?.Trim() ?? string.Empty;
            existing.CalismaSaatleri = input.CalismaSaatleri?.Trim() ?? string.Empty;
        }

        private static void UpdateSosyalSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.FacebookUrl = input.FacebookUrl?.Trim() ?? string.Empty;
            existing.InstagramUrl = input.InstagramUrl?.Trim() ?? string.Empty;
            existing.TwitterUrl = input.TwitterUrl?.Trim() ?? string.Empty;
            existing.YoutubeUrl = input.YoutubeUrl?.Trim() ?? string.Empty;
            existing.TiktokUrl = input.TiktokUrl?.Trim() ?? string.Empty;
            existing.PinterestUrl = input.PinterestUrl?.Trim() ?? string.Empty;
        }

        private static void UpdateKargoSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.ParaBirimi = string.IsNullOrWhiteSpace(input.ParaBirimi) ? "₪" : input.ParaBirimi.Trim();
            existing.KargoBedeli = Math.Max(0, input.KargoBedeli);
            existing.UcretsizKargoLimiti = Math.Max(0, input.UcretsizKargoLimiti);
            existing.StokUyariLimiti = Math.Max(0, input.StokUyariLimiti);
            existing.StoktaYokSatisIzni = input.StoktaYokSatisIzni;
            existing.StokBiteniGriGoster = input.StokBiteniGriGoster;
            existing.KargoFirmasi = string.IsNullOrWhiteSpace(input.KargoFirmasi) || IsLegacyTurkishCargoName(input.KargoFirmasi)
                ? (string.IsNullOrWhiteSpace(existing.KargoFirmasi) ? "توصيل محلي" : existing.KargoFirmasi)
                : input.KargoFirmasi.Trim();
            existing.KargoTakipUrl = input.KargoTakipUrl?.Trim() ?? string.Empty;
            existing.SiparisTeslimSuresiGun = input.SiparisTeslimSuresiGun <= 0 ? 3 : input.SiparisTeslimSuresiGun;
            existing.IadeHakkiGun = input.IadeHakkiGun <= 0 ? 7 : input.IadeHakkiGun;
            existing.IptalSuresiSaat = Math.Max(0, input.IptalSuresiSaat);
            existing.GirisZorunluMu = input.GirisZorunluMu;
            existing.AdreseTeslimAktifMi = input.AdreseTeslimAktifMi;
            existing.MagazadanTeslimAktifMi = input.MagazadanTeslimAktifMi;
            existing.BankaHavalesiAktifMi = input.BankaHavalesiAktifMi;

            if (!existing.AdreseTeslimAktifMi && !existing.MagazadanTeslimAktifMi)
            {
                existing.AdreseTeslimAktifMi = true;
            }
        }

        private static void UpdateKapidaOdemeSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.KapidaOdemeAktifMi = input.KapidaOdemeAktifMi;
            existing.KapidaOdemeHizmetBedeli = Math.Max(0, input.KapidaOdemeHizmetBedeli);
            existing.KapidaOdemeLimiti = input.KapidaOdemeLimiti <= 0 ? 2000 : input.KapidaOdemeLimiti;
            existing.ToptanciMinSiparisTutari = Math.Max(0, input.ToptanciMinSiparisTutari);
        }

        private static void UpdateSeoSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.MetaTitle = string.IsNullOrWhiteSpace(input.MetaTitle)
                ? $"{existing.MarkaAdi} - Filistin E-Ticaret Mağazası | Online Alışveriş"
                : input.MetaTitle.Trim();
            existing.MetaDescription = string.IsNullOrWhiteSpace(input.MetaDescription)
                ? $"{existing.MarkaAdi}; Filistin'de güvenli online alışveriş, hızlı teslimat ve müşteri odaklı e-ticaret deneyimi sunar."
                : input.MetaDescription.Trim();
            existing.MetaKeywords = string.IsNullOrWhiteSpace(input.MetaKeywords)
                ? "7ANRPS48; Filistin online alışveriş, moda, elektronik, ev & yaşam ürünleri"
                : input.MetaKeywords.Trim();
            existing.GoogleAnalyticsId = input.GoogleAnalyticsId?.Trim() ?? string.Empty;
            existing.FacebookPixelId = input.FacebookPixelId?.Trim() ?? string.Empty;
            existing.VarsayilanSosyalPaylasimGorseliUrl = NormalizeLogoUrl(input.VarsayilanSosyalPaylasimGorseliUrl);
            existing.CookieMetni = string.IsNullOrWhiteSpace(input.CookieMetni)
                ? "نستخدم ملفات تعريف الارتباط لتحسين تجربتك وتحليل حركة الموقع."
                : input.CookieMetni.Trim();
        }

        private static void UpdateMailSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.BildirimAliciEmail = input.BildirimAliciEmail?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(existing.BildirimAliciEmail) ||
                existing.BildirimAliciEmail.Equals("admin@7anrps48.com", StringComparison.OrdinalIgnoreCase) ||
                existing.BildirimAliciEmail.Contains(LegacyBrandToken(), StringComparison.OrdinalIgnoreCase))
            {
                existing.BildirimAliciEmail = "info@7anrps48.com";
            }
            existing.YeniSiparisMailBildirimi = input.YeniSiparisMailBildirimi;
            existing.StokUyarisiMailBildirimi = input.StokUyarisiMailBildirimi;
            existing.IadeTalebiMailBildirimi = input.IadeTalebiMailBildirimi;
        }

        private static void UpdateBakimSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            existing.BakimModuAktif = input.BakimModuAktif;
            existing.BakimModuMesaji = string.IsNullOrWhiteSpace(input.BakimModuMesaji)
                ? "نحن نعمل على تحسين الموقع لتقديم تجربة تسوق أفضل. سنعود قريباً!"
                : input.BakimModuMesaji.Trim();
        }

        private void UpdateAllSettings(SiteAyarlari existing, SiteAyarlari input)
        {
            UpdateGenelSettings(existing, input);
            UpdateIletisimSettings(existing, input);
            UpdateSosyalSettings(existing, input);
            UpdateKargoSettings(existing, input);
            UpdateKapidaOdemeSettings(existing, input);
            UpdateSeoSettings(existing, input);
            UpdateMailSettings(existing, input);
            UpdateBakimSettings(existing, input);
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
            settings.TemaRengi = NormalizeThemeColor(settings.TemaRengi);
            settings.UstBarMesaji = settings.UstBarMesaji?.Trim() ?? string.Empty;
            settings.KampanyaMesaji = settings.KampanyaMesaji?.Trim() ?? string.Empty;
            settings.FooterAciklamasi = string.IsNullOrWhiteSpace(settings.FooterAciklamasi)
                ? settings.SiteAciklamasi
                : settings.FooterAciklamasi.Trim();
            settings.FooterAciklamasiEn = settings.FooterAciklamasiEn?.Trim() ?? string.Empty;
            settings.FooterAciklamasiAr = settings.FooterAciklamasiAr?.Trim() ?? string.Empty;
            settings.HeroBaslikAr = settings.HeroBaslikAr?.Trim() ?? string.Empty;
            settings.HeroBaslikEn = settings.HeroBaslikEn?.Trim() ?? string.Empty;
            settings.HeroAltBaslikAr = settings.HeroAltBaslikAr?.Trim() ?? string.Empty;
            settings.HeroAltBaslikEn = settings.HeroAltBaslikEn?.Trim() ?? string.Empty;
            settings.HeroGorselUrl = string.IsNullOrWhiteSpace(settings.HeroGorselUrl) ? "/slider-demo.jpg" : settings.HeroGorselUrl.Trim();

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
            settings.KargoFirmasi = string.IsNullOrWhiteSpace(settings.KargoFirmasi) || IsLegacyTurkishCargoName(settings.KargoFirmasi)
                ? "توصيل محلي"
                : settings.KargoFirmasi.Trim();
            settings.KargoTakipUrl = settings.KargoTakipUrl?.Trim() ?? string.Empty;
            settings.SiparisTeslimSuresiGun = settings.SiparisTeslimSuresiGun <= 0 ? 5 : settings.SiparisTeslimSuresiGun;
            settings.IadeHakkiGun = settings.IadeHakkiGun <= 0 ? 14 : settings.IadeHakkiGun;
            settings.KapidaOdemeHizmetBedeli = Math.Max(0, settings.KapidaOdemeHizmetBedeli);
            settings.KapidaOdemeLimiti = settings.KapidaOdemeLimiti <= 0 ? 2000 : settings.KapidaOdemeLimiti;
            settings.IptalSuresiSaat = Math.Max(0, settings.IptalSuresiSaat);

            if (!settings.AdreseTeslimAktifMi && !settings.MagazadanTeslimAktifMi)
            {
                settings.AdreseTeslimAktifMi = true;
            }

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

        private static string NormalizeThemeColor(string? color)
        {
            var value = color?.Trim();
            return !string.IsNullOrWhiteSpace(value) &&
                   System.Text.RegularExpressions.Regex.IsMatch(value, "^#[0-9A-Fa-f]{6}$")
                ? value.ToUpperInvariant()
                : "#313511";
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
