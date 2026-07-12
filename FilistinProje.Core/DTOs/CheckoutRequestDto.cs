using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FilistinProje.Core.Varliklar;

namespace FilistinProje.Core.DTOs
{
    /// <summary>
    /// Sipariş checkout sayfasından kullanıcının göndermesi gereken alanlar.
    /// Server-owned alanlar (ToplamTutar, IndirimTutari, Durum, AppUserId, SiparisNo, ...) bind edilmez.
    /// B27: Direct entity binding yerine güvenli DTO.
    /// </summary>
    public class CheckoutRequestDto
    {
        [Required, MaxLength(150)]
        public string MusteriAdSoyad { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string Eposta { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Telefon { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Sehir { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Ilce { get; set; } = string.Empty;

        [MaxLength(500)]
        public string AcikAdres { get; set; } = string.Empty;

        public string TeslimatTipi { get; set; } = "AdreseTeslim";

        /// <summary>Sipariş notu</summary>
        [MaxLength(1000)]
        public string? Aciklama { get; set; }

        /// <summary>Reçete dosyası yolu</summary>
        [MaxLength(500)]
        public string? ReceteDosyaYolu { get; set; }

        /// <summary>Kimlik fotoğraf yolu</summary>
        [MaxLength(500)]
        public string? KimlikFotoYolu { get; set; }

        /// <summary>Adresi profilime kaydet checkbox</summary>
        public bool AdresiKaydet { get; set; }

        /// <summary>Yeni adres başlığı</summary>
        [MaxLength(100)]
        public string? YeniAdresBasligi { get; set; }

        /// <summary>Sözleşme onaylandı mı?</summary>
        public bool SozlesmeOnaylandi { get; set; }

        /// <summary>"BankaHavalesi" veya "KapidaOdeme"</summary>
        [Required, MaxLength(30)]
        public string OdemeYontemi { get; set; } = "BankaHavalesi";

        /// <summary>
        /// Validasyonu geçen DTO'yu Siparis entity'sine dönüştürür.
        /// Server-owned alanlar set edilmez (controller bunları hesaplar).
        /// </summary>
        public Siparis ToSiparisEntity()
        {
            return new Siparis
            {
                MusteriAdSoyad = MusteriAdSoyad.Trim(),
                Eposta = Eposta.Trim(),
                Telefon = Telefon.Trim(),
                Sehir = Sehir.Trim(),
                Ilce = Ilce.Trim(),
                AcikAdres = AcikAdres.Trim(),
                TeslimatTipi = TeslimatTipi == "MagazadanTeslim" ? "MagazadanTeslim" : "AdreseTeslim",
                Aciklama = Aciklama?.Trim(),
                ReceteDosyaYolu = ReceteDosyaYolu?.Trim(),
                KimlikFotoYolu = KimlikFotoYolu?.Trim(),
            };
        }
    }
}