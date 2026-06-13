using BookStore.WebApi.Extensions;
using BookStore.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBookStoreDatabase(builder.Configuration);
builder.Services.AddBookStoreServices(builder.Configuration);
builder.Services.AddBookStoreSwagger();
builder.Services.AddBookStoreRateLimiting();

builder.Services.AddControllers()
    .AddBookStoreJsonOptions()
    .AddBookStoreValidation();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

await app.UseDatabaseSeedingAsync();

app.UseBookStoreSwagger();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }