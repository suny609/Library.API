using Library.API.Entities;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class BookRepository : RepositoryBase<Book, Guid>, IBookRepository
    {
        public BookRepository(DbContext dbContext) : base(dbContext) { }

        public void AddBook(BookDto book)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(BookDto book)
        {
            throw new NotImplementedException();
        }

        public async Task<Book> GetBookAsync(Guid authorId, Guid bookId)
        {
            return await DbContext.Set<Book>().FirstOrDefaultAsync(book => book.AuthorId == authorId && book.Id == bookId);
        }

        public BookDto GetBookForAuthor(Guid authorId, Guid bookId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetBooksAsync(Guid authorId)
        {
            return Task.FromResult(DbContext.Set<Book>().Where(book => book.AuthorId == authorId).AsEnumerable());
        }

        public IEnumerable<BookDto> GetBooksForAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto book)
        {
            throw new NotImplementedException();
        }

        //public void Create(Book book)
        //{
        //    base.Create
        //}
    }
}
