using System.Text;
using System.Text.RegularExpressions;

namespace FilistinProje.Web.Services;

public static class PhoneNumberNormalizer
{
    private const string DefaultCountryCode = "970";

    // Add "972" here if the project owner decides to accept Israel/48-region numbers.
    private static readonly string[] AllowedCountryCodes = [DefaultCountryCode];

    private static readonly Regex PalestineNationalNumberPattern = new(@"^(?:5\d{8}|[2489]\d{7})$", RegexOptions.Compiled);

    public static bool TryNormalize(string? input, out string normalized)
    {
        normalized = string.Empty;

        var compact = Compact(input);
        if (string.IsNullOrWhiteSpace(compact))
        {
            return false;
        }

        if (compact.Count(c => c == '+') > 1 || (compact.Contains('+') && !compact.StartsWith("+", StringComparison.Ordinal)))
        {
            return false;
        }

        var hasInternationalPrefix = compact.StartsWith("+", StringComparison.Ordinal) || compact.StartsWith("00", StringComparison.Ordinal);
        var digits = compact.StartsWith("+", StringComparison.Ordinal)
            ? compact[1..]
            : compact.StartsWith("00", StringComparison.Ordinal) ? compact[2..] : compact;
        if (digits.Length == 0 || digits.Any(c => c is < '0' or > '9'))
        {
            return false;
        }

        var countryCode = ResolveCountryCode(digits, hasInternationalPrefix, out var nationalNumber);
        if (countryCode == null || !IsValidNationalNumber(countryCode, nationalNumber))
        {
            return false;
        }

        normalized = "+" + countryCode + nationalNumber;
        return true;
    }

    private static string Compact(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var builder = new StringBuilder(input.Length);
        foreach (var ch in input.Trim())
        {
            if (char.IsWhiteSpace(ch) || ch is '-' or '(' or ')')
            {
                continue;
            }

            builder.Append(ToAsciiDigit(ch));
        }

        return builder.ToString();
    }

    private static char ToAsciiDigit(char value) => value switch
    {
        >= '\u0660' and <= '\u0669' => (char)('0' + value - '\u0660'),
        >= '\u06F0' and <= '\u06F9' => (char)('0' + value - '\u06F0'),
        _ => value
    };

    private static string? ResolveCountryCode(string digits, bool hasPlus, out string nationalNumber)
    {
        nationalNumber = string.Empty;

        foreach (var allowedCountryCode in AllowedCountryCodes.OrderByDescending(x => x.Length))
        {
            if (digits.StartsWith(allowedCountryCode, StringComparison.Ordinal))
            {
                nationalNumber = digits[allowedCountryCode.Length..];
                return allowedCountryCode;
            }
        }

        if (hasPlus)
        {
            return null;
        }

        nationalNumber = digits.StartsWith("0", StringComparison.Ordinal) ? digits[1..] : digits;
        return DefaultCountryCode;
    }

    private static bool IsValidNationalNumber(string countryCode, string nationalNumber)
    {
        return countryCode switch
        {
            "970" => PalestineNationalNumberPattern.IsMatch(nationalNumber),
            _ => false
        };
    }
}
