using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Extensions
{
    public static class ValidationExtensions
    {
        public static IMvcBuilder AddBookStoreValidation(this IMvcBuilder builder)
        {
            return builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value!.Errors.Count > 0)
                        .SelectMany(e => e.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new
                    {
                        status = 400,
                        title = "Validation Failed",
                        errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}