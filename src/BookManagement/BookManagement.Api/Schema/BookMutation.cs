using BookManagement.Api.Application.Commands;
using BookManagement.Api.Extensions;
using GraphQL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Mutation")]
    public class BookMutation
    {
        private readonly IServiceProvider _serviceProvider;

        public BookMutation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [GraphQLMetadata("createBook")]
        public async Task<BookType> CreateBookAsync(BookInput bookInput)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<BookMutation>>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            logger.LogInformation("Begin processing mutation with input ({@Input}).", bookInput);

            var createBookCommand = new CreateBookCommand(bookInput.Title, bookInput.Overview, bookInput.AuthorId,
                bookInput.Isbn, bookInput.Publisher, bookInput.PublicationDate, bookInput.Pages);

            logger.LogDebug("Sending command {CommandName} ({@Command})", nameof(CreateBookCommand), createBookCommand);
            var book = await mediator.Send(createBookCommand);

            logger.LogDebug("Converting entity ({@AggregateRoot}) to response type {ResponseType}.", book, nameof(BookType));
            var result = book.AsBookType();

            logger.LogDebug("Returning mutation result ({@Result})", result);
            return result;
        }
    }
}
