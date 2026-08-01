using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FilistinProje.Core.Varliklar
{
    public enum HomePageSectionType
    {
        HeroSlider = 0,
        ProductBlock = 1,
        CategoryShowcase = 2,
        CustomBanner = 3,
        AutoVitrin = 4,
        AutoCokSatanlar = 5,
        AutoFirsatUrunleri = 6,
        AutoBesParcali = 7
    }

    public class HomePageSection
    {
        public int Id { get; set; }
        public HomePageSectionType SectionType { get; set; }

        // Genel alanlar (aktif, sıralama, başlık vs.)
        public bool Enabled { get; set; } = true;
        public int SortOrder { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string SubtitleEn { get; set; } = string.Empty;
        public string SubtitleAr { get; set; } = string.Empty;

        // Ürün blokları için ek alanlar (View All linki)
        public string? ViewAllText { get; set; }
        public string ViewAllTextEn { get; set; } = string.Empty;
        public string ViewAllTextAr { get; set; } = string.Empty;
        public string? ViewAllUrl { get; set; }

        // Custom banner alanları
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string? ButtonText { get; set; }
        public string ButtonTextEn { get; set; } = string.Empty;
        public string ButtonTextAr { get; set; } = string.Empty;
        public string? ButtonUrl { get; set; }

        // Ürün ilişkisi (manuel ürünler)
        public ICollection<HomePageSectionProduct> SectionProducts { get; set; } = new List<HomePageSectionProduct>();

        [NotMapped]
        public string LocalizedTitle => GetLocalized(Title, TitleEn, TitleAr);

        [NotMapped]
        public string LocalizedSubtitle => GetLocalized(Subtitle, SubtitleEn, SubtitleAr);

        [NotMapped]
        public string LocalizedViewAllText => GetLocalized(ViewAllText, ViewAllTextEn, ViewAllTextAr);

        [NotMapped]
        public string LocalizedDescription => GetLocalized(Description, DescriptionEn, DescriptionAr);

        [NotMapped]
        public string LocalizedButtonText => GetLocalized(ButtonText, ButtonTextEn, ButtonTextAr);

        private static string GetLocalized(string? legacyValue, string? en, string? ar)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return culture switch
            {
                "ar" => FirstNonEmpty(ar, en, legacyValue),
                "en" => FirstNonEmpty(en, ar, legacyValue),
                _ => FirstNonEmpty(ar, en, legacyValue)
            };
        }

        private static string FirstNonEmpty(params string?[] values) =>
            values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim() ?? string.Empty;
    }

    public class HomePageSectionProduct
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public int UrunId { get; set; }
        public int SortOrder { get; set; }

        public HomePageSection Section { get; set; } = null!;
        public Urun Urun { get; set; } = null!;
    }
}
