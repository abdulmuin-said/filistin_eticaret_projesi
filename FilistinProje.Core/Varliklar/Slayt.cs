using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilistinProje.Core.Varliklar
{
    public class Slayt
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public string BaslikEn { get; set; } = string.Empty;
        public string BaslikAr { get; set; } = string.Empty;
        public string AltBaslik { get; set; } = string.Empty;
        public string AltBaslikEn { get; set; } = string.Empty;
        public string AltBaslikAr { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string AciklamaEn { get; set; } = string.Empty;
        public string AciklamaAr { get; set; } = string.Empty;
        public string? ResimUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string Tur { get; set; } = "Resim";
        public int Sira { get; set; }
        public bool AktifMi { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Slider buton alanları - Reve.com tarzı modern slider için
        public string ButonMetni { get; set; } = string.Empty;
        public string ButonMetniEn { get; set; } = string.Empty;
        public string ButonMetniAr { get; set; } = string.Empty;
        public string? BaglantiUrl { get; set; }

        [NotMapped]
        public string LocalizedBaslik => GetLocalized(Baslik, BaslikEn, BaslikAr);

        [NotMapped]
        public string LocalizedAltBaslik => GetLocalized(AltBaslik, AltBaslikEn, AltBaslikAr);

        [NotMapped]
        public string LocalizedAciklama => GetLocalized(Aciklama, AciklamaEn, AciklamaAr);

        [NotMapped]
        public string LocalizedButonMetni => GetLocalized(ButonMetni, ButonMetniEn, ButonMetniAr);

        private static string GetLocalized(string tr, string en, string ar)
        {
            return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName switch
            {
                "ar" => !string.IsNullOrWhiteSpace(ar) ? ar : tr,
                "en" => !string.IsNullOrWhiteSpace(en) ? en : tr,
                _ => tr
            };
        }
    }
}