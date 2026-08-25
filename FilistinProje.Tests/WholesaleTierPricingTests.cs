using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FilistinProje.Tests;

public sealed class WholesaleTierPricingTests
{
    [Theory]
    [InlineData(9, 50)]
    [InlineData(10, 42)]
    [InlineData(24, 42)]
    [InlineData(25, 38)]
    [InlineData(30, 38)]
    public async Task OrderPricing_ProductTiers_UseHighestReachedThreshold(int quantity, decimal expected)
    {
        await using var db = CreateContext();
        var product = CreateTieredProduct();
        db.Urunler.Add(product);
        await db.SaveChangesAsync();

        var result = await CreateOrderPricingService(db).HesaplaAsync(
            [CartItem(product.Id, quantity, 999m)],
            null,
            "BankaHavalesi",
            true,
            null);

        Assert.Equal(expected, Assert.Single(result.Satirlar).BirimFiyat);
    }

    [Fact]
    public async Task OrderPricing_VariantTierHasPriority_AndFallsBackToProductTier()
    {
        await using var db = CreateContext();
        var product = CreateTieredProduct();
        var variant = new UrunSecenek
        {
            Id = 101,
            Urun = product,
            Olcu = "500 ml",
            SatisFiyati = 55m,
            StokAdedi = 100,
            AktifMi = true
        };
        product.UrunSecenek.Add(variant);
        product.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi
        {
            UrunSecenek = variant,
            MinAdet = 25,
            BirimFiyat = 35m,
            AktifMi = true
        });
        db.Urunler.Add(product);
        await db.SaveChangesAsync();

        var fallback = await CreateOrderPricingService(db).HesaplaAsync(
            [CartItem(product.Id, 10, 999m, variant.Id)],
            null,
            "BankaHavalesi",
            true,
            null);
        var variantPrice = await CreateOrderPricingService(db).HesaplaAsync(
            [CartItem(product.Id, 30, 999m, variant.Id)],
            null,
            "BankaHavalesi",
            true,
            null);

