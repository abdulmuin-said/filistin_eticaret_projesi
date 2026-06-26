using System.Collections.Generic;

namespace FilistinProje.Core.Varliklar
{
    public class KargoBolge : BaseEntity
    {
        public string Ad { get; set; } = string.Empty;
        public string? Ulke { get; set; }
        public string? Aciklama { get; set; }
        public int Sira { get; set; }
        public decimal Fiyat { get; set; } = 0;

        public ICollection<KargoBolgeSehir> Sehirler { get; set; } = new List<KargoBolgeSehir>();
        public ICollection<KargoBolgeFiyat> Fiyatlar { get; set; } = new List<KargoBolgeFiyat>();
    }
}
