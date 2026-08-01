using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FilistinProje.Core.Varliklar
{
    public class KurumsalSayfa : BaseEntity
    {
        [Required]
        public string Baslik { get; set; } = string.Empty; // Örn: Mesafeli Satış Sözleşmesi
        public string BaslikEn { get; set; } = string.Empty;
        public string BaslikAr { get; set; } = string.Empty;

        [Required]
        public string Icerik { get; set; } = string.Empty; // HTML Formatında sözleşme metni
        public string IcerikEn { get; set; } = string.Empty;
        public string IcerikAr { get; set; } = string.Empty;

        [Required]
        public string UrlSlug { get; set; } = string.Empty; // Örn: mesafeli-satis-sozlesmesi (Link için)
        
        public int Sira { get; set; } // Menüdeki sırası

        [NotMapped]
        public string LocalizedBaslik => GetLocalized(Baslik, BaslikEn, BaslikAr);

        [NotMapped]
        public string LocalizedIcerik => GetLocalized(Icerik, IcerikEn, IcerikAr);

        private static string GetLocalized(string legacyValue, string en, string ar)
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
}
