using AuthorManagement.Domain.AuthorAggregate;
using AuthorManagement.Infrastructure.EntityTypeConfigurations;
using Core.Domain;
using Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AuthorManagement.Infrastructure
{
    public class AuthorManagementContext
        : CoreDbContext, IUnitOfWork
    {
        public DbSet<Author> Author { get; set; }

        /// <summary>
        /// Constructor for the AuthorManagementContext.
        /// </summary>
        /// <param name="options"></param>
        public AuthorManagementContext(DbContextOptions<AuthorManagementContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new AuthorEntityTypeConfiguration());
        }
    }
}
