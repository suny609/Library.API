using Library.API.Entities;
using Library.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class BookMockRepository : IBookRepository
    {
        public void AddBook(BookDto book)
        {
            LibraryMockData.Current.Books.Add(book);
        }

        public void Create(Book entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Book entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(BookDto book)
        {
            LibraryMockData.Current.Books.Remove(book);
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookAsync(Guid authorId, Guid bookId)
        {
            throw new NotImplementedException();
        }

        public BookDto GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return LibraryMockData.Current.Books.FirstOrDefault(b => b.AuthorId == authorId && b.Id == bookId);
        }

        public Task<IEnumerable<Book>> GetBooksAsync(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookDto> GetBooksForAuthor(Guid authorId)
        {
            return LibraryMockData.Current.Books.Where(b => b.AuthorId == authorId);
        }

        public Task<IEnumerable<Book>> GetByConditionAsync(Expression<Func<Book, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Book entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto book)
        {
            var originalBook = GetBookForAuthor(authorId, bookId);

            originalBook.Title = book.Title;
            originalBook.Pages = book.Pages;
            originalBook.Description = book.Description;
        }
    }
}
