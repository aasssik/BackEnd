var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "�������");
app.MapGet("/faq", () => "FAQ");
app.MapGet("/contacts", () => "��������");
app.Run();
