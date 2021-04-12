using BookManagement.Domain.BookAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Api.Application.Commands
{
    public class CreateBookCommandHandler
        : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly ILogger<CreateBookCommandHandler>_logger;
        private readonly IBookRepository _bookRepository;
        public CreateBookCommandHandler(ILogger<CreateBookCommandHandler> logger, IBookRepository bookRepository)
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));

            _bookRepository = bookRepository 
                ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Instantiating new transient {AggregateRootName} with request ({@Request}).", nameof(Book), request);
            var book = new Book(request.Title, request.Overview, request.Isbn, request.Publisher,
                request.PublicationDate, request.Pages);

            _logger.LogDebug("Determining whether entity {EntityName} with id {Id} exists.", nameof(Author), request.AuthorId);
            var author = await _bookRepository.RetrieveAuthorByAuthorIdAsync(request.AuthorId);

            if (author is null)
            {
                _logger.LogDebug("Instantiating new entity {EntityName} with id {Id}.", nameof(Author), request.AuthorId);
                author = new Author(request.AuthorId);
            }

            _logger.LogDebug("Adding entity ({@Entity}) to transient ({@AggregateRoot}).", author, book);
            book.SetAuthor(author);

            _logger.LogDebug("Saving transient ({@AggregateRoot}) as entity in db.", book);
            var result = _bookRepository.Create(book);

            _logger.LogDebug("Returning entity ({@AggregateRoot}).", result);
            return result;
        }
    }
}
