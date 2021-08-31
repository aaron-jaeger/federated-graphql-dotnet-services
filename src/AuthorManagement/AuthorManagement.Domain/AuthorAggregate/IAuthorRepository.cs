using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorManagement.Domain.AuthorAggregate
{
    public interface IAuthorRepository
    {
        Author Create(Author book);
        Task<Author> RetrieveAuthorByAuthorIdAsync(Guid id);
        Task<IEnumerable<Author>> RetrieveAllAuthorsAsync();
    }
}
