using Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Domain.BookAggregate
{
    public interface IBookRepository 
        : IRepository<Book>
    {
        Book Create(Book book);
        Task<Book> RetrieveBookByBookIdAsync(Guid id);
        Task<IEnumerable<Book>> RetrieveAllBooksAsync();
        Task<Author> RetrieveAuthorByAuthorIdAsync(Guid id);
    }
}
