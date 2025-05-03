using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text;
using System.IO;
using System.Text.Json;
using Lab12.Data;

namespace Lab12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseSamplesController : ControllerBase
    {
        // 1. Возврат JSON
        [HttpGet("json")]
        public IActionResult GetJson()
        {
            var data = new
            {
                Message = "Это JSON ответ",
                Timestamp = DateTime.Now,
                Items = new[] { "Книга 1", "Книга 2" }
            };
            return new JsonResult(data);
        }

        // 2. Возврат HTML
        [HttpGet("html")]
        public ContentResult GetHtml()
        {
            var html = $"""
                <!DOCTYPE html>
                <html>
                <head><title>HTML ответ</title></head>
                <body>
                    <h1>Привет из ASP.NET Core!</h1>
                    <p>Текущее время: {DateTime.Now}</p>
                </body>
                </html>
                """;

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };
        }

        // 3. Возврат файла
        [HttpGet("file")]
        public IActionResult GetFile()
        {
            var fileContent = "Это содержимое текстового файла\nСтрока 2\nСтрока 3";
            var byteArray = Encoding.UTF8.GetBytes(fileContent);

            return File(byteArray, "text/plain", "sample.txt");
        }

        // 4. Возврат XML
        [HttpGet("xml")]
        public IActionResult GetXml()
        {
            var xml = $"""
                <response>
                    <message>XML ответ</message>
                    <timestamp>{DateTime.Now}</timestamp>
                </response>
                """;

            return Content(xml, "application/xml");
        }

        // 5. Возврат разных статус-кодов
        [HttpGet("status/{code:int}")]
        public IActionResult GetStatus(int code)
        {
            return StatusCode(code, new
            {
                Status = code,
                Message = "Тестовый ответ с кодом " + code
            });
        }

        // 6. Возврат перенаправления
        [HttpGet("redirect")]
        public IActionResult RedirectSample()
        {
            return Redirect("https://mospolytech.ru");
        }

        // 7. Возврат данных из БД (пример с книгами)
        [HttpGet("books")]
        public IActionResult GetBooks([FromServices] AppDbContext db)
        {
            var books = db.Books.Take(3).ToList();
            return Ok(books);
        }

        // 8. Возврат PDF файла
        [HttpGet("pdf")]
        public IActionResult GetPdfFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "test.pdf");
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/pdf", "document.pdf");
        }

        // 9. Возврат JPEG изображения
        [HttpGet("image")]
        public IActionResult GetImageFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "image.jpeg");
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "image/jpeg", "image.jpeg");
        }
    }
}