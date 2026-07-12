using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FilistinProje.Web.HealthChecks;

internal static class HealthCheckResponseWriter
{
    public static Task WriteLiveness(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        return context.Response.WriteAsync("alive");
    }

    public static Task WriteReadiness(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var payload = new
        {
            status = report.Status.ToString(),
            phase = nameof(StartupReadinessPhase),
            results = report.Entries.ToDictionary(
                kvp => kvp.Key,
                kvp => new
                {
                    status = kvp.Value.Status.ToString(),
                    description = kvp.Value.Description,
                    durationMs = (long)kvp.Value.Duration.TotalMilliseconds
                })
        };

        return context.Response.WriteAsJsonAsync(payload);
    }
}
