using System.Text.Json.Serialization;

namespace BookStore.WebApi.Extensions
{
    public static class JsonExtensions
    {
        public static IMvcBuilder AddBookStoreJsonOptions(this IMvcBuilder builder)
        {
            return builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
        }
    }
}