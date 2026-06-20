using Serilog;
using BookStore.WebApi.Extensions;
using BookStore.WebApi.Middleware;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog ������ ���� �� � ������
    if (!builder.Environment.IsEnvironment("Test"))
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    builder.Services.AddBookStoreDatabase(builder.Configuration);
    builder.Services.AddBookStoreServices(builder.Configuration);
    builder.Services.AddBookStoreSwagger();
    builder.Services.AddBookStoreRateLimiting();

    builder.Services.AddControllers()
        .AddBookStoreJsonOptions()
        .AddBookStoreValidation();

    // CORS: только указанные домены
    var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]?.Split(",") 
        ?? new[] { "http://localhost:3000" };
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("BookStorePolicy", policy =>
        {
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    if (!builder.Environment.IsEnvironment("Test"))
    {
        app.UseSerilogRequestLogging();
    }

    app.UseCors("BookStorePolicy");

    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    await app.UseDatabaseSeedingAsync();

    app.UseBookStoreSwagger();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
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

public partial class Program { }