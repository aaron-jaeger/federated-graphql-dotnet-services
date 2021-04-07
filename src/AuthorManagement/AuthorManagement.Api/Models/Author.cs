using System;
using System.Collections.Generic;

namespace AuthorManagement.Api.Models
{
    public class Author
    {
        public Author(Guid id, string firstName, string lastName, List<Book> books)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Books = books;
        }

        public Author(string firstName, string lastName, List<Book> books)
        {
            FirstName = firstName;
            LastName = lastName;
            Books = books;
        }

        public Author()
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Book> Books { get; set; }
    }
}
