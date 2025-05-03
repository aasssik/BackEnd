using Lab12;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// 1. Добавляем сервисы в контейнер
builder.Services.AddControllers();

// 2. Настройка подключения к MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 3. Настройка конвейера HTTP-запросов
app.UseHttpsRedirection();
app.UseAuthorization();

// 4. Настройка маршрутизации
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "library",
    pattern: "library/{action=Index}/{id?}",
    defaults: new { controller = "Books" });

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller=Books}/{action=GetBooks}/{id?}");

// 5. Инициализация базы данных с миграциями
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        if (!context.Books.Any())
        {
            context.Books.AddRange(
                new Book { Title = "Война и мир", Author = "Лев Толстой", Year = 1869, Genre = "Роман", IsAvailable = true },
                new Book { Title = "1984", Author = "Джордж Оруэлл", Year = 1949, Genre = "Антиутопия", IsAvailable = false }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при инициализации базы данных");
    }
}

app.MapGet("/", () => "Приложение работает! Проверьте API: /api/books/all");

app.Run();

