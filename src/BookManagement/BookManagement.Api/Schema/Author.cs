using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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
        public bool HasBooks(IResolveFieldContext context, Author source)
        {

            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Author>>();

            logger.LogInformation("doing author things");

            //foreach(var argument in context.Arguments)
            //{
            //    logger.LogInformation(argument.Key);
            //}

            //var test = context.HasArgument("id");

            logger.LogInformation($"The id of the author is {source.Id}");
            
            //var fuckYouId = Guid.Parse((string)context.Arguments["id"].Value);
            //Console.WriteLine(fuckYouId);
            //return fuckYouId == Guid.Empty;
            return source.Id != Guid.Empty;
        }
    }
}
