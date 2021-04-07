using AuthorManagement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly List<Author> _authors;

        public AuthorService()
        {
            _authors = new List<Author>
            {
                new Author(new Guid("d08a13f9-756c-4f25-8e5f-6cb7475e5246"), "Taro", "Gomi", new List<Book>()
                {
                    new Book { Id = new Guid("48de5cca-0266-40fc-a34b-22aaaff258f0")}
                })
            };
        }

        public Author GetAuthorById(Guid id)
        {
            return _authors
                .FirstOrDefault(a => a.Id == id);
        }

        public Task<Author> GetAuthorByIdAsync(Guid id)
        {
            return Task.FromResult(
                _authors
                    .FirstOrDefault(a => a.Id == id));
        }

        public Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return Task.FromResult(
                _authors
                    .AsEnumerable());
        }

        public Task<Author> AddAuthorAsync(Author author)
        {
            return Task.Run(() => 
            {
                author.Id = Guid.NewGuid();
                _authors.Add(author);
                return author;
            });
        }
    }

    public interface IAuthorService
    {
        Author GetAuthorById(Guid id);
        Task<Author> GetAuthorByIdAsync(Guid id);
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<Author> AddAuthorAsync(Author author);
    }
}
