using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.Core.InMemory
{
   public interface ICacheService<T>
   {
      void Set(object key, T value, TimeSpan expirationTime);
      T Get(object key);
      Task<T> GetOrCreate(object key, T value);
      void Remove(object key);
      bool TryGetValue(object key, out T value);
   }
}