using BookManagement.Domain.BookAggregate;
using BookManagement.Infrastructure.EntityTypeConfigurations;
using Core.Domain;
using Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure
{
    public class BookManagementContext 
        : CoreDbContext, IUnitOfWork
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }

        /// <summary>
        /// Constructor for the BookManagementContext.
        /// </summary>
        /// <param name="options"></param>
        public BookManagementContext(DbContextOptions<BookManagementContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new BookEntityTypeConfiguration())
                .ApplyConfiguration(new AuthorEntityTypeConfiguration());
        }
    }
}
