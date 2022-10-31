using CW_ToyShopping.Common.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_ToyShopping.Extensions
{
    public class RequestRateLimitingMiddleware
    {
        private const int Limit = 100; // 请求最大次数
        //一个可以处理HTTP请求的函数。
        private RequestDelegate _next;

        private ICache requestStore;
        public RequestRateLimitingMiddleware(RequestDelegate next, ICache cache)
        {
            _next = next;
            requestStore = cache;
        }
        public async Task Invoke(HttpContext context)
        {
            var requsetKey = $"{context.Request.Method}-{context.Request.Path}";

            int hitCount = 0;

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };

            if(!string.IsNullOrWhiteSpace(await requestStore.GetAsync(requsetKey)))
            {
                hitCount = Convert.ToInt32(await requestStore.GetAsync(requsetKey));
                if (hitCount < Limit)
                {
                    await ProcessRequest(context, requsetKey, hitCount, cacheOptions);
                }
                else
                {
                    context.Response.Headers["X-RateLimit-RetryAfter"] = cacheOptions.AbsoluteExpiration?.ToString();
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                }
            }
            else
            {
                await ProcessRequest(context, requsetKey, hitCount, cacheOptions);
            }
        }

        private async Task ProcessRequest(HttpContext context,string requestKey,int hitCount, MemoryCacheEntryOptions cacjeOptions)
        {
            hitCount++;
            await requestStore.SetAsync(requestKey, hitCount, cacjeOptions);

            context.Response.Headers["X-RateLimit-Limit"] = Limit.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = (Limit - hitCount).ToString();

            await _next(context);
        }
    }
}
