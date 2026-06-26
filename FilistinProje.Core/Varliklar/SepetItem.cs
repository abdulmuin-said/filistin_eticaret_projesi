using System;
using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Core.Varliklar
{
    /// <summary>
    /// Sepetteki tek bir ürün - Database'de tutuluyor
    /// </summary>
    public class SepetItem : BaseEntity
    {
        // Hangi sepete ait (Foreign Key)
        public int SepetId { get; set; }
        public Sepet Sepet { get; set; } = default!;
        
        // Hangi ürün (Foreign Key)
        public int UrunId { get; set; }
        public Urun Urun { get; set; } = default!;
        
        // Hangi seçenek/varyant (Optional - null olabilir)
        public int? UrunSecenekId { get; set; }
        public UrunSecenek? UrunSecenek { get; set; }
        
        // Adet
        public int Adet { get; set; }
        
        // Fiyat (Snapshot - O anki fiyat kayıt edilir, sonra ürün fiyatı değişse de bu sabit kalır)
        public decimal Fiyat { get; set; }
        
        // Ürün Bilgileri (Snapshot - Performans için denormalize)
        public string UrunBaslik { get; set; } = string.Empty;
        public string UrunResimUrl { get; set; } = string.Empty;
        public string CerceveModeli { get; set; } = string.Empty;
        public string? SecenekAdi { get; set; } // Örn: "50x70 cm - Çerçevesiz"

        [MaxLength(500)]
        public string? MusteriNotu { get; set; }

        public bool HediyePaketi { get; set; }
        public decimal HediyePaketFiyati { get; set; }

        // Toplam (Calculated property)
        public decimal Toplam => (Fiyat * Adet) + (HediyePaketi ? HediyePaketFiyati * Adet : 0);
    }
}
