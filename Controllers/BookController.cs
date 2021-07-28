using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public IAuthorRepository AuthorRepository { get; }
        public IBookRepository BookRepository { get; }

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            BookRepository = bookRepository;
            AuthorRepository = authorRepository;
        }

        public ActionResult<List<BookDto>> GetBooks(Guid authorId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            return BookRepository.GetBooksForAuthor(authorId).ToList();
        }

        [HttpGet("{bookId}", Name = nameof(GetBook))]
        public ActionResult<BookDto> GetBook(Guid authorId, Guid bookId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var targetBook = BookRepository.GetBookForAuthor(authorId, bookId);

            if(targetBook == null)
            {
                return NotFound();
            }

            return targetBook;
        }

        [HttpPost]
        public IActionResult AddBook(Guid authorId, BookForCreationDto bookForCreationDto)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var newBook = new BookDto
            {
                Id = Guid.NewGuid(),
                Title = bookForCreationDto.Title,
                Description = bookForCreationDto.Description,
                Pages = bookForCreationDto.Pages,
                AuthorId = authorId
            };

            BookRepository.AddBook(newBook);

            return CreatedAtRoute(nameof(GetBook), new { authorId = authorId, bookId = newBook.Id }, newBook);
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(Guid authorId, Guid bookId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var book = BookRepository.GetBookForAuthor(authorId, bookId);

            if(book == null)
            {
                return NotFound();
            }

            BookRepository.DeleteBook(book);

            return NoContent();
        }

        [HttpPut("{bookId}")]
        public IActionResult UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto updateBook)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var book = BookRepository.GetBookForAuthor(authorId, bookId);
            
            if(book == null)
            {
                return NotFound();
            }

            BookRepository.UpdateBook(authorId, bookId, updateBook);

            return CreatedAtRoute(nameof(GetBook), new { authorId = authorId, bookId = book.Id }, book);
        }
    }
}
