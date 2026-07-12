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

        var digits = compact.StartsWith("+", StringComparison.Ordinal) ? compact[1..] : compact;
        if (digits.Length == 0 || digits.Any(c => !char.IsDigit(c)))
        {
            return false;
        }

        var countryCode = ResolveCountryCode(digits, compact.StartsWith("+", StringComparison.Ordinal), out var nationalNumber);
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

            builder.Append(ch);
        }

        return builder.ToString();
    }

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
