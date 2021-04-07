using GraphQL;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("AuthorInput")]
    public class AuthorInput
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
