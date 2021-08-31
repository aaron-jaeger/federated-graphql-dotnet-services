using BookManagement.Api.Schema;
using BookManagement.Domain.BookAggregate;

namespace BookManagement.Api.Extensions
{
    public static class BookExtensions
    {
        public static Schema.Book AsBookType(this Domain.BookAggregate.Book input)
        {
            var bookType = new Schema.Book
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

        public static Schema.Author AsAuthorType(this Domain.BookAggregate.Author input)
        {
            var authorType = new Schema.Author
            {
                Id = input.Id
            };

            return authorType;
        }
    }
}
