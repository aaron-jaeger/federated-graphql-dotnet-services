using BookManagement.Domain.BookAggregate;
using BookManagement.Infrastructure.EntityTypeConfigurations;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure
{
    public class BookManagementContext 
        : DbContext, IUnitOfWork
    {
        public DbSet<Book> Books { get; set; }

        private IDbContextTransaction _currentTransaction;

        /// <summary>
        /// Checks whether the context has a current transaction.
        /// </summary>
        /// /// <returns>true if the IDbContextTransaction is not null; otherwise, false</returns>
        public bool HasActiveTransaction => _currentTransaction != null;

        /// <summary>
        /// Constructor for the ServiceRequestManagementContext.
        /// </summary>
        /// <param name="options"></param>
        public BookManagementContext(DbContextOptions<BookManagementContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new BookEntityTypeConfiguration())
                .ApplyConfiguration(new AuthorEntityTypeConfiguration());
        }

        /// <summary>
        /// Begins the Db transaction.
        /// </summary>
        /// <returns>IDbContext transaction if the current transaction is null; otherwise returns null.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return null;

            _currentTransaction = await Database.BeginTransactionAsync();

            return _currentTransaction;
        }

        /// <summary>
        /// Rolls back the Db transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// Commits the Db transaction.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>Task</returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
