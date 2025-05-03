using Microsoft.AspNetCore.Mvc;
using Lab12;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Базовый маршрут для API
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BooksController(AppDbContext context)
        {
            _db = context;
        }

        // GET: api/books/all
        [HttpGet("all")]
        public IActionResult GetAllBooks()
        {
            var books = _db.Books.ToList();
            return Ok(books);
        }

        // GET: api/books/details/5
        [HttpGet("details/{id:int:min(1)}")]
        public IActionResult GetBookById(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/books/add
        [HttpPost("add")]
        public IActionResult AddBook([FromBody] Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        // GET: library/search?title=война
        [HttpGet("~/library/search")]
        public IActionResult SearchBooks(string title)
        {
            var books = _db.Books.Where(b => b.Title.Contains(title)).ToList();
            return Ok(books);
        }

        // GET: library/available
        [HttpGet("~/library/available")]
        public IActionResult GetAvailableBooks()
        {
            var books = _db.Books.Where(b => b.IsAvailable).ToList();
            return Ok(books);
        }

        // GET: library/genre/Роман
        [HttpGet("~/library/genre/{genre:alpha}")]
        public IActionResult GetBooksByGenre(string genre)
        {
            var books = _db.Books.Where(b => b.Genre == genre).ToList();
            return Ok(books);
        }
    }
}