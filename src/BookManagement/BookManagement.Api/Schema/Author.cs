using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BookManagement.Api.Schema
{
    [GraphQLMetadata("Author")]
    public class Author
    {
        private readonly IServiceProvider _serviceProvider;

        public Author()
        {
        }
        public Author(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Guid Id { get; set; }
        public bool HasBooks(IResolveFieldContext context, Author author)
        {

            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Author>>();

            logger.LogInformation("Getting the ID of the Author.");

            string id = (context.Source as Dictionary<string, object>)?["id"]?.ToString();

            logger.LogInformation($"The Id of the Author is {id}");

            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return true;
        }
    }
}
