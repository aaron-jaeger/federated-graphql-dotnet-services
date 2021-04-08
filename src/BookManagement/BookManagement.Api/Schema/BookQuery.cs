using BookManagement.Domain.BookAggregate;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Query")]
    public class BookQuery
    {
        private readonly IBookRepository _books;

        public BookQuery(IBookRepository books)
        {
            _books = books;
        }

        [GraphQLMetadata("books")]
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _books.RetrieveAllAsync();
        }

        [GraphQLMetadata("book")]
        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _books.RetrieveByIdAsync(id);
        }
    }
}
