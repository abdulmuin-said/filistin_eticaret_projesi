using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Tests;

public sealed class ShippingAndDecimalRegressionTests
{
    [Theory]
    [InlineData("20,00", "20.00")]
    [InlineData("20.00", "20.00")]
    [InlineData("25,50", "25.50")]
    [InlineData("1.234,56", "1234.56")]
    [InlineData("1,234.56", "1234.56")]
    [InlineData("٢٠٫٠٠", "20.00")]
    [InlineData("۲۵٫۵۰", "25.50")]
    public void FlexibleDecimalParser_AcceptsSupportedFormats(string input, string expected)
    {
        Assert.True(FlexibleDecimalParser.TryParse(input, out var result));
        Assert.Equal(decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture), result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("20-00")]
    [InlineData("--20")]
    public void FlexibleDecimalParser_RejectsInvalidValues(string input)
    {
        Assert.False(FlexibleDecimalParser.TryParse(input, out _));
    }

    [Fact]
    public async Task ShippingCalculation_UsesCreatedAndUpdatedRegionPrice()
    {
        var options = new DbContextOptionsBuilder<KanvasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new KanvasDbContext(options);
        var region = new KargoBolge
        {
            Ad = "Test region",
            Fiyat = 20m,
            Sehirler = new List<KargoBolgeSehir>
            {
                new() { SehirAdi = "Ramallah" }
            }
        };
        db.KargoBolgeler.Add(region);
        await db.SaveChangesAsync();

        var service = new KargoHesaplamaServisi(db);
        Assert.Equal(20m, await service.HesaplaAsync("Ramallah", 100m, 500m));

        region.Fiyat = 25.50m;
        await db.SaveChangesAsync();

        Assert.Equal(25.50m, await service.HesaplaAsync("Ramallah", 100m, 500m));
    }
}
