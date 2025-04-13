using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Data;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BooksController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult AddBook(string title, string author, int year)
        {
            Book book = new Book(title, author, year);
            _db.Books.Add(book);
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _db.Books.ToList();
            return Ok(books);
        }
    }
}