namespace FilistinProje.Core.Varliklar
{
    /// <summary>
    /// Sosyal medya bağlantısı. Platform adı, URL, Font Awesome ikon sınıfı,
    /// görüntülenme sırası ve aktiflik durumu içerir.
    /// </summary>
    public class SosyalMedyaLink
    {
        public int Id { get; set; }

        /// <summary>Platform adı (ör. "Instagram", "Snapchat")</summary>
        public string PlatformAdi { get; set; } = string.Empty;

        /// <summary>Tam URL — sadece https:// veya http:// kabul edilir</summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Font Awesome ikon sınıfı (ör. "fab fa-instagram", "fab fa-snapchat").
        /// Boş bırakılırsa footer varsayılan ikon gösterir.
        /// </summary>
        public string IkonSinifi { get; set; } = string.Empty;

        /// <summary>Küçük sayı = önde gösterilir</summary>
        public int Sira { get; set; } = 0;

        public bool AktifMi { get; set; } = true;

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
    }
}
