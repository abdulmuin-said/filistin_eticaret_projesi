using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    public class SiteDegerlendirme : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Eposta { get; set; }

        [Range(1, 5)]
        public int Puan { get; set; }

        [StringLength(200)]
        public string? Baslik { get; set; }

        [Required]
        [StringLength(2000)]
        public string YorumMetni { get; set; } = string.Empty;

        public bool OnayliMi { get; set; }
    }
}
