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

      public T Get(object key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         if (_memoryCache.TryGetValue<T>(key, out var value))
            return value;

         throw new Exception("The data you are looking for does not exist.");
      }

      public async Task<T> GetOrCreateAsync(object key, T value, TimeSpan slidingExpiration, DateTime absoluteExpiration)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         if (value == null)
            throw new ArgumentNullException(nameof(value), "The value is not exists");

         return await _memoryCache.GetOrCreateAsync<T>(key, entry =>
         {
            entry.Value = value;
            entry.SlidingExpiration = slidingExpiration;
            entry.AbsoluteExpiration = absoluteExpiration;

            return Task.FromResult(value);
         }) ?? throw new Exception("The data could not be found or created...");
      }

      public void Remove(object key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         _memoryCache.Remove(key);
      }

      public void Set(object key, T value, TimeSpan slidingExpiration, DateTime absoluteExpiration)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         var memoryCacheEntryOptions = new MemoryCacheEntryOptions
         {
            SlidingExpiration = slidingExpiration,
            AbsoluteExpiration = absoluteExpiration
         };

         _memoryCache.Set<T>(key, value, memoryCacheEntryOptions);
      }

      public bool TryGetValue(object key, out T value)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         return _memoryCache.TryGetValue(key, out value) && value != null;
      }
   }
}