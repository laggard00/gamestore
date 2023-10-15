using Microsoft.Extensions.Caching.Memory;

namespace GameStore_v2.Middleware
{
    public class GamesCountMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "GameCache";

        public GamesCountMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call along the request pipeline
            await _next(context);

            if (context.Response.Headers.ContainsKey("Game-Cache-Counter")
                && int.TryParse(context.Response.Headers["Game-Cache-Counter"], out int gamesCount))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(CacheKey, gamesCount, cacheEntryOptions);
            }
        }
    }
}
