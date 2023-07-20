using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caching.Core.InMemory;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Caching.Core.Caching.Redis
{
   public class RedisCacheService<T> : ICacheService<T>
   {
      private readonly IConnectionMultiplexer _redisConnection;
      private readonly IDatabase _cache;
      private TimeSpan ExpireTime => TimeSpan.FromDays(1);
      public RedisCacheService(IConnectionMultiplexer redisConnection, IDatabase cache)
      {
         _redisConnection = redisConnection;
         _cache = _redisConnection.GetDatabase();
      }
      public T Get(object key)
      {
         var value = _cache.StringGet(key.ToString());
         return JsonConvert.DeserializeObject<T>(value);
      }

      public async Task<T> GetOrCreate(object key, T value)
      {
         var result = _cache.StringGet(key.ToString());
         if (string.IsNullOrEmpty(result))
         {
            Set(key, JsonConvert.DeserializeObject<T>(value.ToString()), ExpireTime);
         }
         return JsonConvert.DeserializeObject<T>(result);
      }

      public void Remove(object key)
      {
         _cache.KeyDelete(key.ToString());
      }

      public void Set(object key, T value, TimeSpan expirationTime)
      {
         _cache.StringSetAsync(key.ToString(), value.ToString(), ExpireTime);
      }

      public bool TryGetValue(object key, out T value)
      {
         throw new NotImplementedException();
      }
   }
}