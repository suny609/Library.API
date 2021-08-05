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

        public BookDto GetBookForAuthor(Guid authorId, Guid bookId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookDto> GetBooksForAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto book)
        {
            throw new NotImplementedException();
        }
    }
}
