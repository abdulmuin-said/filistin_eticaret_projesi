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
                    cssClass: "bg-[#77786f] text-white",
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
                    cssClass: "bg-red-500 text-white",
                    oncelik: OncelikStok,
                    locKey: "Badge_LowStock"
                ));
            }

            // 3. KAMPANYALI ÜRÜN KONTROLÜ
            if (urun.KampanyaliMi && urun.KampanyaBitisTarihi.HasValue &&
                urun.KampanyaBitisTarihi.Value > DateTime.UtcNow)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-purple-600 text-white",
                    oncelik: OncelikKampanya,
                    locKey: "Badge_Campaign"
                ));
            }

            // 4. İNDİRİM KONTROLÜ
            if (urun.IndirimVarMi)
            {
                int indirimYuzde = urun.IndirimYuzdesi;
                badges.Add(new ProductBadge(
                    metin: $"-{indirimYuzde}%",
                    cssClass: "bg-brand-gold text-white",
                    oncelik: OncelikIndirim,
                    locKey: "Badge_Discount"
                ));
            }

            // 5. YENİ ÜRÜN KONTROLÜ
            // Mantıksal kontrol: Ürün çok eskiyse (satış > 50, görüntüleme > 1000) yeni etiketi gösterme
            if (urun.YeniUrunMu &&
                urun.SatisSayisi < 50 &&
                urun.GoruntulenmeSayisi < 1000)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-brand-olive text-white",
                    oncelik: OncelikYeni,
                    locKey: "Badge_NewProduct"
                ));
            }

            // 6. ÖNE ÇIKAN KONTROLÜ (maks 3 etiket varsa ekleme)
            if (urun.OneCikanMi && badges.Count < 3)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-blue-600 text-white",
                    oncelik: OncelikOneCikan,
                    locKey: "Badge_Featured"
                ));
            }

            // 7. WHATSAPP SİPARİŞ KONTROLÜ
            if (urun.WhatsappSiparisVarMi && badges.Count < 4)
            {
                badges.Add(new ProductBadge(
                    metin: "",
                    cssClass: "bg-green-500 text-white",
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