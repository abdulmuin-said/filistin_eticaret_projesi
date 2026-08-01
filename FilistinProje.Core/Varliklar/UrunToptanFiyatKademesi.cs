using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    public class UrunToptanFiyatKademesi : BaseEntity
    {
        public int UrunId { get; set; }
        public Urun Urun { get; set; } = default!;

        public int? UrunSecenekId { get; set; }
        public UrunSecenek? UrunSecenek { get; set; }

        [Range(1, int.MaxValue)]
        public int MinAdet { get; set; } = 1;

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal BirimFiyat { get; set; }

        public bool AktifMi { get; set; } = true;
        public int Sira { get; set; }
    }
}
