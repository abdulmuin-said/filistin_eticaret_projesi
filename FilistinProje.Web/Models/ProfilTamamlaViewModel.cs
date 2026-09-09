using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FilistinProje.Web.Models
{
    public class ProfilTamamlaViewModel
    {
        [Display(Name = "AdSoyad")]
        public string AdSoyad { get; set; } = string.Empty;

        [Display(Name = "Eposta")]
        public string Eposta { get; set; } = string.Empty;

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

        [Required(ErrorMessage = "Validation_RegionRequired")]
        [Display(Name = "Region")]
        public string? Bolge { get; set; }

        [Required(ErrorMessage = "Validation_CityRequired")]
        [Display(Name = "Sehir")]
        public string Sehir { get; set; } = string.Empty;

        [Required(ErrorMessage = "Validation_AddressRequired")]
        [Display(Name = "Adres")]
        public string Adres { get; set; } = string.Empty;

        [Display(Name = "KimlikFoto")]
        public IFormFile? KimlikFoto { get; set; }

        public string? MevcutKimlikFotoUrl { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
