using FilistinProje.Core.Varliklar;
using FilistinProje.Core.Models;

namespace FilistinProje.Web.Models
{
    public class HomeViewModel
    {
        public List<Urun> VitrinUrunleri { get; set; } = new List<Urun>();
        public List<Urun> BesParcaliKoleksiyon { get; set; } = new List<Urun>();
        public List<Urun> CokSatanlar { get; set; } = new List<Urun>();
        public List<Urun> FirsatUrunleri { get; set; } = new List<Urun>();
        public List<Kategori> Kategoriler { get; set; } = new List<Kategori>();
        public IEnumerable<FilistinProje.Core.Varliklar.HomePageSection> Sections { get; set; } = new List<FilistinProje.Core.Varliklar.HomePageSection>();
        public List<Slayt> AktifSlaytlar { get; set; } = new List<Slayt>();
    }
}
