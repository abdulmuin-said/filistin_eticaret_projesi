using FilistinProje.Core.Varliklar;
using System.Globalization;

namespace FilistinProje.Tests;

public sealed class KategoriLocalizationTests
{
    [Fact]
    public void GetLocalized_ArabicAndEnglishValues_AreSelectedWithoutTurkishFallback()
    {
        var category = new Kategori
        {
            Ad = "Legacy",
            AdEn = "Gaza",
            AdAr = "غزة"
        };

        var originalCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            Assert.Equal("Gaza", category.LocalizedAd);

            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ar");
            Assert.Equal("غزة", category.LocalizedAd);
        }
        finally
        {
            CultureInfo.CurrentUICulture = originalCulture;
        }
    }
}
