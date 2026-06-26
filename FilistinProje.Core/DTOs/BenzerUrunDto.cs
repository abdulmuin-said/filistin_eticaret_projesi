using System.Globalization;

namespace FilistinProje.Core.DTOs
{
    public class BenzerUrunDto
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public string BaslikEn { get; set; } = string.Empty;
        public string BaslikAr { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? AnaGorselUrl { get; set; }
        public decimal Fiyat { get; set; }
        public decimal? IndirimliFiyat { get; set; }
        public string LocalizedBaslik => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName switch
        {
            "ar" => !string.IsNullOrWhiteSpace(BaslikAr) ? BaslikAr : Baslik,
            "en" => !string.IsNullOrWhiteSpace(BaslikEn) ? BaslikEn : Baslik,
            _ => Baslik
        };
    }
}
