using FilistinProje.Core.DTOs;

namespace FilistinProje.Web.Models
{
    /// <summary>
    /// Ürün kartı partial view'ı için hafif DTO.
    /// Urun entity'sinin tüm navigation property'lerini taşımadan
    /// sadece kart gösterimi için gereken alanları içerir.
    /// </summary>
    public class ProductCardViewModel
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string GorselUrl { get; set; } = string.Empty;
        public decimal Fiyat { get; set; }
        public decimal? IndirimliFiyat { get; set; }
        public decimal? TopFiyat { get; set; }
        public bool IndirimVarMi { get; set; }
        public int IndirimYuzdesi { get; set; }
        public bool YeniUrunMu { get; set; }
        public bool StoktaVarMi { get; set; }
        public int ToplamStok { get; set; }
        public bool FiyatGizliMi { get; set; }
        public DateTime? KampanyaBitisTarihi { get; set; }
        public bool IsWholesale { get; set; }

        public string? BadgeText { get; set; }
        public bool ShowFavori { get; set; } = true;
        public bool ShowQuickAdd { get; set; } = true;
        public bool WhatsappSiparisVarMi { get; set; }
        public bool WhatsappSiparisModu => FiyatGizliMi || WhatsappSiparisVarMi;

        public List<ProductBadge> Etiketler { get; set; } = new();

        public bool OneCikanMi { get; set; }
        public bool KampanyaliMi { get; set; }
        public string OneCikanEtiketRengi { get; set; } = "#D6AB5B";
        public string YeniUrunEtiketRengi { get; set; } = "#B33A3A";
        public string KampanyaEtiketRengi { get; set; } = "#31543B";
        public string IndirimEtiketRengi { get; set; } = "#B86A2F";

        public List<ProductBadge> ToBadges(bool stoktaYokSatisIzni = false)
        {
            var digerEtiketler = Etiketler.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(BadgeText))
            {
                digerEtiketler = digerEtiketler.Append(new ProductBadge(BadgeText, "product-badge--custom", 6));
            }

            return ProductBadgeBuilder.Build(new ProductBadgeContext
            {
                StoktaVarMi = StoktaVarMi,
                ToplamStok = ToplamStok,
                KampanyaliMi = KampanyaliMi,
                KampanyaBitisTarihi = KampanyaBitisTarihi,
                FiyatGizliMi = FiyatGizliMi,
                IndirimVarMi = IndirimVarMi,
                IndirimYuzdesi = IndirimYuzdesi,
                ToptanFiyatVarMi = TopFiyat.HasValue && TopFiyat.Value > 0,
                YeniUrunMu = YeniUrunMu,
                OneCikanMi = OneCikanMi,
                WhatsappSiparisModu = WhatsappSiparisModu,
                OneCikanEtiketRengi = OneCikanEtiketRengi,
                YeniUrunEtiketRengi = YeniUrunEtiketRengi,
                KampanyaEtiketRengi = KampanyaEtiketRengi,
                IndirimEtiketRengi = IndirimEtiketRengi,
                DigerEtiketler = digerEtiketler
            }, stoktaYokSatisIzni);
        }
    }
}
