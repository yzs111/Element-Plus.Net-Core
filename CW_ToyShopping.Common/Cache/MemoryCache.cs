using CW_ToyShopping.Common.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.Common.Cache
{
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public long Del(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }
            return key.Length;
        }

        public Task<long> DelAsync(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }
            return Task.FromResult(key.Length.ToLong());
        }

        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        public string Get(string key)
        {
            return _memoryCache.Get(key)?.ToString();
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(Get(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public bool Set(string key, object value)
        {
            _memoryCache.Set(key, value);
            return true;
        }

        public bool Set(string key, object value, MemoryCacheEntryOptions expire)
        {
            _memoryCache.Set(key, value, expire);
            return true;
        }

        public Task<bool> SetAsync(string key, object value)
        {
            Set(key, value);
            return Task.FromResult(true);
        }

        public Task<bool> SetAsync(string key, object value, MemoryCacheEntryOptions expire)
        {
            Set(key, value, expire);
            return Task.FromResult(true);
        }

    }
}
