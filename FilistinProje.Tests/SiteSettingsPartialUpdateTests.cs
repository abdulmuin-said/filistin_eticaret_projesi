using FilistinProje.Core.Models;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace FilistinProje.Tests;

public sealed class SiteSettingsPartialUpdateTests
{
    private sealed class DummyHostEnvironment : IWebHostEnvironment
    {
        public string WebRootPath { get; set; } = string.Empty;
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public string ApplicationName { get; set; } = "FilistinProje.Tests";
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
        public string ContentRootPath { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = "Development";
    }

    private static (SiteSettingsService service, KanvasDbContext db) CreateService()
    {
        var options = new DbContextOptionsBuilder<KanvasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new KanvasDbContext(options);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var config = new ConfigurationBuilder().Build();
        var env = new DummyHostEnvironment();

        var service = new SiteSettingsService(db, cache, config, env);
        return (service, db);
    }

    [Fact]
    public void SaveSettings_KargoTab_UpdatesOnlyShippingFields_PreservesGeneralAndPaymentFields()
    {
        var (service, _) = CreateService();

        // 1. Initial full settings
        var initial = new SiteAyarlari
        {
            SiteAdi = "7ANRPS48",
            SiteBasligi = "Filistin Store",
            Telefon = "+970599123456",
            Email = "info@7anrps48.com",
            KargoBedeli = 20m,
            UcretsizKargoLimiti = 200m,
            SiparisTeslimSuresiGun = 3,
            KapidaOdemeAktifMi = true,
            KapidaOdemeLimiti = 2000m,
            KapidaOdemeHizmetBedeli = 10m
        };
        service.SaveSettings(initial);

        // 2. Simulate form POST from Kargo tab (other tab values are null/0/false in model)
        var kargoTabPost = new SiteAyarlari
        {
            KargoBedeli = 35m,
            UcretsizKargoLimiti = 350m,
            SiparisTeslimSuresiGun = 5,
            // All other tabs are default/empty because they were not in the Kargo tab form:
            SiteAdi = string.Empty,
            SiteBasligi = string.Empty,
            Telefon = string.Empty,
            KapidaOdemeLimiti = 0m,
            KapidaOdemeAktifMi = false
        };

        service.SaveSettings(kargoTabPost, "kargo");

        // 3. Verify
        var updated = service.GetSettings();
        // Shipping fields updated:
        Assert.Equal(35m, updated.KargoBedeli);
        Assert.Equal(350m, updated.UcretsizKargoLimiti);
        Assert.Equal(5, updated.SiparisTeslimSuresiGun);
        // General fields preserved:
        Assert.Equal("7ANRPS48", updated.SiteAdi);
        Assert.Equal("Filistin Store", updated.SiteBasligi);
        Assert.Equal("+970599123456", updated.Telefon);
        Assert.Equal("info@7anrps48.com", updated.Email);
        // Payment fields preserved:
        Assert.True(updated.KapidaOdemeAktifMi);
        Assert.Equal(2000m, updated.KapidaOdemeLimiti);
        Assert.Equal(10m, updated.KapidaOdemeHizmetBedeli);
    }

    [Fact]
    public void SaveSettings_KapidaOdemeTab_UpdatesOnlyPaymentFields_PreservesShippingAndGeneral()
    {
        var (service, _) = CreateService();

        var initial = new SiteAyarlari
        {
            SiteAdi = "7ANRPS48",
            KargoBedeli = 25m,
            UcretsizKargoLimiti = 300m,
            KapidaOdemeAktifMi = true,
            KapidaOdemeLimiti = 2000m,
            KapidaOdemeHizmetBedeli = 15m
        };
        service.SaveSettings(initial);

        var paymentTabPost = new SiteAyarlari
        {
            KapidaOdemeAktifMi = false,
            KapidaOdemeLimiti = 1500m,
            KapidaOdemeHizmetBedeli = 0m,
            // Untouched fields in form are default:
            SiteAdi = string.Empty,
            KargoBedeli = 0m,
            UcretsizKargoLimiti = 0m
        };

        service.SaveSettings(paymentTabPost, "kapida-odeme");

        var updated = service.GetSettings();
        // Payment fields updated:
        Assert.False(updated.KapidaOdemeAktifMi);
        Assert.Equal(1500m, updated.KapidaOdemeLimiti);
        Assert.Equal(0m, updated.KapidaOdemeHizmetBedeli);
        // Shipping & General preserved:
        Assert.Equal("7ANRPS48", updated.SiteAdi);
        Assert.Equal(25m, updated.KargoBedeli);
        Assert.Equal(300m, updated.UcretsizKargoLimiti);
    }

    [Fact]
    public void SaveSettings_GenelTab_UpdatesOnlyGeneralFields_PreservesOtherTabs()
    {
        var (service, _) = CreateService();

        var initial = new SiteAyarlari
        {
            SiteAdi = "Old Brand",
            SiteBasligi = "Old Title",
            KargoBedeli = 30m,
            KapidaOdemeLimiti = 2500m,
            UstBarEtkin = false
        };
        service.SaveSettings(initial);

        var genelTabPost = new SiteAyarlari
        {
            SiteAdi = "New Brand Palestine",
            SiteBasligi = "New Title",
            UstBarEtkin = true,
            // Untouched:
            KargoBedeli = 0m,
            KapidaOdemeLimiti = 0m
        };

        service.SaveSettings(genelTabPost, "genel");

        var updated = service.GetSettings();
        Assert.Equal("New Brand Palestine", updated.SiteAdi);
        Assert.Equal("New Title", updated.SiteBasligi);
        Assert.True(updated.UstBarEtkin);
        Assert.Equal(30m, updated.KargoBedeli);
        Assert.Equal(2500m, updated.KapidaOdemeLimiti);
    }
}
