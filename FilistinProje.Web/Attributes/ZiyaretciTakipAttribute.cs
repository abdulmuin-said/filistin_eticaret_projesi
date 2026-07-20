using System.IO;
using System.Text.RegularExpressions;
using FilistinProje.Web.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilistinProje.Web.Attributes
{
    public class ZiyaretciTakipAttribute : ActionFilterAttribute
    {
        private readonly IVisitorTrackingQueue _queue;

        private static readonly Regex SensitiveQueryParamPattern = new(
            @"\b(token|password|sifre|secret|key|code|auth|hash|signature|reset|confirm|verif|credential)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public ZiyaretciTakipAttribute(IVisitorTrackingQueue queue)
        {
            _queue = queue;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            if (ShouldSkip(request))
            {
                await next();
                return;
            }

            var ipAdresi = ResolveIpAddress(context);
            var userAgent = request.Headers.UserAgent.ToString();
            var cihazBilgi = CihazModeliBul(userAgent);
            var sanitizedPath = SanitizeLogPath(request);
            await next();

            _queue.TryEnqueue(new VisitorTrackingEntry(
                ipAdresi,
                Truncate(sanitizedPath, 1000) ?? string.Empty,
                request.Method,
                Truncate(request.Headers.Referer.ToString(), 500),
                Truncate(userAgent, 512) ?? string.Empty,
                cihazBilgi.Tarayici,
                cihazBilgi.OS,
                cihazBilgi.Model,
                    context.HttpContext.User.Identity?.IsAuthenticated == true
                        ? Truncate(context.HttpContext.User.Identity.Name, 256)
                        : "زائر",
                DateTime.UtcNow));
        }

        private static string SanitizeLogPath(HttpRequest request)
        {
            var path = request.Path.Value ?? string.Empty;

            if (!request.QueryString.HasValue)
                return path;

            var queryString = request.QueryString.Value ?? string.Empty;
            if (SensitiveQueryParamPattern.IsMatch(queryString))
                return path + "?[filtered]";

            return path + queryString;
        }

        private static bool ShouldSkip(HttpRequest request)
        {
            var path = request.Path.Value ?? string.Empty;
            return path.Contains("/admin", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/health", StringComparison.OrdinalIgnoreCase)
                || !HttpMethods.IsGet(request.Method)
                || Path.HasExtension(path)
                || IsLikelyBot(request.Headers.UserAgent.ToString());
        }

        private static string ResolveIpAddress(FilterContext context)
        {
            return context.HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "غير معروف";
        }

        private static bool IsLikelyBot(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return true;
            }

            string[] markers = ["bot", "crawler", "spider", "slurp", "headless", "monitor", "healthcheck"];
            return markers.Any(marker => userAgent.Contains(marker, StringComparison.OrdinalIgnoreCase));
        }

        private static string? Truncate(string? value, int maximumLength) =>
            string.IsNullOrEmpty(value)
                ? value
                : value.Length <= maximumLength ? value : value[..maximumLength];

        private static (string Tarayici, string OS, string Model) CihazModeliBul(string agent)
        {
            var os = "غير معروف";
            var browser = "غير معروف";
            var model = "PC / غير معروف";

            if (agent.Contains("Windows", StringComparison.OrdinalIgnoreCase)) os = "Windows";
            else if (agent.Contains("Android", StringComparison.OrdinalIgnoreCase)) os = "Android";
            else if (agent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) || agent.Contains("iPad", StringComparison.OrdinalIgnoreCase)) os = "iOS";
            else if (agent.Contains("Mac", StringComparison.OrdinalIgnoreCase)) os = "MacOS";
            else if (agent.Contains("Linux", StringComparison.OrdinalIgnoreCase)) os = "Linux";

            if (agent.Contains("Edg", StringComparison.OrdinalIgnoreCase)) browser = "Edge";
            else if (agent.Contains("Chrome", StringComparison.OrdinalIgnoreCase)) browser = "Chrome";
            else if (agent.Contains("Firefox", StringComparison.OrdinalIgnoreCase)) browser = "Firefox";
            else if (agent.Contains("Safari", StringComparison.OrdinalIgnoreCase)) browser = "Safari";
            else if (agent.Contains("Opera", StringComparison.OrdinalIgnoreCase) || agent.Contains("OPR", StringComparison.OrdinalIgnoreCase)) browser = "Opera";

            if (os == "Android")
            {
                var match = Regex.Match(agent, @";\s?([^;]+)\sBuild");
                model = match.Success ? match.Groups[1].Value.Trim() : "جهاز Android";
            }
            else if (os == "iOS")
            {
                model = agent.Contains("iPad", StringComparison.OrdinalIgnoreCase) ? "iPad" : "iPhone";
            }
            else if (os == "Windows")
            {
                model = "PC (Windows)";
            }
            else if (os == "MacOS")
            {
                model = "Macbook / iMac";
            }

            return (browser, os, model);
        }
    }
}
