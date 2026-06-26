using FilistinProje.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace FilistinProje.Core.Varliklar
{
    // IdentityUser'dan miras alıyoruz, yani standart özelliklerin üzerine ekleme yapıyoruz
    public class AppUser : IdentityUser
    {
        public string AdSoyad { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public string KimlikNo { get; set; } = string.Empty;
        public DateTime? DogumTarihi { get; set; }
        public string KimlikFotografYolu { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public WholesaleStatus WholesaleStatus { get; set; } = WholesaleStatus.Pending;
        public DateTime? BasvuruTarihi { get; set; }

        public string? SifreSifirlamaToken { get; set; }
        public DateTime? SifreSifirlamaGecerlilik { get; set; }
        
        // İleride puan sistemi yaparsak diye:
        // public int Puan { get; set; }
    }
}
