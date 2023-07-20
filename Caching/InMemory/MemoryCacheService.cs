using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.Core.InMemory.Concrete
{
   public class MemoryCacheService<T> : ICacheService<T>
   {
      private readonly IMemoryCache _memoryCache;

      public MemoryCacheService(IMemoryCache memoryCache)
      {
         _memoryCache = memoryCache;
      }

      public T Get(object key) => _memoryCache.Get<T>(key) ?? throw new Exception("The data you are looking for does not exists.");

      public async Task<T> GetOrCreate(object key, T value) => await _memoryCache.GetOrCreateAsync<T>(
      key ?? throw new Exception("The key is not exists"),
      entry => Task.FromResult(value) ?? throw new Exception("The value is not exists"))
      ?? throw new Exception("The data could not be found or created...");

      public void Remove(object key) => _memoryCache.Remove(key);

      public void Set(object key, T value, TimeSpan expirationTime) => _memoryCache.Set<T>(key, value, expirationTime);

      public bool TryGetValue(object key, out T value) => (_memoryCache.TryGetValue(key, out value) && value != null);
   }
}