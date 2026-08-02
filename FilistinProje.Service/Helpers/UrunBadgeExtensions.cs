using FilistinProje.Core.DTOs;
using FilistinProje.Core.Varliklar;

namespace FilistinProje.Service.Helpers
{
    /// <summary>
    /// Ürün etiketlerini (badge) oluşturmak için helper.
    /// Mantıksız kombinasyonları engeller ve maks 4 etiket sınırı koyar.
    /// </summary>
    public static class UrunBadgeExtensions
    {
        // Öncelik sırası: Stok durumu > Kampanya > İndirim > Yeni > Öne Çıkan
        private const int OncelikStok = 1;
        private const int OncelikKampanya = 2;
        private const int OncelikIndirim = 3;
        private const int OncelikYeni = 4;
        private const int OncelikOneCikan = 5;
        private const int OncelikWhatsapp = 6;
        private const int OncelikToptan = 4;

        /// <summary>
        /// Bir Urun entity'sinden frontend'de gösterilecek badge listesini oluşturur.
        /// Maks 4 etiket döner, mantıksız kombinasyonlar engellenir.
        /// </summary>
        public static List<ProductBadge> ToBadges(this Urun urun, bool stoktaYokSatisIzni = false)
        {
            var badges = new List<ProductBadge>();

            // 1. STOK DURUMU KONTROLÜ (en yüksek öncelik)
            bool stoktaVar = stoktaYokSatisIzni || urun.StoktaVarMi;

            if (!stoktaVar)
            {
                // Stok yoksa sadece stok etiketi göster, kampanya/indirim/yeni gösterme
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "product-badge--out",
                    oncelik: OncelikStok,
                    locKey: "Badge_OutOfStock"
                ));
                return badges;
            }

            // 2. AZ STOK KONTROLÜ
            if (urun.ToplamStok >= 1 && urun.ToplamStok <= 4)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "product-badge--low",
                    oncelik: OncelikStok,
                    locKey: "Badge_LowStock"
                ));
            }

            // 3. KAMPANYALI ÜRÜN KONTROLÜ
            if (urun.KampanyaliMi &&
                (!urun.KampanyaBitisTarihi.HasValue || urun.KampanyaBitisTarihi.Value > DateTime.UtcNow))
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "product-badge--campaign",
                    oncelik: OncelikKampanya,
                    locKey: "Badge_Campaign",
                    arkaPlanRengi: urun.KampanyaEtiketRengi
                ));
            }

            // 4. İNDİRİM KONTROLÜ
            if (urun.IndirimVarMi)
            {
                int indirimYuzde = urun.IndirimYuzdesi;
                badges.Add(new ProductBadge(
                    metin: $"-{indirimYuzde}%",
                    cssClass: "product-badge--discount",
                    oncelik: OncelikIndirim,
                    locKey: "Badge_Discount",
                    arkaPlanRengi: urun.IndirimEtiketRengi
                ));
            }

            if (urun.TopFiyat.HasValue && urun.TopFiyat.Value > 0)
            {
                badges.Add(new ProductBadge("", "product-badge--wholesale", OncelikToptan, "Badge_Wholesale"));
            }

            // 5. YENİ ÜRÜN KONTROLÜ
            if (urun.YeniUrunMu)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "product-badge--new",
                    oncelik: OncelikYeni,
                    locKey: "Badge_NewProduct",
                    arkaPlanRengi: urun.YeniUrunEtiketRengi
                ));
            }

            // 6. ÖNE ÇIKAN KONTROLÜ (maks 3 etiket varsa ekleme)
            if (urun.OneCikanMi && badges.Count < 3)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "product-badge--featured",
                    oncelik: OncelikOneCikan,
                    locKey: "Badge_Featured",
                    arkaPlanRengi: urun.OneCikanEtiketRengi
                ));
            }

            // 7. WHATSAPP SİPARİŞ KONTROLÜ
            if (urun.WhatsappSiparisVarMi && badges.Count < 4)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "product-badge--whatsapp",
                    oncelik: OncelikWhatsapp,
                    locKey: "Badge_WhatsappOrder"
                ));
            }

            // Önceliğe göre sırala ve maks 4 al
            return badges
                .OrderBy(b => b.Oncelik)
                .Take(4)
                .ToList();
        }
    }
}
