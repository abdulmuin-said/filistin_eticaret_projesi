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
    }
}
