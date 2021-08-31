using AuthorManagement.Api.Extensions;
using AuthorManagement.Domain.AuthorAggregate;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AuthorManagement.Api.Schemas
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
                    .Include<Author>();
                _.Types
                    .Include<BookType>();
                _.Types
                    .Include<AuthorInput>();
                _.Types
                    .Include<AuthorQuery>();
                _.Types
                    .Include<AuthorMutation>();
                _.Types
                    .For(nameof(Author))
                    .ResolveReferenceAsync(async context =>
                    {
                        using var scope = serviceProvider.CreateScope();
                        //var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                        var authorRepository = scope.ServiceProvider.GetRequiredService<IAuthorRepository>();

                        //logger.LogDebug("Parsing id for aggregate root");

                        var id = Guid.Parse((string)context.Arguments["id"]);

                        //logger.LogDebug("Resolving reference for {AggregateRootName} by id {Id}", nameof(Author), id);
                        var author = await authorRepository.RetrieveAuthorByAuthorIdAsync(id);

                        var result = author.AsAuthorType();

                        return result;
                    });
            });
        }
    }
}
