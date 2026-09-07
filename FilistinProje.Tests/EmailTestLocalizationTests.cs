using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Models;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Areas.Admin.Controllers;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace FilistinProje.Tests
{
    public sealed class EmailTestLocalizationTests
    {
        [Fact]
        public void EmailDisabledError_ConstantValue_IsEmailDisabled()
        {
            Assert.Equal("EMAIL_DISABLED", SmtpEmailService.EmailDisabledError);
        }

        [Fact]
        public async Task SmtpEmailService_WhenDisabled_ThrowsInvalidOperationExceptionWithEmailDisabledConstant()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                ["EmailSettings:Enabled"] = "false",
                ["EmailSettings:Host"] = "smtp.example.com",
                ["EmailSettings:Port"] = "587",
                ["EmailSettings:Username"] = "user@example.com",
                ["EmailSettings:Password"] = "password",
                ["EmailSettings:FromEmail"] = "noreply@example.com"
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var service = new SmtpEmailService(
                configuration,
                null!,
                null!,
                NullLogger<SmtpEmailService>.Instance);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.SendMailAsync("test@example.com", "Subject", "Body"));

            Assert.Equal(SmtpEmailService.EmailDisabledError, exception.Message);
            Assert.DoesNotContain("devre", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void SharedResourceFiles_ContainTestMailDisabledKeysInEnglishAndArabic()
        {
            var enResxPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "FilistinProje.Web", "Resources", "SharedResource.en.resx"));
            var arResxPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "FilistinProje.Web", "Resources", "SharedResource.ar.resx"));

            Assert.True(File.Exists(enResxPath), $"English resx not found at {enResxPath}");
            Assert.True(File.Exists(arResxPath), $"Arabic resx not found at {arResxPath}");

            var enDoc = XDocument.Load(enResxPath);
            var arDoc = XDocument.Load(arResxPath);

            var enAdminVal = enDoc.Root?.Elements("data")
                .FirstOrDefault(e => (string?)e.Attribute("name") == "Admin_TestMailDisabled")
                ?.Element("value")?.Value;

            var enVal = enDoc.Root?.Elements("data")
                .FirstOrDefault(e => (string?)e.Attribute("name") == "TestMailDisabled")
                ?.Element("value")?.Value;

            var arAdminVal = arDoc.Root?.Elements("data")
                .FirstOrDefault(e => (string?)e.Attribute("name") == "Admin_TestMailDisabled")
                ?.Element("value")?.Value;

            var arVal = arDoc.Root?.Elements("data")
                .FirstOrDefault(e => (string?)e.Attribute("name") == "TestMailDisabled")
                ?.Element("value")?.Value;

            Assert.False(string.IsNullOrWhiteSpace(enAdminVal));
            Assert.False(string.IsNullOrWhiteSpace(enVal));
            Assert.False(string.IsNullOrWhiteSpace(arAdminVal));
            Assert.False(string.IsNullOrWhiteSpace(arVal));

            Assert.Equal("Email sending is disabled in configuration.", enAdminVal);
            Assert.Equal("Email sending is disabled in configuration.", enVal);
            Assert.Equal("تم تعطيل إرسال البريد الإلكتروني في الإعدادات.", arAdminVal);
            Assert.Equal("تم تعطيل إرسال البريد الإلكتروني في الإعدادات.", arVal);
        }

        [Fact]
        public async Task AyarlarController_TestMail_WhenEmailServiceThrowsEmailDisabled_SetsLocalizedMessageInTempData()
        {
            var inMemoryConfig = new Dictionary<string, string?>
            {
                ["EmailSettings:Host"] = "smtp.example.com",
                ["EmailSettings:Username"] = "sender@example.com",
                ["EmailSettings:Password"] = "secret",
                ["EmailSettings:FromEmail"] = "sender@example.com"
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .Build();

            var emailServiceMock = new DisabledEmailServiceStub();
            var localizerMock = new TestLocalizerStub();

            var options = new DbContextOptionsBuilder<KanvasDbContext>()
                .UseInMemoryDatabase(databaseName: "EmailTestDb_" + Guid.NewGuid())
                .Options;
            using var context = new KanvasDbContext(options);

            var controller = new AyarlarController(
                null!,
                emailServiceMock,
                configuration,
                context,
                localizerMock);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, new TestTempDataProvider());
            controller.TempData = tempData;

            var model = new SiteAyarlari
            {
                Email = "admin@example.com",
                BildirimAliciEmail = "recipient@example.com",
                MarkaAdi = "TestBrand"
            };

            var result = await controller.TestMail(model);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(AyarlarController.Index), redirect.ActionName);

            Assert.Equal("Email sending is disabled in configuration.", controller.TempData["Hata"]);
            Assert.Equal("warning", controller.TempData["Durum"]);
        }

        private sealed class DisabledEmailServiceStub : IEmailService
        {
            public Task SendMailAsync(string to, string subject, string body)
            {
                throw new InvalidOperationException(SmtpEmailService.EmailDisabledError);
            }

            public Task SendTemplateMailAsync(string to, string baslik, string adSoyad, string icerik, string btnLink = "", string btnYazi = "", string culture = "")
            {
                throw new InvalidOperationException(SmtpEmailService.EmailDisabledError);
            }

            public Task<bool> SendKargoNotificationEmail(string toEmail, string musteriAdi, string siparisNo, string kargoFirmasi, string kargoTakipNo)
            {
                return Task.FromResult(false);
            }

            public Task<bool> SendInvoiceEmailAsync(string toEmail, string musteriAdi, string siparisNo, string filePath)
            {
                return Task.FromResult(false);
            }
        }

        private sealed class TestLocalizerStub : IStringLocalizer<SharedResource>
        {
            private readonly Dictionary<string, string> _values = new()
            {
                ["Admin_TestMailSubject"] = "Test Subject",
                ["Admin_TestMailBody"] = "Test Body",
                ["Admin_TestMailSuccess"] = "Success {0}",
                ["Admin_TestMailTimeout"] = "Timeout",
                ["Admin_TestMailFailed"] = "Failed: {0}",
                ["Admin_TestMailDisabled"] = "Email sending is disabled in configuration.",
                ["TestMailDisabled"] = "Email sending is disabled in configuration."
            };

            public LocalizedString this[string name]
            {
                get
                {
                    var found = _values.TryGetValue(name, out var val);
                    return new LocalizedString(name, val ?? name, !found);
                }
            }

            public LocalizedString this[string name, params object[] arguments]
            {
                get
                {
                    var found = _values.TryGetValue(name, out var val);
                    return new LocalizedString(name, val != null ? string.Format(val, arguments) : name, !found);
                }
            }

            public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            {
                return _values.Select(kv => new LocalizedString(kv.Key, kv.Value, false));
            }
        }

        private sealed class TestTempDataProvider : ITempDataProvider
        {
            private readonly Dictionary<string, object> _data = new();

            public IDictionary<string, object> LoadTempData(HttpContext context)
            {
                return _data;
            }

            public void SaveTempData(HttpContext context, IDictionary<string, object> values)
            {
                foreach (var kvp in values)
                {
                    _data[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
