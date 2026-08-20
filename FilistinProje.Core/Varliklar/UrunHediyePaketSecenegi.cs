using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FilistinProje.Core.Varliklar
{
    public class UrunHediyePaketSecenegi : BaseEntity
    {
        public int UrunId { get; set; }

        [ForeignKey(nameof(UrunId))]
        public Urun Urun { get; set; } = default!;

        [MaxLength(150)]
        public string Ad { get; set; } = string.Empty;

        [MaxLength(150)]
        public string AdEn { get; set; } = string.Empty;

        [MaxLength(150)]
        public string AdAr { get; set; } = string.Empty;

        public decimal Fiyat { get; set; }
        public bool AktifMi { get; set; } = true;
        public int Sira { get; set; }

        [NotMapped]
        public string LocalizedAd
        {
            get
            {
                var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return culture switch
                {
                    "ar" => !string.IsNullOrWhiteSpace(AdAr) ? AdAr : (!string.IsNullOrWhiteSpace(AdEn) ? AdEn : Ad),
                    "en" => !string.IsNullOrWhiteSpace(AdEn) ? AdEn : (!string.IsNullOrWhiteSpace(AdAr) ? AdAr : Ad),
                    _ => !string.IsNullOrWhiteSpace(AdAr) ? AdAr : (!string.IsNullOrWhiteSpace(AdEn) ? AdEn : Ad)
                };
            }
        }
    }
}
