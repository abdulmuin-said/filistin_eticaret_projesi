using System.Text.RegularExpressions;

namespace FilistinProje.Core.Helpers
{
    public static partial class VaryantRenkYardimcisi
    {
        public const string NotrRenkKodu = "#6B7280";

        private static readonly IReadOnlyDictionary<string, string> TemelRenkler =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["red"] = "#DC2626",
                ["kırmızı"] = "#DC2626",
                ["kirmizi"] = "#DC2626",
                ["أحمر"] = "#DC2626",
                ["احمر"] = "#DC2626",
                ["green"] = "#16A34A",
                ["yeşil"] = "#16A34A",
                ["yesil"] = "#16A34A",
                ["أخضر"] = "#16A34A",
                ["اخضر"] = "#16A34A",
                ["black"] = "#111827",
                ["siyah"] = "#111827",
                ["أسود"] = "#111827",
                ["اسود"] = "#111827",
                ["white"] = "#FFFFFF",
                ["beyaz"] = "#FFFFFF",
                ["أبيض"] = "#FFFFFF",
                ["ابيض"] = "#FFFFFF",
                ["blue"] = "#2563EB",
                ["mavi"] = "#2563EB",
                ["أزرق"] = "#2563EB",
                ["ازرق"] = "#2563EB",
                ["yellow"] = "#FACC15",
                ["sarı"] = "#FACC15",
                ["sari"] = "#FACC15",
                ["أصفر"] = "#FACC15",
                ["اصفر"] = "#FACC15",
                ["orange"] = "#F97316",
                ["turuncu"] = "#F97316",
                ["برتقالي"] = "#F97316",
                ["purple"] = "#7C3AED",
                ["mor"] = "#7C3AED",
                ["بنفسجي"] = "#7C3AED",
                ["pink"] = "#EC4899",
                ["pembe"] = "#EC4899",
                ["وردي"] = "#EC4899",
                ["gray"] = "#6B7280",
                ["grey"] = "#6B7280",
                ["gri"] = "#6B7280",
                ["رمادي"] = "#6B7280",
                ["brown"] = "#92400E",
                ["kahverengi"] = "#92400E",
                ["بني"] = "#92400E",
                ["beige"] = "#D6C6A5",
                ["bej"] = "#D6C6A5",
                ["بيج"] = "#D6C6A5"
            };

        private static readonly IReadOnlyDictionary<string, (string Ar, string En)> RenkCevirileri =
            new Dictionary<string, (string Ar, string En)>(StringComparer.OrdinalIgnoreCase)
            {
                ["red"] = ("أحمر", "Red"),
                ["kırmızı"] = ("أحمر", "Red"),
                ["kirmizi"] = ("أحمر", "Red"),
                ["أحمر"] = ("أحمر", "Red"),
                ["احمر"] = ("أحمر", "Red"),

                ["green"] = ("أخضر", "Green"),
                ["yeşil"] = ("أخضر", "Green"),
                ["yesil"] = ("أخضر", "Green"),
                ["أخضر"] = ("أخضر", "Green"),
                ["اخضر"] = ("أخضر", "Green"),

                ["black"] = ("أسود", "Black"),
                ["siyah"] = ("أسود", "Black"),
                ["أسود"] = ("أسود", "Black"),
                ["اسود"] = ("أسود", "Black"),

                ["white"] = ("أبيض", "White"),
                ["beyaz"] = ("أبيض", "White"),
                ["أبيض"] = ("أبيض", "White"),
                ["ابيض"] = ("أبيض", "White"),

                ["blue"] = ("أزرق", "Blue"),
                ["mavi"] = ("أزرق", "Blue"),
                ["أزرق"] = ("أزرق", "Blue"),
                ["ازرق"] = ("أزرق", "Blue"),

                ["yellow"] = ("أصفر", "Yellow"),
                ["sarı"] = ("أصفر", "Yellow"),
                ["sari"] = ("أصفر", "Yellow"),
                ["أصفر"] = ("أصفر", "Yellow"),
                ["اصفر"] = ("أصفر", "Yellow"),

                ["orange"] = ("برتقالي", "Orange"),
                ["turuncu"] = ("برتقالي", "Orange"),
                ["برتقالي"] = ("برتقالي", "Orange"),

                ["purple"] = ("بنفسجي", "Purple"),
                ["mor"] = ("بنفسجي", "Purple"),
                ["بنفسجي"] = ("بنفسجي", "Purple"),

                ["pink"] = ("وردي", "Pink"),
                ["pembe"] = ("وردي", "Pink"),
                ["وردي"] = ("وردي", "Pink"),

                ["gray"] = ("رمادي", "Gray"),
                ["grey"] = ("رمادي", "Gray"),
                ["gri"] = ("رمادي", "Gray"),
                ["رمادي"] = ("رمادي", "Gray"),

                ["brown"] = ("بني", "Brown"),
                ["kahverengi"] = ("بني", "Brown"),
                ["بني"] = ("بني", "Brown"),

                ["beige"] = ("بيج", "Beige"),
                ["bej"] = ("بيج", "Beige"),
                ["بيج"] = ("بيج", "Beige")
            };

        public static string GetLocalizedRenk(string? renkAdi, bool isAr)
        {
            if (string.IsNullOrWhiteSpace(renkAdi))
            {
                return string.Empty;
            }

            var normalized = NormalizeName(renkAdi);
            if (RenkCevirileri.TryGetValue(normalized, out var pair))
            {
                return isAr ? pair.Ar : pair.En;
            }

            return renkAdi.Trim();
        }

        public static bool TryNormalizeHex(string? renkKodu, out string normalized)
        {
            var value = renkKodu?.Trim() ?? string.Empty;
            var match = GuvenliHexRegex().Match(value);
            if (!match.Success)
            {
                normalized = string.Empty;
                return false;
            }

            var hex = match.Groups[1].Value.ToUpperInvariant();
            if (hex.Length == 3)
            {
                hex = string.Concat(hex.Select(character => $"{character}{character}"));
            }

            normalized = $"#{hex}";
            return true;
        }

        public static string Resolve(string? renkKodu, string? renkAdi)
        {
            if (TryNormalizeHex(renkKodu, out var normalized))
            {
                return normalized;
            }

            var normalizedName = NormalizeName(renkAdi);
            return TemelRenkler.TryGetValue(normalizedName, out var mappedColor)
                ? mappedColor
                : NotrRenkKodu;
        }

        public static string NormalizeForPersistence(string? renkKodu, string? renkAdi)
        {
            return string.IsNullOrWhiteSpace(renkKodu) && string.IsNullOrWhiteSpace(renkAdi)
                ? string.Empty
                : Resolve(renkKodu, renkAdi);
        }

        private static string NormalizeName(string? renkAdi)
        {
            return string.Join(' ', (renkAdi ?? string.Empty)
                .Trim()
                .ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        [GeneratedRegex("^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$", RegexOptions.CultureInvariant)]
        private static partial Regex GuvenliHexRegex();
    }
}
