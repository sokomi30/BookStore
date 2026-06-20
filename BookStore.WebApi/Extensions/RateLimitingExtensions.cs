using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace BookStore.WebApi.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddBookStoreRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("LoginPolicy", config =>
                {
                    config.PermitLimit = 5;
                    config.Window = TimeSpan.FromMinutes(1);
                    config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    config.QueueLimit = 0;
                });
            });

            return services;
        }
    }
}