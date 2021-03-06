using BookManagement.Domain.BookAggregate;
using GraphQL;
using System;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Book")]
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public Author Author { get; set; }
        public long Isbn { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Pages { get; set; }
    }
}
