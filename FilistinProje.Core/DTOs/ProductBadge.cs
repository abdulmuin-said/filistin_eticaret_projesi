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

    public sealed class ProductBadgeContext
    {
        public bool StoktaVarMi { get; init; }
        public int ToplamStok { get; init; }
        public bool KampanyaliMi { get; init; }
        public DateTime? KampanyaBitisTarihi { get; init; }
        public bool FiyatGizliMi { get; init; }
        public bool IndirimVarMi { get; init; }
        public int IndirimYuzdesi { get; init; }
        public bool ToptanFiyatVarMi { get; init; }
        public bool YeniUrunMu { get; init; }
        public bool OneCikanMi { get; init; }
        public bool WhatsappSiparisModu { get; init; }
        public string OneCikanEtiketRengi { get; init; } = "#D6AB5B";
        public string YeniUrunEtiketRengi { get; init; } = "#B33A3A";
        public string KampanyaEtiketRengi { get; init; } = "#31543B";
        public string IndirimEtiketRengi { get; init; } = "#B86A2F";
        public IEnumerable<ProductBadge> DigerEtiketler { get; init; } = [];
    }

    public static class ProductBadgeBuilder
    {
        public static List<ProductBadge> Build(ProductBadgeContext context, bool stoktaYokSatisIzni = false)
        {
            var badges = new List<ProductBadge>();
            var stoktaVar = stoktaYokSatisIzni || context.StoktaVarMi;

            if (!stoktaVar)
            {
                badges.Add(new ProductBadge("", "product-badge--out", 1, "Badge_OutOfStock"));
            }
            else if (context.ToplamStok is >= 1 and <= 4)
            {
                badges.Add(new ProductBadge("", "product-badge--low", 1, "Badge_LowStock"));
            }

            if (context.KampanyaliMi &&
                (!context.KampanyaBitisTarihi.HasValue || context.KampanyaBitisTarihi.Value > DateTime.UtcNow))
            {
                badges.Add(new ProductBadge("", "product-badge--campaign", 2, "Badge_Campaign", context.KampanyaEtiketRengi));
            }

            if (!context.FiyatGizliMi && context.IndirimVarMi)
            {
                badges.Add(new ProductBadge($"-{context.IndirimYuzdesi}%", "product-badge--discount", 3, "Badge_Discount", context.IndirimEtiketRengi));
            }

            if (context.YeniUrunMu)
            {
                badges.Add(new ProductBadge("", "product-badge--new", 4, "Badge_NewProduct", context.YeniUrunEtiketRengi));
            }

            if (context.OneCikanMi)
            {
                badges.Add(new ProductBadge("", "product-badge--featured", 5, "Badge_Featured", context.OneCikanEtiketRengi));
            }

            if (!context.FiyatGizliMi && context.ToptanFiyatVarMi)
            {
                badges.Add(new ProductBadge("", "product-badge--wholesale", 6, "Badge_Wholesale"));
            }

            if (context.WhatsappSiparisModu)
            {
                badges.Add(new ProductBadge("", "product-badge--whatsapp", 6, "Badge_WhatsappOrder"));
            }

            badges.AddRange(context.DigerEtiketler.Select(badge =>
                new ProductBadge(badge.Metin, badge.CssClass, 6, badge.LocalizasyonKey, badge.ArkaPlanRengi)));

            return badges
                .Select((badge, index) => new { Badge = badge, Index = index })
                .GroupBy(x => GetKey(x.Badge), StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .OrderBy(x => x.Badge.Oncelik)
                .ThenBy(x => x.Index)
                .Select(x => x.Badge)
                .ToList();
        }

        private static string GetKey(ProductBadge badge) =>
            !string.IsNullOrWhiteSpace(badge.LocalizasyonKey)
                ? $"loc:{badge.LocalizasyonKey.Trim()}"
                : $"text:{badge.Metin.Trim()}|css:{badge.CssClass.Trim()}";
    }
}
