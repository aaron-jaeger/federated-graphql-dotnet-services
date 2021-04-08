using BookManagement.Api.Schema;
using BookManagement.Domain.BookAggregate;

namespace BookManagement.Api.Extensions
{
    public static class BookExtensions
    {
        public static BookType AsBookType(this Book input)
        {
            var bookType = new BookType
            {
                Id = input.Id,
                Title = input.Title,
                Overview = input.Overview,
                Author = input.Author.AsAuthorType(),
                Isbn = input.Isbn,
                Publisher = input.Publisher,
                PublicationDate = input.PublicationDate,
                Pages = input.Pages
            };

            return bookType;
        }

        public static AuthorType AsAuthorType(this Author input)
        {
            var authorType = new AuthorType
            {
                Id = input.Id
            };

            return authorType;
        }
    }
}
