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
            var everyonePoops = new Book
            {
                Id = new Guid("48de5cca-0266-40fc-a34b-22aaaff258f0")
            };

            var expanseBooks = new List<Book>()
            {
                new Book
                {
                    Id = new Guid("af5370ae-33f8-48b9-b5a0-eeb81190181b")
                },
                new Book
                {
                    Id = new Guid("85ce8816-5fbf-443a-b512-b37ede78911a")
                },
                new Book
                {
                    Id = new Guid("b4d706b1-5c58-478e-8158-44ab50759af7")
                }
            };

            var jamesSACoreyAliases = new List<Author>()
            {
                new Author(new Guid("21c34f3d-089b-4e06-bc0d-cde8ffa16d3d"), "Daniel", null, "Abraham", false, new List<Author>(), new List<Book>()),
                new Author(new Guid("3c52ce95-5569-4836-8b0b-2e79cffaafb4"), "Ty", null, "Franck", false, new List<Author>(), new List<Book>())
            };

            _authors = new List<Author>
            {
                new Author(new Guid("d08a13f9-756c-4f25-8e5f-6cb7475e5246"), "Taro", null, "Gomi", false, new List<Author>(), new List<Book>() { everyonePoops }),
                new Author(new Guid("2f775d9a-835e-4674-9b68-8ad29ccba6d6"), "James", "S.A.", "Corey", true, jamesSACoreyAliases, expanseBooks)
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
