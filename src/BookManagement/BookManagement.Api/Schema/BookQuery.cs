using BookManagement.Api.Extensions;
using BookManagement.Domain.BookAggregate;
using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Query")]
    public class BookQuery
    {
        private readonly IServiceProvider _serviceProvider;

        public BookQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [GraphQLMetadata("books")]
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();

            var books = await bookRepository.RetrieveAllBooksAsync();

            var result = books.Select(book => book.AsBookType());

            return result;
        }

        [GraphQLMetadata("book")]
        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            using var scope = _serviceProvider.CreateScope();
            var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();

            var book = await bookRepository.RetrieveBookByBookIdAsync(id);

            var result = book.AsBookType();

            return result;
        }
    }
}
