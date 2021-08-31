using AuthorManagement.Api.Application.Commands;
using AuthorManagement.Api.Extensions;
using AuthorManagement.Domain.AuthorAggregate;
using GraphQL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Mutation")]
    public class AuthorMutation
    {
        private readonly IServiceProvider _serviceProvider;
        public AuthorMutation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [GraphQLMetadata("createAuthor")]
        public async Task<Author> CreateAuthorAsync(AuthorInput authorInput)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AuthorMutation>>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            logger.LogInformation("Begin processing mutation with input ({@Input}).", authorInput);

            var createAuthorCommand = new CreateAuthorCommand(authorInput.FirstName, authorInput.MiddleName, authorInput.LastName,
                authorInput.BookIds);

            logger.LogDebug("Sending command {CommandName} ({@Command})", nameof(CreateAuthorCommand), createAuthorCommand);
            var author = await mediator.Send(createAuthorCommand);

            logger.LogDebug("Converting entity ({@AggregateRoot}) to response type {ResponseType}.", author, nameof(Author));
            var result = author.AsAuthorType();

            logger.LogDebug("Returning mutation result ({@Result})", result);
            return result;
        }
    }
}
