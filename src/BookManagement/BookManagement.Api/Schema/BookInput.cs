using GraphQL;
using System;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("BookInput")]
    public class BookInput
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public Guid AuthorId { get; set; }
        public long Isbn { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Pages { get; set; }
    }
}
