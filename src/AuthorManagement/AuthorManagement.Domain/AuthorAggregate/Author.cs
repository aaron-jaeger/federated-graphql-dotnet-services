using Core.Domain;
using System.Collections.Generic;

namespace AuthorManagement.Domain.AuthorAggregate
{
    public class Author
        : Entity, IAggregateRoot
    {
        private string _firstName;
        public string FirstName => _firstName;
        private string _middleName;
        public string MiddleName => _middleName;
        private string _lastName;
        public string LastName => _lastName;
        public string FullName => string.IsNullOrWhiteSpace(MiddleName) 
            ? $"{FirstName} {LastName}" 
            : $"{FirstName} {MiddleName} {LastName}";
        public List<Book> Books { get; private set; }

        public Author(string firstName, string middleName, string lastName)
        {
            SetFirstName(firstName);
            SetMiddleName(middleName);
            SetLastName(lastName);
        }

        public void SetFirstName(string firstName)
        {
            _firstName = firstName;
        }

        public void SetMiddleName(string middleName)
        {
            _middleName = middleName;
        }

        public void SetLastName(string lastName)
        {
            _lastName = lastName;
        }

        public void AddBook(Book book)
        {
            if (Books is null)
                Books = new List<Book>();

            Books.Add(book);
        }
    }
}
