using BookManagement.Domain.BookAggregate;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Repositories
{
    public class BookRepository 
        : IBookRepository
    {
        private readonly BookManagementContext _context;
        public IUnitOfWork UnitOfWork { get { return _context; } }

        public BookRepository(BookManagementContext context)
        {
            _context = context 
                ?? throw new ArgumentNullException(nameof(context));
        }

        public Book Create(Book book)
        {
            return _context.Book
                .Add(book)
                .Entity;
        }

        public async Task<IEnumerable<Book>> RetrieveAllBooksAsync()
        {
            return await _context.Book
                .Include(book => book.Author)
                .ToListAsync();
        }

        public async Task<Book> RetrieveBookByBookIdAsync(Guid id)
        {
            return await _context.Book
                .Include(book => book.Author)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task<Author> RetrieveAuthorByAuthorIdAsync(Guid id)
        {
            return await _context.Author
                .FirstOrDefaultAsync(author => author.Id == id);
        }
    }
}
