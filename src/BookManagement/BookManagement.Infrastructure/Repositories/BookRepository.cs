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
            return _context.Books
                .Add(book)
                .Entity;
        }

        public async Task<IEnumerable<Book>> RetrieveAllAsync()
        {
            return await _context.Books
                .ToListAsync();
        }

        public async Task<Book> RetrieveByIdAsync(Guid id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(book => book.Id == id);
        }
    }
}
