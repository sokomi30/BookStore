using BookStore.WebApi.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Все регистрации — через методы расширения
builder.Services.AddBookStoreDatabase(builder.Configuration);
builder.Services.AddBookStoreServices();
builder.Services.AddBookStoreSwagger();

builder.Services.AddControllers()
    .AddBookStoreJsonOptions();

var app = builder.Build();

await app.UseDatabaseSeedingAsync();

app.UseBookStoreSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();