using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    public class CarkOdul : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string LabelTr { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LabelEn { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LabelAr { get; set; } = string.Empty;

        /// <summary>discount | freeship | none</summary>
        [Required]
        [StringLength(20)]
        public string Tip { get; set; } = "discount";

        /// <summary>İndirim yüzdesi (discount tipinde)</summary>
        public int Deger { get; set; }

        /// <summary>Canvas dilim rengi (hex)</summary>
        [Required]
        [StringLength(10)]
        public string Renk { get; set; } = "#313511";

        /// <summary>Kazanyana mesaj (opsiyonel)</summary>
        [StringLength(200)]
        public string? MesajTr { get; set; }
        [StringLength(200)]
        public string? MesajEn { get; set; }
        [StringLength(200)]
        public string? MesajAr { get; set; }

        public int Sira { get; set; }
        public bool AktifMi { get; set; } = true;
    }
}
