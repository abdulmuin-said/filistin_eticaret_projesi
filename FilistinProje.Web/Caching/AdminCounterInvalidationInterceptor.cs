using System.Runtime.CompilerServices;
using FilistinProje.Core.Varliklar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace FilistinProje.Web.Caching;

public sealed class AdminCounterInvalidationInterceptor : SaveChangesInterceptor
{
    private sealed record PendingInvalidation(bool Orders, bool Messages);

    private readonly IMemoryCache _cache;
    private readonly ConditionalWeakTable<DbContext, PendingInvalidation> _pending = new();

    public AdminCounterInvalidationInterceptor(IMemoryCache cache)
    {
        _cache = cache;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        Capture(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Capture(eventData.Context);
        return ValueTask.FromResult(result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        Invalidate(eventData.Context);
        return result;
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        Invalidate(eventData.Context);
        return ValueTask.FromResult(result);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData) =>
        Clear(eventData.Context);

    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        Clear(eventData.Context);
        return Task.CompletedTask;
    }

    private void Capture(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        var orders = context.ChangeTracker.Entries<Siparis>()
            .Any(entry => entry.State is EntityState.Added or EntityState.Deleted ||
                          (entry.State == EntityState.Modified && entry.Property(x => x.Durum).IsModified));
        var messages = context.ChangeTracker.Entries<IletisimMesaj>()
            .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        _pending.Remove(context);
        if (orders || messages)
        {
            _pending.Add(context, new PendingInvalidation(orders, messages));
        }
    }

    private void Invalidate(DbContext? context)
    {
        if (context == null || !_pending.TryGetValue(context, out var pending))
        {
            return;
        }

        if (pending.Orders)
        {
            _cache.InvalidateOrders();
        }

        if (pending.Messages)
        {
            _cache.InvalidateMessages();
        }

        _pending.Remove(context);
    }

    private void Clear(DbContext? context)
    {
        if (context != null)
        {
            _pending.Remove(context);
        }
    }
}
