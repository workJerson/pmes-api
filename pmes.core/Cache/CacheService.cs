using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Cache
{
    public class CacheService(IDistributedCache cache)
    {
        private readonly IDistributedCache cache = cache;

        public async Task<string?> Get(string key)
        {
            return await cache.GetStringAsync(key);
        }

        public async Task Set(string key, string data)
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            await cache.SetStringAsync(key, data, cacheEntryOptions);
        }

        public async Task InvalidateCache(string cacheKey)
        {
            await cache.RemoveAsync(cacheKey);
        }
    }
}
