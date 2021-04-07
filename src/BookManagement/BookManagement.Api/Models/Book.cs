using System;

namespace BookManagement.Api.Models
{
    public class Book
    {
        public Book(Guid id, string title, string description, Author author)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }

        public Guid Id { get; set; }
        public string Title { get; set;  }
        public string Description { get; set; }
        public Author Author { get; set; }
    }
}
