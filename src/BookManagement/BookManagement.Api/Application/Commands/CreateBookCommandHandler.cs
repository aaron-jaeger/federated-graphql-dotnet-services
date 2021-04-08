using BookManagement.Domain.BookAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Api.Application.Commands
{
    public class CreateBookCommandHandler
        : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly IBookRepository _bookRepository;
        public CreateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository 
                ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken = default)
        {
            var book = new Book(request.Title, request.Overview, request.Isbn, request.Publisher,
                request.PublicationDate, request.Pages);

            var author = new Author(request.AuthorId);

            book.SetAuthor(author);

            var result = _bookRepository.Create(book);

            return Task.FromResult(result);
        }
    }
}
