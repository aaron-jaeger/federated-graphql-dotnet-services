using Core.Domain;
using System;

namespace BookManagement.Domain.BookAggregate
{
    public class Book 
        : Entity, IAggregateRoot
    {
        private string _title;
        public string Title => _title;

        private string _overview;
        public string Overview => _overview;

        public Author Author { get; private set; }

        private long _isbn;
        public long Isbn => _isbn;

        private string _publisher;
        public string Publisher => _publisher;

        private DateTime _publicationDate;
        public DateTime PublicationDate => _publicationDate;

        private int _pages;
        public int Pages => _pages;
        
        public Book()
        {
        }

        public Book(string title, string overview, long isbn, string publisher, DateTime publicationDate,
            int pages)
        {
            SetTitle(title);
            SetOverview(overview);
            SetIsbn(isbn);
            SetPublisher(publisher);
            SetPublicationDate(publicationDate);
            SetPages(pages);
        }

        public void SetTitle(string title)
        {
            _title = title;
        }

        public void SetOverview(string overview)
        {
            _overview = overview;
        }

        public void SetAuthor(Author author)
        {
            Author = author;
        }

        public void SetIsbn(long isbn)
        {
            _isbn = isbn;
        }

        public void SetPublisher(string publisher)
        {
            _publisher = publisher;
        }

        public void SetPublicationDate(DateTime publicationDate)
        {
            _publicationDate = publicationDate;
        }

        public void SetPages(int pages)
        {
            _pages = pages;
        }
    }
}
