using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    public class SiparisDetay : BaseEntity
    {
        public int SiparisId { get; set; }
        [ForeignKey("SiparisId")]
        public virtual Siparis Siparis { get; set; } = default!;

        public int? UrunSecenekId { get; set; }

        [ForeignKey("UrunSecenekId")]
        public virtual UrunSecenek? UrunSecenek { get; set; }

        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public int UrunId { get; set; }
        [ForeignKey("UrunId")]
        public Urun Urun { get; set; } = default!;
        public string CerceveModeli { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? MusteriNotu { get; set; }

        public int? HediyePaketSecenegiId { get; set; }

        [ForeignKey(nameof(HediyePaketSecenegiId))]
        public UrunHediyePaketSecenegi? HediyePaketSecenegi { get; set; }

        public bool HediyePaketi { get; set; }
        public decimal HediyePaketFiyati { get; set; }

        [MaxLength(150)]
        public string HediyePaketAdi { get; set; } = string.Empty;

        [MaxLength(150)]
        public string HediyePaketAdiEn { get; set; } = string.Empty;

        [MaxLength(150)]
        public string HediyePaketAdiAr { get; set; } = string.Empty;

        [NotMapped]
        public string LocalizedHediyePaketAdi
        {
            get
            {
                var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return culture switch
                {
                    "ar" => !string.IsNullOrWhiteSpace(HediyePaketAdiAr) ? HediyePaketAdiAr : (!string.IsNullOrWhiteSpace(HediyePaketAdiEn) ? HediyePaketAdiEn : HediyePaketAdi),
                    "en" => !string.IsNullOrWhiteSpace(HediyePaketAdiEn) ? HediyePaketAdiEn : (!string.IsNullOrWhiteSpace(HediyePaketAdiAr) ? HediyePaketAdiAr : HediyePaketAdi),
                    _ => !string.IsNullOrWhiteSpace(HediyePaketAdiAr) ? HediyePaketAdiAr : (!string.IsNullOrWhiteSpace(HediyePaketAdiEn) ? HediyePaketAdiEn : HediyePaketAdi)
                };
            }
        }
    }
}
