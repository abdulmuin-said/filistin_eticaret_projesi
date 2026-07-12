namespace FilistinProje.Web.HealthChecks;

public enum StartupReadinessPhase
{
    Booting,
    DatabaseUnavailable,
    SchemaDriftFailed,
    MigrationPending,
    MigrationFailed,
    SeedFailed,
    Ready
}

public sealed class StartupReadinessState
{
    private readonly object _gate = new();

    public StartupReadinessPhase Phase { get; private set; } = StartupReadinessPhase.Booting;

    public string? LastErrorType { get; private set; }

    public string? LastErrorMessage { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; } = DateTime.UtcNow;

    public bool IsReady => Phase == StartupReadinessPhase.Ready;

    public void Transition(StartupReadinessPhase phase, Exception? error = null)
    {
        lock (_gate)
        {
            Phase = phase;
            UpdatedAtUtc = DateTime.UtcNow;
            if (error != null)
            {
                LastErrorType = error.GetType().FullName;
                LastErrorMessage = error.Message;
            }
        }
    }
}
