using Serilog;
using BookStore.WebApi.Extensions;
using BookStore.WebApi.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddBookStoreDatabase(builder.Configuration);
    builder.Services.AddBookStoreServices(builder.Configuration);
    builder.Services.AddBookStoreSwagger();
    builder.Services.AddBookStoreRateLimiting();

    builder.Services.AddControllers()
        .AddBookStoreJsonOptions()
        .AddBookStoreValidation();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    await app.UseDatabaseSeedingAsync();

    app.UseBookStoreSwagger();
    app.UseHttpsRedirection();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}