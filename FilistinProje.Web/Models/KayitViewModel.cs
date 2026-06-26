using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FilistinProje.Web.Models
{
    public class KayitViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [Display(Name = "AdSoyad")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kimlik Numarası zorunludur")]
        [StringLength(11, MinimumLength = 5, ErrorMessage = "Kimlik numarası 5-11 haneli olmalıdır")]
        [Display(Name = "KimlikNo")]
        public string KimlikNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğum Tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "DogumTarihi")]
        public DateTime? DogumTarihi { get; set; }

        [Required(ErrorMessage = "Telefon zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-Posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz")]
        [Display(Name = "Eposta")]
        public string Eposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres zorunludur")]
        [Display(Name = "Adres")]
        public string Adres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kimlik fotoğrafı zorunludur")]
        [DataType(DataType.Upload)]
        [Display(Name = "KimlikFoto")]
        public IFormFile? KimlikFoto { get; set; }

        [Required(ErrorMessage = "Şehir zorunludur")]
        [Display(Name = "Sehir")]
        public string Sehir { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalı")]
        [DataType(DataType.Password)]
        [Display(Name = "Sifre")]
        public string Sifre { get; set; } = string.Empty;

        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        [Display(Name = "SifreTekrar")]
        public string SifreTekrar { get; set; } = string.Empty;

        [Display(Name = "ToptanciMi")]
        public bool ToptanciMi { get; set; }
    }
}
