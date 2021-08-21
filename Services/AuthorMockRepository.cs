using Library.API.Entities;
using Library.API.Filters;
using Library.API.Helpers;
using Library.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class AuthorMockRepository : IAuthorRepository
    {
        public void AddAuthor(AuthorDto author)
        {
            author.Id = Guid.NewGuid();
            LibraryMockData.Current.Authors.Add(author);
        }

        public void Create(Author entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Author entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(AuthorDto author)
        {
            LibraryMockData.Current.Books.RemoveAll(book => book.AuthorId == author.Id);
            LibraryMockData.Current.Authors.Remove(author);
        }

        public Task<IEnumerable<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PageList<Author>> GetAllAsync(AuthorResourceParameters parameters)
        {
            throw new NotImplementedException();
        }

        public AuthorDto GetAuthor(Guid authorId)
        {
            var author = LibraryMockData.Current.Authors.FirstOrDefault(au => au.Id == authorId);
            return author;
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            return LibraryMockData.Current.Authors;
        }

        public Task<IEnumerable<Author>> GetByConditionAsync(Expression<Func<Author, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthorExists(Guid authorId)
        {
            return LibraryMockData.Current.Authors.Any(au => au.Id == authorId);
        }

        public Task<bool> IsExistAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Author entity)
        {
            throw new NotImplementedException();
        }
    }
}
