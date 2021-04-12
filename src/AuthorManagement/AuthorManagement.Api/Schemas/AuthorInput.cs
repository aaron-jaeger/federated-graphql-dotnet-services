using GraphQL;
using System;
using System.Collections.Generic;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("AuthorInput")]
    public class AuthorInput
    {

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public List<Guid> BookIds { get; set; }
    }
}
