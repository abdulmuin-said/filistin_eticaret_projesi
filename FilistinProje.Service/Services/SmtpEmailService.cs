using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Globalization;
using FilistinProje.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FilistinProje.Service.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(
            IConfiguration config,
            IWebHostEnvironment env,
            ISiteSettingsService siteSettingsService,
            ILogger<SmtpEmailService> logger)
        {
            _config = config;
            _env = env;
            _siteSettingsService = siteSettingsService;
            _logger = logger;
        }

        public async Task SendMailAsync(string to, string subject, string body)
        {
            await SendMailInternalAsync(to, subject, body);
        }

        private async Task SendMailInternalAsync(string to, string subject, string body, string? inlineLogoPath = null)
        {
            if (!_config.GetValue<bool>("EmailSettings:Enabled"))
            {
                throw new InvalidOperationException("E-posta gonderimi yapilandirmada devre disi.");
            }

            var host = _config["EmailSettings:Host"] ?? string.Empty;
            var port = int.TryParse(_config["EmailSettings:Port"], out var parsedPort) ? parsedPort : 587;
            var enableSsl = bool.TryParse(_config["EmailSettings:EnableSSL"], out var parsedSsl) ? parsedSsl : true;
            var username = _config["EmailSettings:Username"] ?? string.Empty;
            var password = _config["EmailSettings:Password"] ?? string.Empty;
            var siteSettings = _siteSettingsService.GetSettings();
            var brandName = string.IsNullOrWhiteSpace(siteSettings.MarkaAdi) ? siteSettings.SiteAdi : siteSettings.MarkaAdi;
            var fromEmail = _config["EmailSettings:FromEmail"];
            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                fromEmail = siteSettings.Email;
            }
            var fromName = _config["EmailSettings:FromName"];
            if (string.IsNullOrWhiteSpace(fromName))
            {
                fromName = brandName;
            }

            if (!TryCreateMailAddress(fromEmail, fromName, out var fromAddress) || string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("SMTP e-posta ayarlari eksik oldugu icin mail gonderimi atlandi. Subject={Subject}", subject);
                throw new InvalidOperationException("SMTP ayarlari eksik. Sunucu, kullanici, parola ve gonderici e-posta adresini kontrol edin.");
            }

            if (!TryCreateMailAddress(to, null, out var toAddress))
            {
                _logger.LogWarning("Gecersiz alici e-posta adresi nedeniyle mail gonderimi atlandi. To={To}, Subject={Subject}", to, subject);
                throw new InvalidOperationException("Alici e-posta adresi gecersiz.");
            }

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                Timeout = 30000
            };

            using var mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            if (!string.IsNullOrWhiteSpace(inlineLogoPath) && File.Exists(inlineLogoPath))
            {
                var htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                var logoResource = new LinkedResource(inlineLogoPath, "image/png")
                {
                    ContentId = "7anrps48-logo",
                    TransferEncoding = TransferEncoding.Base64
                };

                logoResource.ContentType.Name = "7anrps48-logo.png";
                logoResource.ContentLink = new Uri("cid:7anrps48-logo");
                htmlView.LinkedResources.Add(logoResource);
                mailMessage.AlternateViews.Add(htmlView);
            }

            mailMessage.To.Add(toAddress);
            await SendWithBoundedRetryAsync(client, mailMessage);
        }

        private async Task SendWithBoundedRetryAsync(SmtpClient client, MailMessage mailMessage)
        {
            const int maxAttempts = 3;
            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await client.SendMailAsync(mailMessage).WaitAsync(TimeSpan.FromSeconds(30));
                    return;
                }
                catch (Exception ex) when (
                    attempt < maxAttempts &&
                    (ex is SmtpException || ex is TimeoutException))
                {
                    _logger.LogWarning(
                        "SMTP gonderimi gecici olarak basarisiz. Deneme={Attempt}/{MaxAttempts}, HataTuru={ErrorType}",
                        attempt,
                        maxAttempts,
                        ex.GetType().Name);
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                }
            }
        }

        public async Task SendTemplateMailAsync(string to, string baslik, string adSoyad, string icerik, string btnLink = "", string btnYazi = "", string culture = "")
        {
            var siteSettings = _siteSettingsService.GetSettings();
            var brandName = string.IsNullOrWhiteSpace(siteSettings.MarkaAdi) ? siteSettings.SiteAdi : siteSettings.MarkaAdi;
            var siteUrl = _siteSettingsService.BuildAbsoluteUrl(string.Empty);
            var logoUrl = "cid:7anrps48-logo";
            var inlineLogoPath = Path.Combine(_env.WebRootPath, "EmailTemplates", "7anrps48-email-logo.png");
            var instagramUrl = string.IsNullOrWhiteSpace(siteSettings.InstagramUrl) ? siteUrl : siteSettings.InstagramUrl;
            var contactSeparator = !string.IsNullOrWhiteSpace(siteSettings.Email) && !string.IsNullOrWhiteSpace(siteSettings.Telefon)
                ? "|"
                : string.Empty;

            var normalizedCulture = culture.Trim().ToLowerInvariant();
            if (normalizedCulture != "ar" && normalizedCulture != "en")
            {
                normalizedCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en" ? "en" : "ar";
            }

            var sablonDosyasi = normalizedCulture switch
            {
                "ar" => "Sablon.ar.html",
                _ => "Sablon.en.html"
            };
            var path = Path.Combine(_env.WebRootPath, "EmailTemplates", sablonDosyasi);
            var body = await File.ReadAllTextAsync(path);

            var butonGorunum = string.IsNullOrEmpty(btnLink) ? "none" : "table-row";

            body = body.Replace("{BASLIK}", baslik)
                       .Replace("{ADSOYAD}", adSoyad)
                       .Replace("{ICERIK}", icerik)
                       .Replace("{BUTON_GORUNUM}", butonGorunum)
                       .Replace("{BUTON_LINK}", btnLink)
                       .Replace("{BUTON_YAZI}", btnYazi)
                       .Replace("{SITE_ADI}", siteSettings.SiteAdi)
                       .Replace("{MARKA_ADI}", brandName)
                       .Replace("{SITE_URL}", siteUrl)
                       .Replace("{SITE_LOGO_URL}", logoUrl)
                       .Replace("{SITE_EMAIL}", siteSettings.Email)
                       .Replace("{SITE_PHONE}", siteSettings.Telefon)
                       .Replace("{SITE_CONTACT_SEPARATOR}", contactSeparator)
                       .Replace("{INSTAGRAM_URL}", instagramUrl);

            await SendMailInternalAsync(to, baslik, body, inlineLogoPath);
        }

        public async Task<bool> SendKargoNotificationEmail(string toEmail, string musteriAdi, string siparisNo, string kargoFirmasi, string kargoTakipNo)
        {
            try
            {
                var subject = $"تم شحن طلبك - {siparisNo}";
                var safeSiparisNo = WebUtility.HtmlEncode(siparisNo);
                var safeKargoFirmasi = WebUtility.HtmlEncode(kargoFirmasi);
                var safeKargoTakipNo = WebUtility.HtmlEncode(kargoTakipNo);
                var trackingLinkHtml = GetKargoTrackingLink(kargoTakipNo);
                var trackingLinkBlock = string.IsNullOrWhiteSpace(trackingLinkHtml)
                    ? string.Empty
                    : $"<p>يمكنك تتبع شحنتك عبر الرابط أدناه.</p><div style='text-align:center; margin:24px 0;'>{trackingLinkHtml}</div>";
                var content = $@"
                    <p>تم شحن طلبك رقم <strong>{safeSiparisNo}</strong>.</p>
                    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:12px; background:#fffaf0; margin:18px 0;'>
                        <tr>
                            <td style='padding:16px; border-bottom:1px solid #e5e2dc; color:#47473d;'>
                                <strong style='color:#313511;'>شركة الشحن:</strong> {safeKargoFirmasi}
                            </td>
                        </tr>
                        <tr>
                            <td style='padding:16px; color:#47473d;'>
                                <strong style='color:#313511;'>رقم التتبع:</strong> <span style='font-size:18px; color:#b58735; font-weight:700;'>{safeKargoTakipNo}</span>
                            </td>
                        </tr>
                    </table>
                    {trackingLinkBlock}";

                await SendTemplateMailAsync(toEmail, subject, musteriAdi, content, "", "", "ar");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kargo email gonderim hatasi. SiparisNo={SiparisNo}", siparisNo);
                return false;
            }
        }

        private string GetKargoTrackingLink(string takipNo)
        {
            var siteSettings = _siteSettingsService.GetSettings();
            var trackingUrlTemplate = siteSettings.KargoTakipUrl?.Trim();
            if (string.IsNullOrWhiteSpace(trackingUrlTemplate) || string.IsNullOrWhiteSpace(takipNo))
            {
                return string.Empty;
            }

            var encodedTakipNo = Uri.EscapeDataString(takipNo ?? string.Empty);
            var trackingUrl = trackingUrlTemplate
                .Replace("{TRACKING_NO}", encodedTakipNo, StringComparison.OrdinalIgnoreCase)
                .Replace("{takipNo}", encodedTakipNo, StringComparison.OrdinalIgnoreCase)
                .Replace("{code}", encodedTakipNo, StringComparison.OrdinalIgnoreCase);
            if (trackingUrl == trackingUrlTemplate)
            {
                var separator = trackingUrl.Contains("?", StringComparison.Ordinal) ? "&" : "?";
                trackingUrl = $"{trackingUrl}{separator}trackingNo={encodedTakipNo}";
            }

            if (!Uri.TryCreate(trackingUrl, UriKind.Absolute, out var trackingUri) ||
                (trackingUri.Scheme != Uri.UriSchemeHttps && trackingUri.Scheme != Uri.UriSchemeHttp))
            {
                _logger.LogWarning("Gecersiz kargo takip URL sablonu nedeniyle takip linki gosterilmedi. Template={Template}", trackingUrlTemplate);
                return string.Empty;
            }

            var safeTrackingUrl = WebUtility.HtmlEncode(trackingUri.ToString());
            return $"<a href='{safeTrackingUrl}' style='display:inline-block; background:#313511; color:#ffffff; padding:13px 24px; text-decoration:none; border-radius:999px; font-size:12px; font-weight:700; letter-spacing:.08em;'>تتبع الشحنة</a>";
        }

        private static bool TryCreateMailAddress(string? address, string? displayName, out MailAddress mailAddress)
        {
            mailAddress = null!;
            if (string.IsNullOrWhiteSpace(address))
            {
                return false;
            }

            try
            {
                mailAddress = string.IsNullOrWhiteSpace(displayName)
                    ? new MailAddress(address.Trim())
                    : new MailAddress(address.Trim(), displayName.Trim());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task<bool> SendInvoiceEmailAsync(string toEmail, string musteriAdi, string siparisNo, string filePath)
        {
            try
            {
                if (!_config.GetValue<bool>("EmailSettings:Enabled"))
                {
                    _logger.LogWarning("E-posta devre disi oldugu icin fatura maili gonderilmedi.");
                    return false;
                }

                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Fatura dosyasi bulunamadi: {FilePath}", filePath);
                    return false;
                }

                var siteSettings = _siteSettingsService.GetSettings();
                var brandName = string.IsNullOrWhiteSpace(siteSettings.MarkaAdi) ? siteSettings.SiteAdi : siteSettings.MarkaAdi;
                var fromEmail = _config["EmailSettings:FromEmail"];
                if (string.IsNullOrWhiteSpace(fromEmail)) fromEmail = siteSettings.Email;
                var fromName = _config["EmailSettings:FromName"];
                if (string.IsNullOrWhiteSpace(fromName)) fromName = brandName;

                var host = _config["EmailSettings:Host"] ?? string.Empty;
                var port = int.TryParse(_config["EmailSettings:Port"], out var parsedPort) ? parsedPort : 587;
                var enableSsl = bool.TryParse(_config["EmailSettings:EnableSSL"], out var parsedSsl) ? parsedSsl : true;
                var username = _config["EmailSettings:Username"] ?? string.Empty;
                var password = _config["EmailSettings:Password"] ?? string.Empty;

                if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    _logger.LogWarning("SMTP ayarlari eksik - fatura maili gonderilemedi");
                    return false;
                }

                if (!TryCreateMailAddress(fromEmail, fromName, out var fromAddress) || !TryCreateMailAddress(toEmail, null, out var toAddress))
                {
                    _logger.LogWarning("Gecersiz e-posta adresi - fatura maili gonderilemedi");
                    return false;
                }

                var safeSiparisNo = WebUtility.HtmlEncode(siparisNo);
                var subject = $"فاتورة طلبك جاهزة - {siparisNo}";
                var content = $@"
                    <p>تم إعداد فاتورة طلبك رقم <strong>{safeSiparisNo}</strong>. يمكنك تنزيل الفاتورة من المرفق أدناه.</p>
                    <p style='color:#7a766a; font-size:14px;'>إذا كان لديك أي استفسار، لا تتردد في التواصل معنا.</p>";

                var siteUrl = _siteSettingsService.BuildAbsoluteUrl(string.Empty);
                var logoUrl = "cid:7anrps48-logo";
                var inlineLogoPath = Path.Combine(_env.WebRootPath, "EmailTemplates", "7anrps48-email-logo.png");
                var instagramUrl = string.IsNullOrWhiteSpace(siteSettings.InstagramUrl) ? siteUrl : siteSettings.InstagramUrl;
                var contactSeparator = !string.IsNullOrWhiteSpace(siteSettings.Email) && !string.IsNullOrWhiteSpace(siteSettings.Telefon) ? "|" : string.Empty;

                var sablonPath = Path.Combine(_env.WebRootPath, "EmailTemplates", "Sablon.ar.html");
                var body = await File.ReadAllTextAsync(sablonPath);
                body = body.Replace("{BASLIK}", subject)
                           .Replace("{ADSOYAD}", WebUtility.HtmlEncode(musteriAdi))
                           .Replace("{ICERIK}", content)
                           .Replace("{BUTON_GORUNUM}", "none")
                           .Replace("{BUTON_LINK}", "")
                           .Replace("{BUTON_YAZI}", "")
                           .Replace("{SITE_ADI}", siteSettings.SiteAdi)
                           .Replace("{MARKA_ADI}", brandName)
                           .Replace("{SITE_URL}", siteUrl)
                           .Replace("{SITE_LOGO_URL}", logoUrl)
                           .Replace("{SITE_EMAIL}", siteSettings.Email)
                           .Replace("{SITE_PHONE}", siteSettings.Telefon)
                           .Replace("{SITE_CONTACT_SEPARATOR}", contactSeparator)
                           .Replace("{INSTAGRAM_URL}", instagramUrl);

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl,
                    Timeout = 30000
                };

                using var mailMessage = new MailMessage { From = fromAddress, Subject = subject, IsBodyHtml = true };
                mailMessage.To.Add(toAddress);

                if (File.Exists(inlineLogoPath))
                {
                    var htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                    var logoResource = new LinkedResource(inlineLogoPath, "image/png") { ContentId = "7anrps48-logo", TransferEncoding = TransferEncoding.Base64 };
                    logoResource.ContentType.Name = "7anrps48-logo.png";
                    logoResource.ContentLink = new Uri("cid:7anrps48-logo");
                    htmlView.LinkedResources.Add(logoResource);
                    mailMessage.AlternateViews.Add(htmlView);
                }
                else
                {
                    mailMessage.Body = body;
                }

                var attachment = new Attachment(filePath, "application/pdf");
                if (attachment.ContentDisposition != null)
                {
                    attachment.ContentDisposition.Inline = false;
                    attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                }
                mailMessage.Attachments.Add(attachment);

                await SendWithBoundedRetryAsync(client, mailMessage);
                _logger.LogInformation("Fatura maili basariyla gonderildi. SiparisNo={SiparisNo}, To={To}", siparisNo, toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatura maili gonderim hatasi. SiparisNo={SiparisNo}", siparisNo);
                return false;
            }
        }
    }
}