        Assert.Equal(42m, Assert.Single(fallback.Satirlar).BirimFiyat);
        Assert.Equal(35m, Assert.Single(variantPrice.Satirlar).BirimFiyat);
    }

    [Fact]
    public async Task OrderPricing_NormalCustomer_IgnoresWholesaleTiersAndClientPrice()
    {
        await using var db = CreateContext();
        var product = CreateTieredProduct();
        db.Urunler.Add(product);
        await db.SaveChangesAsync();

        var result = await CreateOrderPricingService(db).HesaplaAsync(
            [CartItem(product.Id, 30, 0.01m)],
            null,
            "BankaHavalesi",
            false,
            null);

        var line = Assert.Single(result.Satirlar);
        Assert.Equal(60m, line.BirimFiyat);
        Assert.Equal(0.01m, line.OncekiSepetFiyat);
    }

    [Fact]
    public async Task CartAndOrderPricing_SplitLinesUseSameAggregateQuantity()
    {
        await using var db = CreateContext();
        const string userId = "wholesale-user";
        AddWholesaleRole(db, userId);
        var product = CreateTieredProduct();
        var cart = new Sepet { AppUserId = userId, SilindiMi = false };
        cart.SepetItems.Add(CartItem(0, 10, 999m, note: "A", product: product));
        cart.SepetItems.Add(CartItem(0, 20, 0.01m, note: "B", product: product));
        db.AddRange(product, cart);
        await db.SaveChangesAsync();

        var cartItems = await new SepetService(db, NullLogger<SepetService>.Instance)
            .GetSepetItemsAsync(userId, "unused-session");
        var order = await CreateOrderPricingService(db).HesaplaAsync(
            cartItems,
            null,
            "BankaHavalesi",
            true,
            null);

        Assert.Equal(2, cartItems.Count);
        Assert.All(cartItems, item => Assert.Equal(38m, item.Fiyat));
        Assert.Equal(38m, Assert.Single(order.Satirlar).BirimFiyat);
    }

    [Fact]
    public async Task CartPricing_InactiveGroupDiscountIsIgnored()
    {
        await using var db = CreateContext();
        const string userId = "inactive-discount-user";
        AddWholesaleRole(db, userId);
        var group = new ToptanciUrunGrubu { Ad = "Test", AktifMi = true };
        var product = CreateTieredProduct();
        product.ToptanciUrunGrubu = group;
        group.IskontoOranlari.Add(new ToptanciIskontoOrani
        {
            MinAdet = 1,
            IskontoYuzdesi = 90m,
            AktifMi = false
        });
        var cart = new Sepet { AppUserId = userId, SilindiMi = false };
        cart.SepetItems.Add(CartItem(0, 9, 1m, product: product));
        db.AddRange(product, cart);
        await db.SaveChangesAsync();

        var item = Assert.Single(await new SepetService(db, NullLogger<SepetService>.Instance)
            .GetSepetItemsAsync(userId, "unused-session"));

        Assert.Equal(50m, item.Fiyat);
    }

    [Fact]
    public void WholesaleTierIndexes_AreUniqueAndScopeFiltered()
    {
        using var db = CreateContext();
        var indexes = db.Model.FindEntityType(typeof(UrunToptanFiyatKademesi))!.GetIndexes()
            .ToDictionary(index => index.GetDatabaseName()!, StringComparer.Ordinal);

        var productIndex = indexes["UX_UrunToptanFiyatKademeleri_Urun_MinAdet"];
        Assert.True(productIndex.IsUnique);
        Assert.Equal("\"UrunSecenekId\" IS NULL AND \"SilindiMi\" = false", productIndex.GetFilter());

        var variantIndex = indexes["UX_UrunToptanFiyatKademeleri_Varyant_MinAdet"];
        Assert.True(variantIndex.IsUnique);
        Assert.Equal("\"UrunSecenekId\" IS NOT NULL AND \"SilindiMi\" = false", variantIndex.GetFilter());
    }

    private static KanvasDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<KanvasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new KanvasDbContext(options);
    }

    private static Urun CreateTieredProduct()
    {
        var product = new Urun
        {
            Baslik = "Toptan test ürünü",
            Fiyat = 60m,
            TopFiyat = 50m,
            AktifMi = true,
            YayindaMi = true,
            MinSiparisAdedi = 1,
            StokDurumu = "Stokta"
        };
        product.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi
        {
            MinAdet = 10,
            BirimFiyat = 42m,
            AktifMi = true
        });
        product.ToptanFiyatKademeleri.Add(new UrunToptanFiyatKademesi
        {
            MinAdet = 25,
            BirimFiyat = 38m,
            AktifMi = true
        });
        return product;
    }

    private static SepetItem CartItem(
        int productId,
        int quantity,
        decimal clientPrice,
        int? variantId = null,
        string? note = null,
        Urun? product = null)
    {
        return new SepetItem
        {
            Urun = product!,
            UrunId = product?.Id ?? productId,
            UrunSecenekId = variantId,
            Adet = quantity,
            Fiyat = clientPrice,
            UrunBaslik = product?.Baslik ?? "Toptan test ürünü",
            CerceveModeli = string.Empty,
            MusteriNotu = note,
            SilindiMi = false
        };
    }

    private static void AddWholesaleRole(KanvasDbContext db, string userId)
    {
        const string roleId = "wholesale-role";
        db.Roles.Add(new IdentityRole
        {
            Id = roleId,
            Name = "Wholesale",
            NormalizedName = "WHOLESALE"
        });
        db.UserRoles.Add(new IdentityUserRole<string> { UserId = userId, RoleId = roleId });
    }

    private static OrderPricingService CreateOrderPricingService(KanvasDbContext db)
    {
        return new OrderPricingService(
            db,
            new TestSiteSettingsService(),
            new TestShippingService(),
            NullLogger<OrderPricingService>.Instance);
    }

    private sealed class TestSiteSettingsService : ISiteSettingsService
    {
        public SiteAyarlari GetSettings() => new();
        public void SaveSettings(SiteAyarlari settings) { }
        public string BuildAbsoluteUrl(string? path) => path ?? string.Empty;
    }

    private sealed class TestShippingService : IKargoHesaplamaServisi
    {
        public Task<decimal> HesaplaAsync(string sehir, decimal siparisToplami, decimal ucretsizKargoLimiti) => Task.FromResult(0m);
        public Task<bool> SehirdeAktifKargoVarMiAsync(string sehir) => Task.FromResult(true);
    }
}
