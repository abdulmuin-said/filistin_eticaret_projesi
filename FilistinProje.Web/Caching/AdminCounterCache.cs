using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace FilistinProje.Web.Caching;

public static class AdminCounterCacheKeys
{
    public const string PendingOrders = "admin:counter:pending-orders:v1";
    public const string UnreadMessages = "admin:counter:unread-messages:v1";
    public const string OnlineVisitors = "admin:counter:online-visitors:v1";
    public static readonly TimeSpan MaximumStaleTime = TimeSpan.FromMinutes(1);
}

public static class AdminCounterCache
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> KeyLocks = new();

    public static async Task<int> GetOrCreateAsync(
        IMemoryCache cache,
        string key,
        Func<Task<int>> factory)
    {
        if (cache.TryGetValue(key, out int cached))
        {
            return cached;
        }

        var keyLock = KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await keyLock.WaitAsync();
        try
        {
            if (cache.TryGetValue(key, out cached))
            {
                return cached;
            }

            var value = await factory();
            cache.Set(key, value, AdminCounterCacheKeys.MaximumStaleTime);
            return value;
        }
        finally
        {
            keyLock.Release();
        }
    }

    public static void InvalidateOrders(this IMemoryCache cache) =>
        cache.Remove(AdminCounterCacheKeys.PendingOrders);

    public static void InvalidateMessages(this IMemoryCache cache) =>
        cache.Remove(AdminCounterCacheKeys.UnreadMessages);
}
