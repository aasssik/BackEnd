using Lab12; // �������� ��� ��������� � ������ �����
using Lab12.Data; // ��� AppDbContext
using Lab12.Models; // ��� Book
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// ��������� Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// ��������� ����������� � MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// ��������� middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

// ��������� �������������
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

// ������������� ���� ������
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
                new Book { Title = "����� � ���", Author = "��� �������", Year = 1869, Genre = "�����", IsAvailable = true },
                new Book { Title = "1984", Author = "������ ������", Year = 1949, Genre = "����������", IsAvailable = false }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "������ ��� ������������� ���� ������");
    }
}

// ������� ����� Files, ���� �� ���
var filesDir = Path.Combine(Directory.GetCurrentDirectory(), "Files");
if (!Directory.Exists(filesDir))
{
    Directory.CreateDirectory(filesDir);

    // ������� �������� �����
    File.WriteAllText(Path.Combine(filesDir, "pdf1.pdf"), "PDF content placeholder");
    File.WriteAllText(Path.Combine(filesDir, "image1.jpg"), "JPEG content placeholder");
}

// ����������� API endpoints
app.MapGet("/", () => "���������� ��������! ��������� endpoints: /api/books/all, /files/pdf, /files/image");

// Endpoint ��� ��������� PDF
app.MapGet("/files/pdf", () =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "pdf1.pdf");
    if (!File.Exists(filePath))
        return Results.NotFound("PDF file not found");

    return Results.File(filePath, "application/pdf", "document.pdf");
});

// Endpoint ��� ��������� JPEG
app.MapGet("/files/image", () =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "image1.jpg");
    if (!File.Exists(filePath))
        return Results.NotFound("Image file not found");

    return Results.File(filePath, "image/jpeg", "image.jpg");
});

// Endpoint ��� ������ ���� ������
app.MapGet("/files/list", () =>
{
    var files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Files"))
        .Select(Path.GetFileName)
        .ToArray();

    return Results.Ok(files);
});

app.Run();