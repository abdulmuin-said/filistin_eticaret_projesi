using FilistinProje.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilistinProje.Service.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromHours(1);
        private static readonly ConcurrentDictionary<string, byte> RegisteredKeys = new(StringComparer.Ordinal);

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? _defaultExpiration
            };
            
            _memoryCache.Set(key, value, cacheOptions);
            RegisterInvalidationKey(key);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            RegisteredKeys.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (!_memoryCache.TryGetValue(key, out T? value))
            {
                value = await factory();
                if (value != null)
                {
                    await SetAsync(key, value, expiration);
                }
            }

            return value!;
        }

        public Task RemoveByPrefixAsync(string prefix)
        {
            foreach (var key in RegisteredKeys.Keys)
            {
                if (key.StartsWith(prefix, StringComparison.Ordinal))
                {
                    _memoryCache.Remove(key);
                    RegisteredKeys.TryRemove(key, out _);
                }
            }

            return Task.CompletedTask;
        }

        public void RegisterInvalidationKey(string cacheKey)
        {
            RegisteredKeys.TryAdd(cacheKey, 0);
        }

        public void ClearRegisteredKeys()
        {
            foreach (var key in RegisteredKeys.Keys)
            {
                _memoryCache.Remove(key);
                RegisteredKeys.TryRemove(key, out _);
            }
        }
    }
}
