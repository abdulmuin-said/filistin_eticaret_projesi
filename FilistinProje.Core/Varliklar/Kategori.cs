using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FilistinProje.Core.Varliklar
{
    public class Kategori : BaseEntity
    {
        public string Ad { get; set; } = string.Empty;
        public string AdEn { get; set; } = string.Empty;
        public string AdAr { get; set; } = string.Empty;
        public string KisaAciklama { get; set; } = string.Empty;
        public string KisaAciklamaEn { get; set; } = string.Empty;
        public string KisaAciklamaAr { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string AciklamaEn { get; set; } = string.Empty;
        public string AciklamaAr { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? GorselUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string SeoTitle { get; set; } = string.Empty;
        public string SeoTitleEn { get; set; } = string.Empty;
        public string SeoTitleAr { get; set; } = string.Empty;
        public string SeoDescription { get; set; } = string.Empty;
        public string SeoDescriptionEn { get; set; } = string.Empty;
        public string SeoDescriptionAr { get; set; } = string.Empty;
        public string UstMetin { get; set; } = string.Empty;
        public string UstMetinEn { get; set; } = string.Empty;
        public string UstMetinAr { get; set; } = string.Empty;
        public string AltMetin { get; set; } = string.Empty;
        public string AltMetinEn { get; set; } = string.Empty;
        public string AltMetinAr { get; set; } = string.Empty;
        public string KampanyaEtiketi { get; set; } = string.Empty;
        public string KampanyaEtiketiEn { get; set; } = string.Empty;
        public string KampanyaEtiketiAr { get; set; } = string.Empty;
        public string UrunSiralamaTipi { get; set; } = "manual";
        public int Sira { get; set; }
        public bool AktifMi { get; set; } = true;
        public bool ReceteGerekliMi { get; set; } = false;

        public int? ParentKategoriId { get; set; }
        public Kategori? ParentKategori { get; set; }
        public ICollection<Kategori> AltKategoriler { get; set; } = new List<Kategori>();

        public ICollection<Urun> Urunler { get; set; } = new List<Urun>();

        [NotMapped]
        public bool AnaKategoriMi => !ParentKategoriId.HasValue;

        [NotMapped]
        public string LocalizedAd => GetLocalized(Ad, AdEn, AdAr);

        [NotMapped]
        public string LocalizedKisaAciklama => GetLocalized(KisaAciklama, KisaAciklamaEn, KisaAciklamaAr);

        [NotMapped]
        public string LocalizedAciklama => GetLocalized(Aciklama, AciklamaEn, AciklamaAr);

        [NotMapped]
        public string LocalizedSeoTitle => GetLocalized(SeoTitle, SeoTitleEn, SeoTitleAr);

        [NotMapped]
        public string LocalizedSeoDescription => GetLocalized(SeoDescription, SeoDescriptionEn, SeoDescriptionAr);

        [NotMapped]
        public string LocalizedUstMetin => GetLocalized(UstMetin, UstMetinEn, UstMetinAr);

        [NotMapped]
        public string LocalizedAltMetin => GetLocalized(AltMetin, AltMetinEn, AltMetinAr);

        [NotMapped]
        public string LocalizedKampanyaEtiketi => GetLocalized(KampanyaEtiketi, KampanyaEtiketiEn, KampanyaEtiketiAr);

        private static string GetLocalized(string tr, string en, string ar)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return culture switch
            {
                "ar" => !string.IsNullOrWhiteSpace(ar) ? ar : tr,
                "en" => !string.IsNullOrWhiteSpace(en) ? en : tr,
                _ => tr
            };
        }
    }
}
