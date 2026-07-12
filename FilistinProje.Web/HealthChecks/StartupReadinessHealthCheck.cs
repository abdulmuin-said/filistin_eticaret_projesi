using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FilistinProje.Web.HealthChecks;

internal sealed class StartupReadinessHealthCheck : IHealthCheck
{
    private readonly StartupReadinessState _state;

    public StartupReadinessHealthCheck(StartupReadinessState state)
    {
        _state = state;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var phase = _state.Phase;

        var result = phase switch
        {
            StartupReadinessPhase.Ready => HealthCheckResult.Healthy("ready"),
            StartupReadinessPhase.Booting => HealthCheckResult.Degraded("booting"),
            StartupReadinessPhase.DatabaseUnavailable => HealthCheckResult.Unhealthy("db_unavailable"),
            StartupReadinessPhase.SchemaDriftFailed => HealthCheckResult.Unhealthy("schema_drift_failed"),
            StartupReadinessPhase.MigrationPending => HealthCheckResult.Unhealthy("migration_pending"),
            StartupReadinessPhase.MigrationFailed => HealthCheckResult.Unhealthy("migration_failed"),
            StartupReadinessPhase.SeedFailed => HealthCheckResult.Unhealthy("seed_failed"),
            _ => HealthCheckResult.Unhealthy("unknown_phase")
        };

        return Task.FromResult(result);
    }
}
