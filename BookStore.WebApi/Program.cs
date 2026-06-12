using BookStore.WebApi.Middleware;
using BookStore.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBookStoreDatabase(builder.Configuration);
builder.Services.AddBookStoreServices();
builder.Services.AddBookStoreSwagger();

builder.Services.AddControllers()
    .AddBookStoreJsonOptions()
    .AddBookStoreValidation(); // ← валидация

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); // ← middleware ошибок

await app.UseDatabaseSeedingAsync();

app.UseBookStoreSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();