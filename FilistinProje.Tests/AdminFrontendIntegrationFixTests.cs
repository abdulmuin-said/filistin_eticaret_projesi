using FilistinProje.Core.DTOs;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service;
using FilistinProje.Service.Helpers;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Controllers;
using FilistinProje.Web.Helpers;
using FilistinProje.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace FilistinProje.Tests
{
    public sealed class AdminFrontendIntegrationFixTests
    {
        private static KanvasDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<KanvasDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new KanvasDbContext(options);
        }

        // --- 1. KAMPANYA TARİH VE ETKİN FİYAT TESTLERİ ---

        [Fact]
        public void EtkinFiyat_FutureCampaignDate_ReturnsDiscountedPrice()
        {
            var urun = new Urun
            {
                Fiyat = 100m,
                IndirimliFiyat = 80m,
                KampanyaBitisTarihi = DateTime.UtcNow.AddDays(2)
            };

            Assert.True(urun.IndirimVarMi);
            Assert.Equal(80m, urun.EtkinFiyat);
        }

        [Fact]
        public void EtkinFiyat_ExpiredCampaignDate_ReturnsOriginalPrice()
        {
            var urun = new Urun
            {
                Fiyat = 100m,
                IndirimliFiyat = 80m,
                KampanyaBitisTarihi = DateTime.UtcNow.AddDays(-1)
            };

            Assert.False(urun.IndirimVarMi);
            Assert.Equal(100m, urun.EtkinFiyat);
        }

        [Fact]
        public void EtkinFiyat_NullCampaignDate_ReturnsDiscountedPrice()
        {
            var urun = new Urun
            {
                Fiyat = 100m,
                IndirimliFiyat = 75m,
                KampanyaBitisTarihi = null
            };

            Assert.True(urun.IndirimVarMi);
            Assert.Equal(75m, urun.EtkinFiyat);
        }

        [Fact]
        public void ToBadges_CampaignWithoutEndDate_ShowsCampaignBadge()
        {
            var urun = new Urun
            {
                KampanyaliMi = true,
                KampanyaBitisTarihi = null,
                StokDurumu = "Stokta"
            };

            Assert.Contains(urun.ToBadges(), badge => badge.LocalizasyonKey == "Badge_Campaign");
        }

        [Fact]
        public void ToBadges_AdminMarkedNewProduct_ShowsNewBadge()
        {
            var urun = new Urun
            {
                YeniUrunMu = true,
                SatisSayisi = 50,
                GoruntulenmeSayisi = 1000,
                StokDurumu = "Stokta"
            };

            Assert.Contains(urun.ToBadges(), badge => badge.LocalizasyonKey == "Badge_NewProduct");
        }

        // --- 2. SEPET BİRLEŞTİRME GERÇEK SERVİS TESTLERİ (POST-AUDIT-001) ---

        [Fact]
        public async Task MergeSepetler_Anonymous3_User3_Max5_RejectsMerge()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            var sepetService = new SepetService(db, NullLogger<SepetService>.Instance);

            var urun = new Urun
            {
                Id = 1,
                Baslik = "Test Urun Max 5",
                Fiyat = 50m,
                AktifMi = true,
                SilindiMi = false,
                MaxSiparisAdedi = 5,
                MinSiparisAdedi = 1,
                StokDurumu = "Stokta"
            };
            db.Urunler.Add(urun);

            var anonCart = new Sepet { Id = 1, SessionId = "anon-123", SilindiMi = false };
            anonCart.SepetItems.Add(new SepetItem { Id = 101, SepetId = 1, UrunId = 1, Adet = 3, Fiyat = 50m, SilindiMi = false, CerceveModeli = "Çerçevesiz" });

            var userCart = new Sepet { Id = 2, AppUserId = "user-456", SilindiMi = false };
            userCart.SepetItems.Add(new SepetItem { Id = 102, SepetId = 2, UrunId = 1, Adet = 3, Fiyat = 50m, SilindiMi = false, CerceveModeli = "Çerçevesiz" });

            db.Sepetler.AddRange(anonCart, userCart);
            await db.SaveChangesAsync();

            var result = await sepetService.MergeSepetlerDetailedAsync("anon-123", "user-456");

            Assert.False(result.Basarili);
            Assert.Equal("Sepet_MaxSiparisAdediAsildi", result.MessageKey);

            // User cart must remain unchanged (3 items)
            var finalUserCart = await db.Sepetler.Include(s => s.SepetItems).FirstAsync(s => s.AppUserId == "user-456");
            Assert.Equal(3, finalUserCart.SepetItems.First(i => !i.SilindiMi).Adet);
        }

        [Fact]
        public async Task MergeSepetler_Anonymous2_User3_Max5_AcceptsMerge()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            var sepetService = new SepetService(db, NullLogger<SepetService>.Instance);

            var urun = new Urun
            {
                Id = 2,
                Baslik = "Test Urun Max 5",
                Fiyat = 50m,
                AktifMi = true,
                SilindiMi = false,
                MaxSiparisAdedi = 5,
                MinSiparisAdedi = 1,
                StokDurumu = "Stokta"
            };
            db.Urunler.Add(urun);

            var anonCart = new Sepet { Id = 10, SessionId = "anon-789", SilindiMi = false };
            anonCart.SepetItems.Add(new SepetItem { Id = 201, SepetId = 10, UrunId = 2, Adet = 2, Fiyat = 50m, SilindiMi = false, CerceveModeli = "Çerçevesiz" });

            var userCart = new Sepet { Id = 11, AppUserId = "user-789", SilindiMi = false };
            userCart.SepetItems.Add(new SepetItem { Id = 202, SepetId = 11, UrunId = 2, Adet = 3, Fiyat = 50m, SilindiMi = false, CerceveModeli = "Çerçevesiz" });

            db.Sepetler.AddRange(anonCart, userCart);
            await db.SaveChangesAsync();

            var result = await sepetService.MergeSepetlerDetailedAsync("anon-789", "user-789");

            Assert.True(result.Basarili);

            var finalUserCart = await db.Sepetler.Include(s => s.SepetItems).FirstAsync(s => s.AppUserId == "user-789");
            Assert.Equal(5, finalUserCart.SepetItems.First(i => !i.SilindiMi).Adet);
        }

        [Fact]
        public async Task MergeSepetler_InactiveProduct_RejectsMerge()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            var sepetService = new SepetService(db, NullLogger<SepetService>.Instance);

            var urun = new Urun
            {
                Id = 3,
                Baslik = "Pasif Urun",
                Fiyat = 50m,
                AktifMi = false, // Inactive!
                SilindiMi = false,
                MaxSiparisAdedi = 10
            };
            db.Urunler.Add(urun);

            var anonCart = new Sepet { Id = 20, SessionId = "anon-pasif", SilindiMi = false };
            anonCart.SepetItems.Add(new SepetItem { Id = 301, SepetId = 20, UrunId = 3, Adet = 1, Fiyat = 50m, SilindiMi = false, CerceveModeli = "Çerçevesiz" });

            var userCart = new Sepet { Id = 21, AppUserId = "user-pasif", SilindiMi = false };
            db.Sepetler.AddRange(anonCart, userCart);
            await db.SaveChangesAsync();

            var result = await sepetService.MergeSepetlerDetailedAsync("anon-pasif", "user-pasif");

            Assert.False(result.Basarili);
            Assert.Equal("Sepet_ProductUnavailable", result.MessageKey);
        }

        // --- 3. KURUMSAL SAYFA PASİFLİK VE XSS TESTLERİ ---

        [Fact]
        public async Task Kurumsal_DeletedPage_ReturnsNotFound()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            db.KurumsalSayfalar.Add(new KurumsalSayfa
            {
                Id = 1,
                Baslik = "Gizlilik Politikasi",
                UrlSlug = "gizlilik",
                Icerik = "<p>Gizlilik metni</p>",
                SilindiMi = true // Deleted/Inactive!
            });
            await db.SaveChangesAsync();

            var controller = new KurumsalController(db, null!, null!, NullLogger<KurumsalController>.Instance, null!);
            var actionResult = await controller.Detay("gizlilik");

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void HtmlSanitizer_NeutralizesScriptsAndEventHandlers()
        {
            var dirtyHtml = "<script>alert('xss')</script><img src=\"x\" onerror=\"alert('xss')\"><a href=\"javascript:alert('xss')\">Click</a>";
            var sanitized = HtmlSanitizerHelper.Sanitize(dirtyHtml);

            Assert.DoesNotContain("<script>", sanitized, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("onerror", sanitized, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("javascript:", sanitized, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void HtmlSanitizer_PreservesSafeFormatting()
        {
            var safeHtml = "<h1>Baslik</h1><p>Metin <b>Kalin</b> ve <i>Italik</i></p>";
            var sanitized = HtmlSanitizerHelper.Sanitize(safeHtml);

            Assert.Equal(safeHtml, sanitized);
        }

        // --- 4. KAMPANYA VE DİL/ZAMAN DİLİMİ TESTLERİ ---

        [Fact]
        public void BusinessTimeZone_ConvertLocalToUtc_HandlesTimezone()
        {
            var localTime = new DateTime(2026, 7, 25, 20, 0, 0, DateTimeKind.Unspecified);
            var utcTime = BusinessTimeZoneService.ConvertStoreLocalToUtc(localTime);

            Assert.NotNull(utcTime);
            Assert.Equal(DateTimeKind.Utc, utcTime!.Value.Kind);

            var roundtripLocal = BusinessTimeZoneService.ConvertUtcToStoreLocal(utcTime);
            Assert.NotNull(roundtripLocal);
            Assert.Equal(localTime.Hour, roundtripLocal!.Value.Hour);
        }

        // --- 5. TEMA RENGI REGEX TESTLERİ ---

        [Fact]
        public void ThemeColor_ValidHex_IsAccepted()
        {
            var themeColor = "#313511";
            var isValid = Regex.IsMatch(themeColor, @"^#[0-9A-Fa-f]{6}$");
            Assert.True(isValid);
        }

        [Fact]
        public void ThemeColor_CssInjectionAttempt_IsRejected()
        {
            var maliciousInput = "</style><script>alert(1)</script>";
            var isValid = Regex.IsMatch(maliciousInput, @"^#[0-9A-Fa-f]{6}$");
            Assert.False(isValid);
        }

        // --- 6. ÇERÇEVE FİYATI SIFIRLANMA KONTROLÜ ---

        [Fact]
        public async Task OrderPricingService_FramePriceCalculation_ReturnsZero()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            var pricingService = new OrderPricingService(db, null!, null!, NullLogger<OrderPricingService>.Instance);

            var urun = new Urun
            {
                Id = 10,
                Baslik = "Kanvas Tablo",
                Fiyat = 200m,
                AktifMi = true,
                SilindiMi = false
            };
            db.Urunler.Add(urun);
            await db.SaveChangesAsync();

            var cartItems = new List<SepetItem>
            {
                new SepetItem
                {
                    Id = 50,
                    UrunId = 10,
                    Adet = 1,
                    Fiyat = 200m,
                    CerceveModeli = "Siyah" // Çerçeve seçilse bile ekstra ücret 0 olmalı!
                }
            };

            var result = await pricingService.HesaplaAsync(cartItems, null, "BankaHavalesi", false, null);

            Assert.Single(result.Satirlar);
            Assert.Equal(200m, result.Satirlar[0].BirimFiyat); // 200 + 0 = 200m!
            Assert.Equal(200m, result.AraToplam);
        }

        [Fact]
        public async Task OrderPricingService_WholesaleTier_UsesHighestReachedQuantityPrice()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            var pricingService = new OrderPricingService(db, null!, null!, NullLogger<OrderPricingService>.Instance);
            var urun = new Urun { Id = 20, Baslik = "Toptan Urun", Fiyat = 100m, TopFiyat = 90m, AktifMi = true, StokDurumu = "Stokta" };
            urun.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi { MinAdet = 10, BirimFiyat = 80m, AktifMi = true });
            urun.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi { MinAdet = 25, BirimFiyat = 70m, AktifMi = true });
            db.Urunler.Add(urun);
            await db.SaveChangesAsync();

            var result = await pricingService.HesaplaAsync(
                new List<SepetItem> { new() { Id = 60, UrunId = 20, Adet = 30, Fiyat = 90m } },
                null, "BankaHavalesi", true, null);

            Assert.Single(result.Satirlar);
            Assert.Equal(70m, result.Satirlar[0].BirimFiyat);
        }

        [Fact]
        public async Task OrderPricingService_VariantWholesaleTier_OverridesProductTier()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());
            var pricingService = new OrderPricingService(db, null!, null!, NullLogger<OrderPricingService>.Instance);
            var urun = new Urun { Id = 21, Baslik = "Varyantli Toptan Urun", Fiyat = 100m, AktifMi = true, StokDurumu = "Stokta" };
            var variant = new UrunSecenek { Id = 210, UrunId = 21, Olcu = "500", OlcuBirimi = "ml", SatisFiyati = 95m, StokAdedi = 100, AktifMi = true };
            urun.UrunSecenek.Add(variant);
            urun.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi { MinAdet = 10, BirimFiyat = 82m, AktifMi = true });
            urun.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi { UrunSecenekId = 210, MinAdet = 10, BirimFiyat = 76m, AktifMi = true });
            db.Urunler.Add(urun);
            await db.SaveChangesAsync();

            var result = await pricingService.HesaplaAsync(
                new List<SepetItem> { new() { Id = 61, UrunId = 21, UrunSecenekId = 210, Adet = 12, Fiyat = 95m } },
                null, "BankaHavalesi", true, null);

            Assert.Single(result.Satirlar);
            Assert.Equal(76m, result.Satirlar[0].BirimFiyat);
        }

        [Fact]
        public void LoginPost_AcceptsAccountAliasUsedByLoginForm()
        {
            var method = typeof(HesapController).GetMethod(
                nameof(HesapController.GirisYap),
                new[] { typeof(string), typeof(string), typeof(string) });

            Assert.NotNull(method);
            var postRoutes = method!
                .GetCustomAttributes(typeof(HttpPostAttribute), inherit: false)
                .Cast<HttpPostAttribute>()
                .Select(attribute => attribute.Template);

            Assert.Contains("/account/GirisYap", postRoutes);
        }

        [Fact]
        public void ToBadges_UsesProductSpecificCampaignColor()
        {
            var urun = new Urun
            {
                StokDurumu = "Stokta",
                KampanyaliMi = true,
                KampanyaEtiketRengi = "#123456"
            };

            var badge = Assert.Single(urun.ToBadges(), x => x.LocalizasyonKey == "Badge_Campaign");

            Assert.Equal("#123456", badge.ArkaPlanRengi);
            Assert.Equal("#FFFFFF", badge.YaziRengi);
        }

        [Fact]
        public void ProductBadge_LightBackground_UsesDarkReadableText()
        {
            var badge = new ProductBadge("", "product-badge--featured", 1, "Badge_Featured", "#F4D99F");

            Assert.Equal("#18231B", badge.YaziRengi);
        }

        [Theory]
        [InlineData("+970 599 123 456", "+970599123456")]
        [InlineData("00970 599 123 456", "+970599123456")]
        [InlineData("٠٥٩٩١٢٣٤٥٦", "+970599123456")]
        [InlineData("۵۹۹۱۲۳۴۵۶", "+970599123456")]
        public void PhoneNumberNormalizer_AcceptsPalestineFormats(string input, string expected)
        {
            Assert.True(PhoneNumberNormalizer.TryNormalize(input, out var normalized));
            Assert.Equal(expected, normalized);
        }

        [Theory]
        [InlineData("+905551234567")]
        [InlineData("0555")]
        [InlineData("+970ABC")]
        public void PhoneNumberNormalizer_RejectsInvalidOrUnsupportedNumbers(string input)
        {
            Assert.False(PhoneNumberNormalizer.TryNormalize(input, out _));
        }

        [Fact]
        public void HomePageSection_LocalizedFields_FollowCurrentCultureAndFallback()
        {
            var originalCulture = CultureInfo.CurrentUICulture;
            try
            {
                var section = new HomePageSection
                {
                    Title = "Legacy",
                    TitleEn = "English title",
                    TitleAr = "عنوان عربي"
                };

                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ar");
                Assert.Equal("عنوان عربي", section.LocalizedTitle);

                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                Assert.Equal("English title", section.LocalizedTitle);

                section.TitleEn = string.Empty;
                Assert.Equal("عنوان عربي", section.LocalizedTitle);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }

        [Fact]
        public void KurumsalSayfa_LocalizedContent_FallsBackWithoutLosingLegacyContent()
        {
            var originalCulture = CultureInfo.CurrentUICulture;
            try
            {
                var page = new KurumsalSayfa
                {
                    Baslik = "Legacy title",
                    Icerik = "<p>Legacy content</p>",
                    BaslikAr = "عنوان",
                    IcerikAr = "<p>محتوى</p>"
                };

                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                Assert.Equal("عنوان", page.LocalizedBaslik);
                Assert.Equal("<p>محتوى</p>", page.LocalizedIcerik);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }
    }
}
