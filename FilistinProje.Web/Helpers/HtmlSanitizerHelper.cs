using System.Text.RegularExpressions;

namespace FilistinProje.Web.Helpers
{
    public static class HtmlSanitizerHelper
    {
        private static readonly Regex ScriptBlockPattern = new(
            @"<\s*script[^>]*>.*?<\s*/\s*script\s*>",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex UnclosedScriptPattern = new(
            @"<\s*script[^>]*>",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex EventHandlerAttributePattern = new(
            @"\b(on\w+)\s*=\s*(""[^""]*""|'[^']*'|[^\s>]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex JavascriptUrlPattern = new(
            @"(href|src)\s*=\s*['""]?\s*(javascript|vbscript|data\s*:\s*text/html)\s*:[^'"">\s]*['""]?",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DangerousTagsPattern = new(
            @"<\s*/?\s*(iframe|object|embed|applet|form|input|select|textarea|meta|link|base)\b[^>]*>",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string Sanitize(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            var sanitized = ScriptBlockPattern.Replace(html, string.Empty);
            sanitized = UnclosedScriptPattern.Replace(sanitized, string.Empty);
            sanitized = EventHandlerAttributePattern.Replace(sanitized, string.Empty);
            sanitized = JavascriptUrlPattern.Replace(sanitized, "$1=\"#\"");
            sanitized = DangerousTagsPattern.Replace(sanitized, string.Empty);

            return sanitized;
        }
    }
}