using LibraryAPI.DAL;
using LibraryAPI.DTOs.Book;
using LibraryAPI.Models;
using LibraryAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class BookController : ControllerBase
    {
        private AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _context.Books.Where(bk => bk.IsDeleted == false).FirstOrDefaultAsync(bk => bk.Id == id);
            if (book is null) return StatusCode(StatusCodes.Status404NotFound, new { errorCode = 1045, message = "This book could not found" });
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books.Where(bk => bk.IsDeleted == false).ToListAsync();
            if (books is null) return StatusCode(StatusCodes.Status404NotFound, new { errorCode = 1046, message = "This books could not found" });
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateBookDTOs bookDTO)
        {

            BookValidator validator = new BookValidator();

            var book = new Book()
            {
                Name = bookDTO.Name,
                Price = bookDTO.Price,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            var validationResult = validator.Validate(book);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAsync(int id, UpdateBookDTOs UpdateBook)
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest, new { errorCode = 7000, message = "Model is invalid" });

            var DbBook = await _context.Books.Where(bk => bk.Id == id && !bk.IsDeleted).FirstOrDefaultAsync();
            if (DbBook is null) return NotFound();
            DbBook.Name = UpdateBook.Name ?? DbBook.Name;
            DbBook.Price = UpdateBook.Price == 0 ? DbBook.Price : UpdateBook.Price;
            _context.Books.Update(DbBook);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var DbBook = await _context.Books.Where(bk => bk.Id == id && !bk.IsDeleted).FirstOrDefaultAsync();
            if (DbBook is null) return NotFound();
            DbBook.IsDeleted = true;
            _context.Update(DbBook);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
