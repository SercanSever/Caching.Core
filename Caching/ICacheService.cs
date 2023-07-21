
namespace Caching.Core.InMemory
{
   public interface ICacheService<T>
   {
      void Set(object key, T value, TimeSpan slidingExpiration, DateTime absoluteExpiration);
      T Get(object key);
      Task<T> GetOrCreateAsync(object key, T value, TimeSpan slidingExpiration, DateTime absoluteExpiration);
      void Remove(object key);
      bool TryGetValue(object key, out T value);
   }
}