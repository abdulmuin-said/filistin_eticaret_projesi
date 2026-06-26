using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilistinProje.Core.Varliklar
{
    /// <summary>
    /// FCM push bildirim aboneliklerini tutar.
    /// Her kayıt bir tarayıcı/cihaz için FCM token'ını saklar.
    /// </summary>
    public class PushAbonelik : BaseEntity
    {
        /// <summary>Firebase Cloud Messaging cihaz token'ı</summary>
        [Required]
        [StringLength(500)]
        public string Token { get; set; } = string.Empty;

        /// <summary>Opsiyonel: Oturum açmış kullanıcı ID'si</summary>
        public string? AppUserId { get; set; }

        [ForeignKey(nameof(AppUserId))]
        public AppUser? AppUser { get; set; }

        /// <summary>Tarayıcı bilgisi (Chrome 120, Firefox 121 vb.)</summary>
        [StringLength(200)]
        public string? Tarayici { get; set; }

        /// <summary>İşletim sistemi / platform (Windows, Android, iOS vb.)</summary>
        [StringLength(100)]
        public string? Platform { get; set; }

        /// <summary>Bildirimlere izin verildi mi?</summary>
        public bool AktifMi { get; set; } = true;

        /// <summary>Token'ın en son görüldüğü tarih (aktif kullanım kontrolü)</summary>
        public DateTime? SonGorulmeTarihi { get; set; }

        /// <summary>Kayıt tarihi (BaseEntity.OlusturulmaTarihi)</summary>
        /// <summary>Bildirim tercihleri (JSON): kampanya, siparis, firsat vb.</summary>
        [StringLength(500)]
        public string? Tercihler { get; set; }
    }
}
