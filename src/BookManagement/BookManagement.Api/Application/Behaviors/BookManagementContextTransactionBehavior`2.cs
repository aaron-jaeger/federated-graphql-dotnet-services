using BookManagement.Infrastructure;
using Core.Application.Behaviors;
using Microsoft.Extensions.Logging;

namespace BookManagement.Api.Application.Behaviors
{
    public class BookManagementContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<TRequest, TResponse>
    {
        /// <summary>
        /// Constructor for the TransactionBehavior class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        public BookManagementContextTransactionBehavior(ILogger<BookManagementContextTransactionBehavior<TRequest, TResponse>> logger, BookManagementContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
