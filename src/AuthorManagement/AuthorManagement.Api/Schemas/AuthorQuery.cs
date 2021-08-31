using AuthorManagement.Api.Extensions;
using AuthorManagement.Domain.AuthorAggregate;
using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Query")]
    public class AuthorQuery
    {
        private readonly IServiceProvider _serviceProvider;
        public AuthorQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [GraphQLMetadata("authors")]
        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var authorRepository = scope.ServiceProvider.GetRequiredService<IAuthorRepository>();

            var authors = await authorRepository.RetrieveAllAuthorsAsync();

            var result = authors.Select(author => author.AsAuthorType());

            return result;
        }

        [GraphQLMetadata("author")]
        public async Task<Author> GetAuthorByIdAsync(Guid id)
        {
            using var scope = _serviceProvider.CreateScope();
            var authorRepository = scope.ServiceProvider.GetRequiredService<IAuthorRepository>();

            var author = await authorRepository.RetrieveAuthorByAuthorIdAsync(id);

            var result = author.AsAuthorType();

            return result;
        }
    }
}
