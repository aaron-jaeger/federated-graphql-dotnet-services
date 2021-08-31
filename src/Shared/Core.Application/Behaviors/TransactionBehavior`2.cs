using Core.Application.Extensions;
using Core.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Behaviors
{
    /// <summary>
    /// Creates a transaction that spans the life of the request.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class TransactionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly CoreDbContext _dbContext;

        /// <summary>
        /// Constructor for the TransactionBehavior class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        public TransactionBehavior(ILogger logger, CoreDbContext dbContext)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));

            _dbContext = (dbContext 
                ?? throw new ArgumentNullException(nameof(dbContext)));
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (_dbContext.HasActiveTransaction)
                    return await next();

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.CommitTransactionAsync(transaction);
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}
