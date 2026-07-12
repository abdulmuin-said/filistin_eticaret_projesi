using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FilistinProje.Web.Diagnostics;

public sealed class RequestSqlProfiler : DbCommandInterceptor
{
    private static readonly AsyncLocal<RequestSqlMetrics?> CurrentMetrics = new();

    public static IDisposable BeginScope()
    {
        var previous = CurrentMetrics.Value;
        CurrentMetrics.Value = new RequestSqlMetrics();
        return new Scope(previous);
    }

    public static RequestSqlSnapshot Snapshot()
    {
        var metrics = CurrentMetrics.Value;
        return metrics == null
            ? new RequestSqlSnapshot(0, TimeSpan.Zero)
            : new RequestSqlSnapshot(metrics.QueryCount, metrics.Elapsed);
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        Record(eventData.Duration);
        return base.ReaderExecuted(command, eventData, result);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        Record(eventData.Duration);
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        Record(eventData.Duration);
        return base.NonQueryExecuted(command, eventData, result);
    }

    public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        Record(eventData.Duration);
        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override object? ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object? result)
    {
        Record(eventData.Duration);
        return base.ScalarExecuted(command, eventData, result);
    }

    public override ValueTask<object?> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object? result, CancellationToken cancellationToken = default)
    {
        Record(eventData.Duration);
        return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    private static void Record(TimeSpan duration)
    {
        var metrics = CurrentMetrics.Value;
        if (metrics == null)
        {
            return;
        }

        metrics.QueryCount++;
        metrics.Elapsed += duration;
    }

    private sealed class RequestSqlMetrics
    {
        public int QueryCount { get; set; }
        public TimeSpan Elapsed { get; set; }
    }

    private sealed class Scope : IDisposable
    {
        private readonly RequestSqlMetrics? _previous;

        public Scope(RequestSqlMetrics? previous)
        {
            _previous = previous;
        }

        public void Dispose()
        {
            CurrentMetrics.Value = _previous;
        }
    }
}

public readonly record struct RequestSqlSnapshot(int QueryCount, TimeSpan Elapsed);
