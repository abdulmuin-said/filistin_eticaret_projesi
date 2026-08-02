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
        public string ArkaPlanRengi { get; set; } = string.Empty;

        public string YaziRengi
        {
            get
            {
                if (ArkaPlanRengi.Length != 7 || ArkaPlanRengi[0] != '#')
                {
                    return "#FFFFFF";
                }

                if (!int.TryParse(ArkaPlanRengi[1..3], System.Globalization.NumberStyles.HexNumber, null, out var red)
                    || !int.TryParse(ArkaPlanRengi[3..5], System.Globalization.NumberStyles.HexNumber, null, out var green)
                    || !int.TryParse(ArkaPlanRengi[5..7], System.Globalization.NumberStyles.HexNumber, null, out var blue))
                {
                    return "#FFFFFF";
                }

                var luminance = (0.299 * red) + (0.587 * green) + (0.114 * blue);
                return luminance > 160 ? "#18231B" : "#FFFFFF";
            }
        }

        public ProductBadge() { }

        public ProductBadge(string metin, string cssClass, int oncelik, string locKey = "", string arkaPlanRengi = "")
        {
            Metin = metin;
            CssClass = cssClass;
            Oncelik = oncelik;
            LocalizasyonKey = locKey;
            ArkaPlanRengi = arkaPlanRengi;
        }
    }
}
