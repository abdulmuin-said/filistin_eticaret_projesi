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

        public string VarsayilanArkaPlanRengi => CssClass
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(cssClass => cssClass switch
            {
                "product-badge--featured" => "#FDE047",
                "product-badge--new" => "#B91C1C",
                "product-badge--campaign" => "#6D28D9",
                "product-badge--discount" => "#C2410C",
                "product-badge--whatsapp" => "#047857",
                "product-badge--wholesale" => "#166534",
                "product-badge--low" => "#A21CAF",
                "product-badge--out" => "#44403C",
                "product-badge--custom" => "#313511",
                _ => string.Empty
            })
            .FirstOrDefault(color => color.Length > 0) ?? "#313511";

        public string GuvenliArkaPlanRengi => NormalizeHexColor(ArkaPlanRengi, VarsayilanArkaPlanRengi);

        public string YaziRengi
        {
            get
            {
                var backgroundLuminance = GetRelativeLuminance(GuvenliArkaPlanRengi);
                var blackContrast = (backgroundLuminance + 0.05) / 0.05;
                var whiteContrast = 1.05 / (backgroundLuminance + 0.05);
                return blackContrast >= whiteContrast ? "#000000" : "#FFFFFF";
            }
        }

        private static string NormalizeHexColor(string? color, string fallback)
        {
            var normalized = (color ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized.Length != 7 || normalized[0] != '#')
            {
                return fallback;
            }

            return int.TryParse(normalized[1..], System.Globalization.NumberStyles.HexNumber, null, out _)
                ? normalized
                : fallback;
        }

        private static double GetRelativeLuminance(string color)
        {
            static double ToLinear(int channel)
            {
                var value = channel / 255d;
                return value <= 0.04045
                    ? value / 12.92
                    : Math.Pow((value + 0.055) / 1.055, 2.4);
            }

            var red = Convert.ToInt32(color[1..3], 16);
            var green = Convert.ToInt32(color[3..5], 16);
            var blue = Convert.ToInt32(color[5..7], 16);
            return (0.2126 * ToLinear(red)) + (0.7152 * ToLinear(green)) + (0.0722 * ToLinear(blue));
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
        public decimal EskiFiyat { get; init; }
        public decimal EtkinFiyat { get; init; }
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
            var indirimYuzdesi = FilistinProje.Core.Helpers.IndirimHesaplayici.YuzdeHesapla(context.EskiFiyat, context.EtkinFiyat);

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

            if (!context.FiyatGizliMi && indirimYuzdesi.HasValue)
            {
                badges.Add(new ProductBadge($"-{indirimYuzdesi.Value}%", "product-badge--discount", 3, "Badge_Discount", context.IndirimEtiketRengi));
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

            badges.AddRange(context.DigerEtiketler
                .Where(badge => !string.IsNullOrWhiteSpace(badge.Metin) || !string.IsNullOrWhiteSpace(badge.LocalizasyonKey))
                .Select(badge =>
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
