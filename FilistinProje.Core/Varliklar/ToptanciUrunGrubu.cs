using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilistinProje.Core.Varliklar
{
    public class ToptanciUrunGrubu : BaseEntity
    {
        [Required]
        public string Ad { get; set; } = string.Empty;
        public string? Aciklama { get; set; }
        public bool AktifMi { get; set; } = true;
        public int Sira { get; set; }

        public virtual ICollection<Urun> Urunler { get; set; } = new List<Urun>();
        public virtual ICollection<ToptanciIskontoOrani> IskontoOranlari { get; set; } = new List<ToptanciIskontoOrani>();
    }
}
