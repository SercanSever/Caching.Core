using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caching.Core.InMemory;
using Caching.Core.InMemory.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caching.Core.DIResolvers
{
   public static class CacheCoreModule
   {
      public static IServiceCollection ConfigureCaching(this IServiceCollection services, IConfiguration configuration)
      {
         //INMEMORY
         services.AddMemoryCache();
         services.AddScoped<ICacheService<object>, MemoryCacheService<object>>();
         //REDIS
         services.AddStackExchangeRedisCache(options =>
         {
            options.Configuration = "redis server URL";
         });
         return services;
      }
   }
}