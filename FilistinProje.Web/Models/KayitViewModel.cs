using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FilistinProje.Web.Models
{
    public class KayitViewModel
    {
        [Required(ErrorMessage = "Validation_FullNameRequired")]
        [Display(Name = "AdSoyad")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_NationalIdRequired")]
        [StringLength(11, MinimumLength = 5, ErrorMessage = "Validation_NationalIdLength")]
        [Display(Name = "KimlikNo")]
        public string KimlikNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_DateOfBirthRequired")]
        [DataType(DataType.Date)]
        [Display(Name = "DogumTarihi")]
        public DateTime? DogumTarihi { get; set; }

        [Required(ErrorMessage = "Validation_PhoneRequired")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_EmailRequired")]
        [EmailAddress(ErrorMessage = "Validation_EmailInvalid")]
        [Display(Name = "Eposta")]
        public string Eposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_AddressRequired")]
        [Display(Name = "Adres")]
        public string Adres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_IdentityPhotoRequired")]
        [DataType(DataType.Upload)]
        [Display(Name = "KimlikFoto")]
        public IFormFile? KimlikFoto { get; set; }

        [Required(ErrorMessage = "Validation_CityRequired")]
        [Display(Name = "Sehir")]
        public string Sehir { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_PasswordRequired")]
        [MinLength(8, ErrorMessage = "Validation_PasswordLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Sifre")]
        public string Sifre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_PasswordRepeatRequired")]
        [Compare("Sifre", ErrorMessage = "Validation_PasswordMismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "SifreTekrar")]
        public string SifreTekrar { get; set; } = string.Empty;

        [Display(Name = "ToptanciMi")]
        public bool ToptanciMi { get; set; }
    }
}
