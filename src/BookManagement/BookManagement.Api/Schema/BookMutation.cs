using BookManagement.Api.Application.Commands;
using BookManagement.Api.Extensions;
using GraphQL;
using MediatR;
using System;
using System.Threading.Tasks;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Mutation")]
    public class BookMutation
    {
        private readonly IMediator _mediator;

        public BookMutation(IMediator mediator)
        {
            _mediator = mediator 
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        [GraphQLMetadata("createBook")]
        public async Task<BookType> CreateBookAsync(BookInput bookInput)
        {
            var createBookCommand = new CreateBookCommand(bookInput.Title, bookInput.Overview, bookInput.AuthorId,
                bookInput.Isbn, bookInput.Publisher, bookInput.PublicationDate, bookInput.Pages);

            var book = await _mediator.Send(createBookCommand);

            var result = book.AsBookType();

            return result;
        }
    }
}
