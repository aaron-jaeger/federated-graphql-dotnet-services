using BookManagement.Api.Models;
using BookManagement.Api.Services;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Query")]
    public class BookQuery
    {
        private readonly IBookService _books;

        public BookQuery(IBookService books)
        {
            _books = books;
        }

        [GraphQLMetadata("books")]
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _books.GetBooksAsync();
        }

        [GraphQLMetadata("book")]
        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _books.GetBookByIdAsync(id);
        }
    }
}
