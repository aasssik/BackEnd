var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Главная");
app.MapGet("/faq", () => "FAQ");
app.MapGet("/contacts", () => "Контакты");
app.Run();
