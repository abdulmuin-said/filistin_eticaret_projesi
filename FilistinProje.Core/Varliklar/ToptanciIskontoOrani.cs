using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    public class ToptanciIskontoOrani : BaseEntity
    {
        [Required]
        public int ToptanciUrunGrubuId { get; set; }
        public virtual ToptanciUrunGrubu? ToptanciUrunGrubu { get; set; }

        public int? UrunId { get; set; }
        public virtual Urun? Urun { get; set; }

        public int MinAdet { get; set; } = 1;
        public string IskontoTipi { get; set; } = "Yuzde"; // "Yuzde" or "Tutar"
        public decimal IskontoYuzdesi { get; set; }
        public decimal IskontoTutari { get; set; }
        public bool AktifMi { get; set; } = true;
    }
}
