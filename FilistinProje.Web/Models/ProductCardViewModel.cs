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

        // Ekstra: kart davranışını değiştiren opsiyonel bayraklar
        public string? BadgeText { get; set; }      // Özel rozet metni (ör: "5 Parça")
        public bool ShowFavori { get; set; } = true; // Favori butonu gösterilsin mi?
        public bool ShowQuickAdd { get; set; } = true; // Sepete Ekle overlay gösterilsin mi?
        public bool WhatsappSiparisVarMi { get; set; }

        // Ürün kartında gösterilecek etiketler (maks 4 adet, öncelik sırasına göre)
        public List<ProductBadge> Etiketler { get; set; } = new();

        // Admin bayrakları (etiket oluşturmak için kullanılır)
        public bool OneCikanMi { get; set; }
        public bool KampanyaliMi { get; set; }

        public List<ProductBadge> ToBadges(bool stoktaYokSatisIzni = false)
        {
            var badges = new List<ProductBadge>();

            bool stoktaVar = stoktaYokSatisIzni || StoktaVarMi;

            if (!stoktaVar)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-[#77786f] text-white",
                    oncelik: 1,
                    locKey: "Badge_OutOfStock"
                ));
                return badges;
            }

            if (ToplamStok >= 1 && ToplamStok <= 4)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-red-500 text-white",
                    oncelik: 1,
                    locKey: "Badge_LowStock"
                ));
            }

            if (KampanyaliMi && KampanyaBitisTarihi.HasValue && KampanyaBitisTarihi.Value > DateTime.UtcNow)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-purple-600 text-white",
                    oncelik: 2,
                    locKey: "Badge_Campaign"
                ));
            }

            if (IndirimVarMi)
            {
                badges.Add(new ProductBadge(
                    metin: $"-{IndirimYuzdesi}%",
                    cssClass: "bg-brand-gold text-white",
                    oncelik: 3,
                    locKey: "Badge_Discount"
                ));
            }

            if (YeniUrunMu)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-brand-olive text-white",
                    oncelik: 4,
                    locKey: "Badge_NewProduct"
                ));
            }

            if (OneCikanMi && badges.Count < 3)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-blue-600 text-white",
                    oncelik: 5,
                    locKey: "Badge_Featured"
                ));
            }

            if (WhatsappSiparisVarMi && badges.Count < 4)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-green-500 text-white",
                    oncelik: 6,
                    locKey: "Badge_WhatsappOrder"
                ));
            }

            return badges
                .OrderBy(b => b.Oncelik)
                .Take(4)
                .ToList();
        }
    }
}
