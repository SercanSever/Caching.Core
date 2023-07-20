using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caching.Core.InMemory;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Caching.Core.Caching.Redis
{
   public class RedisCacheService<T> : ICacheService<T>
   {
      private readonly IDistributedCache _distributedCache;

      public RedisCacheService(IDistributedCache distributedCache)
      {
         _distributedCache = distributedCache;
      }

      public T Get(object key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         var cachedValue = _distributedCache.GetString(key.ToString());

         return JsonConvert.DeserializeObject<T>(cachedValue);
      }

      public async Task<T> GetOrCreateAsync(object key, T value, TimeSpan slidingExpiration, DateTime absoluteExpiration)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         var cachedValue = await _distributedCache.GetStringAsync(key.ToString());

         if (cachedValue != null)
         {
            return JsonConvert.DeserializeObject<T>(cachedValue);
         }

         var cacheEntryOptions = new DistributedCacheEntryOptions
         {
            AbsoluteExpiration = absoluteExpiration,
            SlidingExpiration = slidingExpiration
         };

         await _distributedCache.SetStringAsync(key.ToString(), JsonConvert.SerializeObject(value), cacheEntryOptions);

         return value;
      }

      public void Remove(object key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         _distributedCache.Remove(key.ToString());
      }

      public void Set(object key, T value, TimeSpan slidingExpiration, DateTime absoluteExpiration)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         var cacheEntryOptions = new DistributedCacheEntryOptions
         {
            AbsoluteExpiration = absoluteExpiration,
            SlidingExpiration = slidingExpiration
         };

         _distributedCache.SetString(key.ToString(), JsonConvert.SerializeObject(value), cacheEntryOptions);
      }

      public bool TryGetValue(object key, out T value)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key), "The key is not exists");

         var cachedValue = _distributedCache.GetString(key.ToString());

         if (cachedValue != null)
         {
            value = JsonConvert.DeserializeObject<T>(cachedValue);
            return true;
         }

         value = default(T);
         return false;
      }
   }
}