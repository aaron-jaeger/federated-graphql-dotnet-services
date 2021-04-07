using BookManagement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.Api.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books;
        
        public BookService()
        {
            _books = new List<Book>()
            {
                new Book(new Guid("48de5cca-0266-40fc-a34b-22aaaff258f0"), "Everybody Poops", "A book about pooping", new Author
                {
                    Id = new Guid("d08a13f9-756c-4f25-8e5f-6cb7475e5246")
                })
            };
        }

        public Book GetBookById(Guid id)
        {
            return _books.FirstOrDefault(
                b => b.Id == id);
        }

        public Task<Book> GetBookByIdAsync(Guid id)
        {
            return Task.FromResult(
                _books.FirstOrDefault(b => b.Id == id));
        }

        public Task<IEnumerable<Book>> GetBooksAsync()
        {
            return Task.FromResult(
                _books.AsEnumerable());
        }
    }

    public interface IBookService
    {
        Book GetBookById(Guid id);
        Task<Book> GetBookByIdAsync(Guid id);
        Task<IEnumerable<Book>> GetBooksAsync();
    }
}
