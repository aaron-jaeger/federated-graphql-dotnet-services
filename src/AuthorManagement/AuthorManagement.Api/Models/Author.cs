using System;
using System.Collections.Generic;

namespace AuthorManagement.Api.Models
{
    public class Author
    {
        public Author(Guid id, string firstName, string middleName, string lastName, bool isPenName, List<Author> aliases, List<Book> books)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            IsPenName = isPenName;
            Aliases = aliases;
            Books = books;
        }

        public Author()
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        public bool IsPenName { get; set; }
        public List<Author> Aliases { get; set; }
        public List<Book> Books { get; set; }
    }
}
