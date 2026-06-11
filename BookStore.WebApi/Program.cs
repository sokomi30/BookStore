using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BookStore.Infrastructure.Data;
using BookStore.Application.Validators;
using BookStore.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BookStoreDb"));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BookProfile>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed данных - одна строка вместо кучи кода
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();