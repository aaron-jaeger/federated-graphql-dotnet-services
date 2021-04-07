using System;

namespace BookManagement.Api.Models
{
    public class Book
    {
        public Book(Guid id, string title, string overview, Author author, long isbn, string publisher,
                    DateTime publicationDate, int pages)
        {
            Id = id;
            Title = title;
            Overview = overview;
            Author = author;
            Isbn = isbn;
            Publisher = publisher;
            PublicationDate = publicationDate;
            Pages = pages;
        }

        public Guid Id { get; set; }
        public string Title { get; set;  }
        public string Overview { get; set; }
        public Author Author { get; set; }
        public long Isbn { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Pages { get; set; }
    }
}
