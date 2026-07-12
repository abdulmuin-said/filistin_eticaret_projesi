using System.Text.RegularExpressions;

namespace FilistinProje.Web.Helpers
{
    public static class HtmlSanitizerHelper
    {
        private static readonly Regex ActiveContentPattern = new(
            @"<\s*(script|iframe|object|embed|applet|meta|link[^>]*rel\s*=\s*['""]?\s*stylesheet)[\s>]|" +
            @"\bon\w+\s*=\s*|" +
            @"javascript\s*:|" +
            @"vbscript\s*:|" +
            @"data\s*:\s*text/html|" +
            @"<\s*!--\s*\[if\b|" +
            @"<\s*form\b|" +
            @"<\s*input\b[^>]*type\s*=\s*['""]?\s*(hidden|file)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ScriptBlockPattern = new(
            @"<script[\s>].*?</script\s*>",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex EventHandlerAttributePattern = new(
            @"\b(on\w+)\s*=\s*(""[^""]*""|'[^']*'|[^\s>]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string Sanitize(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var sanitized = ScriptBlockPattern.Replace(html, string.Empty);
            sanitized = EventHandlerAttributePattern.Replace(sanitized, string.Empty);
            sanitized = ActiveContentPattern.Replace(sanitized, string.Empty);

            return sanitized;
        }
    }
}