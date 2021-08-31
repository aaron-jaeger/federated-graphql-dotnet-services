using AuthorManagement.Domain.AuthorAggregate;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorManagement.Infrastructure.Repositories
{
    public class AuthorRepository
        : IAuthorRepository
    {
        private readonly AuthorManagementContext _context;
        public IUnitOfWork UnitOfWork { get { return _context; } }

        public AuthorRepository(AuthorManagementContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public Author Create(Author author)
        {
            return _context.Author
                .Add(author)
                .Entity;
        }

        public async Task<IEnumerable<Author>> RetrieveAllAuthorsAsync()
        {
            return await _context.Author
                .Include(author => author.Books)
                .ToListAsync();
        }

        public async Task<Author> RetrieveAuthorByAuthorIdAsync(Guid id)
        {
            return await _context.Author
                .Include(author => author.Books)
                .FirstOrDefaultAsync(author => author.Id == id);
        }
    }
}
