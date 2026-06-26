using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    public class ToptanciIskontoOrani : BaseEntity
    {
        [Required]
        public int ToptanciUrunGrubuId { get; set; }
        public virtual ToptanciUrunGrubu? ToptanciUrunGrubu { get; set; }

        public int MinAdet { get; set; } = 1;
        public decimal IskontoYuzdesi { get; set; }
        public bool AktifMi { get; set; } = true;
    }
}
