using AuthorManagement.Api.Schemas;
using AuthorManagement.Domain.AuthorAggregate;
using System.Linq;

namespace AuthorManagement.Api.Extensions
{
    public static class AuthorExtensions
    {
        public static AuthorType AsAuthorType(this Author input)
        {
            var authorType = new AuthorType
            {
                Id = input.Id,
                FirstName = input.FirstName,
                MiddleName = input.MiddleName,
                LastName = input.LastName,
                FullName = input.FullName,
                Books = input.Books?.Select(book => book.AsBookType()).ToList()
            };

            return authorType;
        }

        public static BookType AsBookType(this Book input)
        {
            var bookType = new BookType
            {
                Id = input.Id
            };

            return bookType;
        }
    }
}
