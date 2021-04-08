using GraphQL;
using System;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Author")]
    public class AuthorType
    {
        public Guid Id { get; set; }
    }
}
