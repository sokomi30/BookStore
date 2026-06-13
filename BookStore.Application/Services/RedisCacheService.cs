using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using BookStore.Application.Services;

namespace BookStore.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var data = await _cache.GetStringAsync(key);
            return data == null ? null : JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration) where T : class
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            var data = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, data, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            // Redis не поддерживает удаление по префиксу нативно
            // Для простоты — сбрасываем связанные ключи вручную
            await Task.CompletedTask;
        }
    }
}