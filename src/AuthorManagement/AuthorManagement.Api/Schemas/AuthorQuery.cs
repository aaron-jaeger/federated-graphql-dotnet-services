using AuthorManagement.Api.Models;
using AuthorManagement.Api.Services;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Query")]
    public class AuthorQuery
    {
        private readonly IAuthorService _authors;
        public AuthorQuery(IAuthorService authors)
        {
            _authors = authors;
        }

        [GraphQLMetadata("authors")]
        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _authors.GetAuthorsAsync();
        }

        [GraphQLMetadata("author")]
        public async Task<Author> GetAuthorByIdAsync(Guid id)
        {
            return await _authors.GetAuthorByIdAsync(id);
        }
    }
}
