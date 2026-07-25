namespace FilistinProje.Core.DTOs
{
    /// <summary>
    /// Ürün kartında gösterilecek bir etiket/badge.
    /// Backend'de oluşturulur, View'da sadece render edilir.
    /// Localization key'i ile view'da Localizer kullanılarak metin gösterilir.
    /// </summary>
    public class ProductBadge
    {
        public string Metin { get; set; } = string.Empty;
        public string CssClass { get; set; } = "bg-brand-olive text-white";
        public int Oncelik { get; set; }
        public string LocalizasyonKey { get; set; } = string.Empty;

        public ProductBadge() { }

        public ProductBadge(string metin, string cssClass, int oncelik, string locKey = "")
        {
            Metin = metin;
            CssClass = cssClass;
            Oncelik = oncelik;
            LocalizasyonKey = locKey;
        }
    }
}