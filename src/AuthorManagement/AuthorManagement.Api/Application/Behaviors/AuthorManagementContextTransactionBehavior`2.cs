using AuthorManagement.Infrastructure;
using Core.Application.Behaviors;
using Microsoft.Extensions.Logging;

namespace AuthorManagement.Api.Application.Behaviors
{
    public class AuthorManagementContextTransactionBehavior<TRequest, TResponse> 
        : TransactionBehavior<TRequest, TResponse>
    {
        /// <summary>
        /// Constructor for the TransactionBehavior class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        public AuthorManagementContextTransactionBehavior(ILogger<AuthorManagementContextTransactionBehavior<TRequest, TResponse>> logger,
            AuthorManagementContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
