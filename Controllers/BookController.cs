using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
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
    [ServiceFilter(typeof(CheckAuthorExistFilterAttribute))]
    public class BookController : ControllerBase
    {
        public IRepositoryWrapper RepositoryWrapper { get; }

        public IMapper Mapper { get; }

        public BookController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetBooksAsync(Guid authorId)
        {
            var books = await RepositoryWrapper.Book.GetBooksAsync(authorId);
            var bookDtoList = Mapper.Map<IEnumerable<BookDto>>(books);

            return bookDtoList.ToList();
        }

        [HttpGet("{bookId}", Name = nameof(GetBookAsync))]
        public ActionResult<BookDto> GetBookAsync(Guid authorId, Guid bookId)
        {

            var targetBook = RepositoryWrapper.Book.GetBookAsync(authorId, bookId);

            if(targetBook == null)
            {
                return NotFound();
            }

            var bookDto = Mapper.Map<BookDto>(targetBook);

            return bookDto;
        }

        [HttpPost]
        public async Task<IActionResult> AddBookAsync(Guid authorId, BookForCreationDto bookForCreationDto)
        {
            var book = Mapper.Map<Book>(bookForCreationDto);

            book.AuthorId = authorId;

            RepositoryWrapper.Book.Create(book);

            if(!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("创建资源book失败");
            }

            var bookDto = Mapper.Map<BookDto>(book);

            return CreatedAtRoute(nameof(GetBookAsync), new { authorId = authorId, bookId = bookDto.Id }, bookDto);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookAsync(Guid authorId, Guid bookId)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorId, bookId);

            if(book == null)
            {
                return NotFound();
            }

            RepositoryWrapper.Book.Delete(book);

            if(!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("删除资源book失败");
            }

            return NoContent();
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBookAsync(Guid authorId, Guid bookId, BookForUpdateDto updateBook)
        {
            var book = await RepositoryWrapper.Book.GetBookAsync(authorId, bookId);
            
            if(book == null)
            {
                return NotFound();
            }

            Mapper.Map(updateBook, book, typeof(BookForUpdateDto), typeof(Book));

            RepositoryWrapper.Book.Update(book);

            if(!await RepositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("更新资源book失败");
            }

            return CreatedAtRoute(nameof(GetBookAsync), new { authorId = authorId, bookId = book.Id }, book);
        }
    }
}
