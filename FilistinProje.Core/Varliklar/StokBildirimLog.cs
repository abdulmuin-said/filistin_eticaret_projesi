using System.ComponentModel.DataAnnotations.Schema;

namespace FilistinProje.Core.Varliklar
{
    /// <summary>
    /// Stok uyarı bildirimlerinin log'u.
    /// Her satır, bir varyant için gönderilen bir stok uyarısını temsil eder.
    /// Tekrarlı bildirimleri önlemek için kullanılır (cooldown: 24 saat).
    /// </summary>
    public class StokBildirimLog : BaseEntity
    {
        public int UrunId { get; set; }
        public Urun? Urun { get; set; }

        public int? UrunSecenekId { get; set; }
        public UrunSecenek? UrunSecenek { get; set; }

        /// <summary>Bildirim anındaki kalan stok adedi</summary>
        public int KalanStok { get; set; }

        /// <summary>Bildirimi tetikleyen stok eşiği (SiteAyarlari.StokUyariLimiti)</summary>
        public int StokEsigi { get; set; }

        /// <summary>"DusukStok" veya "Tukendi"</summary>
        public string BildirimTipi { get; set; } = "DusukStok";

        /// <summary>Bildirimin başarıyla gönderilip gönderilmediği</summary>
        public bool GonderildiMi { get; set; } = true;
    }
}
