using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caching.Core.InMemory;
using Caching.Core.InMemory.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Caching.Core.DIResolvers
{
   public static class CacheCoreModule
   {
      public static IServiceCollection ConfigureCaching(this IServiceCollection services)
      {
         services.AddMemoryCache();
         services.AddScoped<IMemoryCacheService<object>, MemoryCacheService<object>>();
         return services;
      }
   }
}