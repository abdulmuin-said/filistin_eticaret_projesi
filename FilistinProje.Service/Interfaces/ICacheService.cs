using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilistinProje.Service.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        
        Task RemoveByPrefixAsync(string prefix);
        
        void RegisterInvalidationKey(string cacheKey);
        void ClearRegisteredKeys();
    }
}
