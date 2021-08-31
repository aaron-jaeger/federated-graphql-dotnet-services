using BookManagement.Domain.BookAggregate;
using MediatR;
using System;

namespace BookManagement.Api.Application.Commands
{
    public class CreateBookCommand
        : IRequest<Book>
    {
        public string Title { get; private set; }
        public string Overview { get; private set; }
        public Guid AuthorId { get; private set; }
        public long Isbn { get; private set; }
        public string Publisher { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public int Pages { get; private set; }

        public CreateBookCommand(string title, string overview, Guid authorId, long isbn, string publisher, DateTime publicationDate, int pages)
        {
            Title = title;
            Overview = overview;
            AuthorId = authorId;
            Isbn = isbn;
            Publisher = publisher;
            PublicationDate = publicationDate;
            Pages = pages;
        }
    }
}
