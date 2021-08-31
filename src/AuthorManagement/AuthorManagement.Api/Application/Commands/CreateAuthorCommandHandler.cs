using AuthorManagement.Domain.AuthorAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Application.Commands
{
    public class CreateAuthorCommandHandler
        : IRequestHandler<CreateAuthorCommand, Author>
    {
        private readonly ILogger<CreateAuthorCommandHandler> _logger;
        private readonly IAuthorRepository _authorRepository;

        public CreateAuthorCommandHandler(ILogger<CreateAuthorCommandHandler> logger, IAuthorRepository authorRepository)
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));

            _authorRepository = authorRepository
                ?? throw new ArgumentNullException(nameof(authorRepository));
        }

        public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Instantiating new transient {AggregateRootName} with request ({@Request}).", nameof(Author), request);
            var author = new Author(request.FirstName, request.MiddleName, request.LastName);

            foreach (var bookId in request.BookIds)
            {

                _logger.LogDebug("Instantiating new entity {EntityName} with id {Id}.", nameof(Book), bookId);
                var book = new Book(bookId);

                _logger.LogDebug("Adding entity ({@Entity}) to transient ({@AggregateRoot}).", book, author);
                author.AddBook(book);
            }     

            _logger.LogDebug("Saving transient ({@AggregateRoot}) as entity in db.", author);
            var result = _authorRepository.Create(author);

            _logger.LogDebug("Returning entity ({@AggregateRoot}).", result);
            return await Task.FromResult(result);
        }
    }
}
