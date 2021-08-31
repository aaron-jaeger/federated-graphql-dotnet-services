using GraphQL;
using System;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Book")]
    public class BookType
    {
        public Guid Id { get; set; }
    }
}
