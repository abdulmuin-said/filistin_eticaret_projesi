using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FilistinProje.Tests;

public sealed class GiftPackagePricingTests
{
    [Fact]
    public async Task Pricing_UsesCurrentServerPackagePrice_PerItem()
    {
        await using var db = CreateContext();
        var product = CreateProduct(100m);
        var package = new UrunHediyePaketSecenegi
        {
            Urun = product,
            Ad = "Standart paket",
            AdEn = "Standard package",
            AdAr = "تغليف قياسي",
            Fiyat = 15m,
            AktifMi = true,
            Sira = 1
        };
        db.AddRange(product, package);
        await db.SaveChangesAsync();

        var cartItem = new SepetItem
        {
            Id = 42,
            UrunId = product.Id,
            HediyePaketSecenegiId = package.Id,
            HediyePaketFiyati = 0.01m,
            HediyePaketi = true,
            Fiyat = 1m,
            Adet = 2,
            UrunBaslik = product.Baslik
        };

        var result = await CreateService(db).HesaplaAsync([cartItem], null, "BankaHavalesi", false, null);

        var line = Assert.Single(result.Satirlar);
        Assert.False(result.GecersizHediyePaketiVar);
        Assert.Equal(15m, line.HediyePaketBirim);
        Assert.Equal(230m, line.SatirToplam);
        Assert.Equal(230m, result.AraToplam);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public async Task Pricing_RejectsInactiveOrOtherProductPackage(bool sameProduct, bool inactive)
    {
        await using var db = CreateContext();
        var product = CreateProduct(100m);
        var packageOwner = sameProduct ? product : CreateProduct(80m);
        var package = new UrunHediyePaketSecenegi
        {
            Urun = packageOwner,
            Ad = "Paket",
            AdEn = "Package",
            AdAr = "تغليف",
            Fiyat = 15m,
            AktifMi = !inactive,
            Sira = 1
        };
        db.Add(product);
        if (!sameProduct) db.Add(packageOwner);
        db.Add(package);
        await db.SaveChangesAsync();

        var result = await CreateService(db).HesaplaAsync(
            [new SepetItem { Id = 7, UrunId = product.Id, HediyePaketSecenegiId = package.Id, Adet = 1, UrunBaslik = product.Baslik }],
            null,
            "BankaHavalesi",
            false,
            null);

        Assert.True(result.GecersizHediyePaketiVar);
        Assert.Empty(result.Satirlar);
    }

    private static KanvasDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<KanvasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new KanvasDbContext(options);
    }

    private static Urun CreateProduct(decimal price)
    {
        return new Urun
        {
            Baslik = "Test ürün",
            Fiyat = price,
            AktifMi = true,
            YayindaMi = true,
            MinSiparisAdedi = 1,
            StokDurumu = "Stokta"
        };
    }

    private static OrderPricingService CreateService(KanvasDbContext db)
    {
        return new OrderPricingService(
            db,
            new TestSiteSettingsService(),
            new TestShippingService(),
            NullLogger<OrderPricingService>.Instance);
    }

    private sealed class TestSiteSettingsService : ISiteSettingsService
    {
        public SiteAyarlari GetSettings() => new() { UcretsizKargoLimiti = 500m };
        public void SaveSettings(SiteAyarlari settings) { }
        public string BuildAbsoluteUrl(string? path) => path ?? string.Empty;
    }

    private sealed class TestShippingService : IKargoHesaplamaServisi
    {
        public Task<decimal> HesaplaAsync(string sehir, decimal siparisToplami, decimal ucretsizKargoLimiti) => Task.FromResult(0m);
        public Task<bool> SehirdeAktifKargoVarMiAsync(string sehir) => Task.FromResult(true);
    }
}
