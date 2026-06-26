using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace FilistinProje.Core.Varliklar
{
    public class Urun : BaseEntity
    {
        public string Baslik { get; set; } = string.Empty;
        public string BaslikEn { get; set; } = string.Empty;
        public string BaslikAr { get; set; } = string.Empty;
        public string KisaAd { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string UrlYolu { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Barkod { get; set; } = string.Empty;
        public string Marka { get; set; } = string.Empty;
        public string UrunTipi { get; set; } = "Genel";
        public string Etiketler { get; set; } = string.Empty;
        public string KisaAciklama { get; set; } = string.Empty;
        public string KisaAciklamaEn { get; set; } = string.Empty;
        public string KisaAciklamaAr { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string AciklamaEn { get; set; } = string.Empty;
        public string AciklamaAr { get; set; } = string.Empty;
        public string TeknikOzellikler { get; set; } = string.Empty;
        public string MalzemeBilgisi { get; set; } = string.Empty;
        public string BakimTalimati { get; set; } = string.Empty;
        public string PaketlemeBilgisi { get; set; } = string.Empty;
        public string AnaGorselUrl { get; set; } = string.Empty;
        public string StokDurumu { get; set; } = "Stokta";
        public decimal Fiyat { get; set; }
        public decimal? IndirimliFiyat { get; set; }
        public decimal? TopFiyat { get; set; } // Toptan satış fiyatı (Wholesale Price)
        public bool HediyePaketiVarMi { get; set; }
        public decimal HediyePaketFiyati { get; set; }
        public bool WhatsappSiparisVarMi { get; set; }
        public bool FiyatGizliMi { get; set; }
        public decimal Maliyet { get; set; }
        public decimal KdvOrani { get; set; } = 20;
        public int UretimSuresiGun { get; set; }
        public int KargoyaVerilisSuresiGun { get; set; }
        public int TahminiTeslimSuresiGun { get; set; }
        public bool AktifMi { get; set; } = true;
        public bool YayindaMi { get; set; } = true;
        public bool OneCikanMi { get; set; }
        public bool YeniUrunMu { get; set; }
        public bool KampanyaliMi { get; set; }
        public bool AnaSayfadaGoster { get; set; }
        public int Sira { get; set; }
        public int GoruntulenmeSayisi { get; set; }
        public int SatisSayisi { get; set; }
        public int FavoriSayisi { get; set; }
        public int MinSiparisAdedi { get; set; } = 1;
        public int? MaxSiparisAdedi { get; set; }
        public DateTime? KampanyaBitisTarihi { get; set; }
        public string SeoTitle { get; set; } = string.Empty;
        public string SeoTitleEn { get; set; } = string.Empty;
        public string SeoTitleAr { get; set; } = string.Empty;
        public string SeoDescription { get; set; } = string.Empty;
        public string SeoDescriptionEn { get; set; } = string.Empty;
        public string SeoDescriptionAr { get; set; } = string.Empty;
        public string SeoKeywords { get; set; } = string.Empty;

        public int KategoriId { get; set; }
        public Kategori? Kategori { get; set; }

        public int? ToptanciUrunGrubuId { get; set; }
        public ToptanciUrunGrubu? ToptanciUrunGrubu { get; set; }

        public virtual ICollection<UrunResim> UrunResimleri { get; set; } = new List<UrunResim>();
        public virtual ICollection<UrunSecenek> UrunSecenek { get; set; } = new List<UrunSecenek>();
        public virtual ICollection<UrunOzellikDegeri> UrunOzellikleri { get; set; } = new List<UrunOzellikDegeri>();

        [NotMapped]
        public decimal EtkinFiyat =>
            IndirimliFiyat.HasValue && IndirimliFiyat.Value > 0 && IndirimliFiyat.Value < Fiyat
                ? IndirimliFiyat.Value
                : Fiyat;

        [NotMapped]
        public bool IndirimVarMi =>
            IndirimliFiyat.HasValue && IndirimliFiyat.Value > 0 && IndirimliFiyat.Value < Fiyat;

        [NotMapped]
        public int IndirimYuzdesi =>
            IndirimVarMi ? (int)Math.Round((1 - (IndirimliFiyat!.Value / Fiyat)) * 100) : 0;

        [NotMapped]
        public decimal EtkinTopFiyat =>
            TopFiyat.HasValue && TopFiyat.Value > 0 ? TopFiyat.Value : EtkinFiyat;

        [NotMapped]
        public int ToplamStok =>
            UrunSecenek?.Where(x => !x.SilindiMi && x.AktifMi).Sum(x => x.StokAdedi) ?? 0;

        [NotMapped]
        public bool StoktaVarMi =>
            (UrunSecenek?.Any(x =>
                !x.SilindiMi &&
                x.SatinAlinabilirMi &&
                (!x.TukeninceGizle || x.StokAdedi > 0 || x.OnSipariseAcikMi)) ?? false)
            || string.Equals(StokDurumu, "Stokta", System.StringComparison.OrdinalIgnoreCase)
            || ToplamStok > 0;

        [NotMapped]
        public string LocalizedBaslik => GetLocalized(Baslik, BaslikEn, BaslikAr);

        [NotMapped]
        public string LocalizedKisaAciklama => GetLocalized(KisaAciklama, KisaAciklamaEn, KisaAciklamaAr);

        [NotMapped]
        public string LocalizedAciklama => GetLocalized(Aciklama, AciklamaEn, AciklamaAr);

        [NotMapped]
        public string LocalizedSeoTitle => GetLocalized(SeoTitle, SeoTitleEn, SeoTitleAr);

        [NotMapped]
        public string LocalizedSeoDescription => GetLocalized(SeoDescription, SeoDescriptionEn, SeoDescriptionAr);

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
