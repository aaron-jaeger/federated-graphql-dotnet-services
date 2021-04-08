using AuthorManagement.Api.Models;
using AuthorManagement.Api.Services;
using GraphQL;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Mutation")]
    public class AuthorMutation
    {
        private readonly IAuthorService _service;
        public AuthorMutation(IAuthorService service)
        {
            _service = service;
        }

        [GraphQLMetadata("createAuthor")]
        public Task<Author> AddAuthorAsync(AuthorInput input)
        {
            var author = new Author();
            return _service.AddAuthorAsync(author);
        }
    }
}
