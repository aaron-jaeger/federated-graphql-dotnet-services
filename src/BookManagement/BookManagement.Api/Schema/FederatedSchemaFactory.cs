using BookManagement.Domain.BookAggregate;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BookManagement.Api.Schema
{
    public static class FederatedSchemaFactory
    {
        private static string _schemaPath = @"./schema.graphql";

        public static ISchema BuildFederatedSchema(IServiceProvider serviceProvider)
        {
            var schema = File.ReadAllText(_schemaPath);

            return FederatedSchema.For(schema, _ =>
            {
                _.ServiceProvider = serviceProvider;
                _.Types
                    .Include<BookType>();
                _.Types
                    .Include<AuthorType>();
                _.Types
                    .Include<BookInput>();
                _.Types
                    .Include<BookQuery>();
                _.Types
                    .Include<BookMutation>();
                _.Types
                    .For(nameof(Book))
                    .ResolveReferenceAsync(context =>
                    {
                        using var scope = serviceProvider.CreateScope();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                        var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();

                        var id = Guid.Parse((string)context.Arguments[nameof(Book.Id)]);

                        logger.LogDebug("Resolving reference for {AggregateRootName} by id {Id}", nameof(Book), id);
                        return bookRepository.RetrieveBookByBookIdAsync(id);
                    });
            });
        }
    }
}
