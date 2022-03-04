using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace InMemoryCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public CacheController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet("{key}")]
        public IActionResult GetCache(string key)
        {
            string value = string.Empty;
            memoryCache.TryGetValue(key, out value);

            return Ok(value);
        }

        // Takes a key and a value
        [HttpPost]
        public IActionResult SetCache(CacheRequest data)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                // set the actual expiration regardless of sliding
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                // a cache entry will expire if not used within this time
                SlidingExpiration = TimeSpan.FromMinutes(2), 
                Size = 1024,
            };
            memoryCache.Set(data.key, data.value, cacheExpiryOptions);

            return Ok();
        }

        public class CacheRequest
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}
