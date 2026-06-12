using BookStore.Application.Mappings;
using BookStore.Application.Services;
using BookStore.Application.Validators;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Services;
using FluentValidation;

namespace BookStore.WebApi.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddBookStoreServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BookProfile>();
                cfg.AddProfile<AuthorProfile>();
            });

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();

            // Сервисы
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();

            // Сидер
            services.AddTransient<IDataSeeder>(_ => new BookStoreSeeder(100));

            return services;
        }
    }
}