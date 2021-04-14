using BookManagement.Api.Application.Behaviors;
using BookManagement.Api.Extensions;
using BookManagement.Api.Schema;
using BookManagement.Domain.BookAggregate;
using BookManagement.Infrastructure;
using BookManagement.Infrastructure.Repositories;
using Core.Application.Behaviors;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllerServices()
                .AddDbServices(Configuration)
                .AddMediatRServices()
                .AddSingleton<AnyScalarGraphType>()
                .AddSingleton<ServiceGraphType>()
                .AddSingleton<BookType>()
                .AddSingleton<AuthorType>()
                .AddSingleton<BookInput>()
                .AddSingleton<BookQuery>()
                .AddSingleton<BookMutation>()
                .AddSingleton<ISchema>(s =>
                {
                    return FederatedSchema.For(@"
                        type Book @key(fields: ""id"") {
                            id: ID!
                            title: String
                            overview: String
                            author: Author
                            isbn: Long
                            publisher: String
                            publicationDate: Date
                            pages: Int
                        }

                        extend type Author @key(fields: ""id"") {
                            id: ID! @external
                        }

                        input BookInput {
                            title: String!
                            overview: String!
                            authorId: String!
                            isbn: Long!
                            publisher: String!
                            publicationDate: Date!
                            pages: Int!
                        }

                        extend type Query {
                            books:[Book!]
                            book(id: ID!): Book
                        }

                        extend type Mutation {
                            createBook(bookInput: BookInput): Book
                        }
                    ", _ =>
                    {
                        _.ServiceProvider = s;
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
                            .For("Book")
                            .ResolveReferenceAsync(async context =>
                            {
                                using var scope = s.CreateScope();
                                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                                var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();
                                try
                                {
                                    logger.LogDebug("Parsing id for aggregate root");

                                    var id = Guid.Parse((string)context.Arguments["id"]);

                                    logger.LogDebug("Resolving reference for {AggregateRootName} by id {Id}", nameof(Book), id);
                                    //var newGuid = Guid.NewGuid();
                                    var book = await bookRepository.RetrieveBookByBookIdAsync(id);

                                    logger.LogDebug("({@AggregateRoot})", book);
                                    logger.LogDebug("Book: Id {Id}, Title {Title}, Overview {Overview}, AuthorId {AuthorId}, Isbn {ISBN}, Publiser {Publisher}, PublicationDate {PublicationDate}, Pages {Pages}", book.Id, book.Title, book.Overview, book.Author.Id, book.Isbn, book.Publisher, book.PublicationDate, book.Pages);
                                    //logger.LogDebug("Extending {AggregateRoot} as {GraphType}", nameof(Book), nameof(BookType));
                                    //var result = book.AsBookType();

                                    //logger.LogDebug("BookType: Id {Id}, Title {Title}, Overview {Overview}, AuthorId {AuthorId}, Isbn {ISBN}, Publiser {Publisher}, PublicationDate {PublicationDate}, Pages {Pages}", result.Id, result.Title, result.Overview, result.Author.Id, result.Isbn, result.Publisher, result.PublicationDate, result.Pages);

                                    logger.LogDebug("Returning result ({@Result})", book);
                                    //var taskResult = Task.FromResult(result);
                                    return book;
                                }
                                catch(Exception ex)
                                {
                                    logger.LogError(ex, "Something messed up?");
                                    throw;
                                }
                            });
                    });
                })
                .AddGraphQL(options => 
                {
                    options.EnableMetrics = true;
                })
                .AddSystemTextJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BookManagementContext context)
        {
            context.Database
                .Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL<ISchema>();

            app.UseGraphQLPlayground();
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection AddControllerServices(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BookManagement"));
            });

            services.AddScoped<IBookRepository, BookRepository>();

            return services;
        }

        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {

            services.AddMediatR(typeof(Application.Commands.CreateBookCommandHandler).Assembly);
            
            // Register pipeline behaviors in the order you want them to run in the pipeline.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(BookManagementContextTransactionBehavior<,>));

            return services;
        }

        public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
        {
            services
                .AddSingleton<AnyScalarGraphType>()
                .AddSingleton<ServiceGraphType>()
                .AddSingleton<BookType>()
                .AddSingleton<AuthorType>()
                .AddSingleton<BookInput>()
                .AddSingleton<BookQuery>()
                .AddSingleton<BookMutation>()
                .AddTransient<ISchema>(s =>
                {
                    //var books = s.GetRequiredService<IBookService>();

                    return FederatedSchema.For(@"
                        type Book @key(fields: ""id"") {
                            id: ID!
                            title: String
                            overview: String
                            author: Author
                            isbn: Long
                            publisher: String
                            publicationDate: Date
                            pages: Int
                        }

                        extend type Author @key(fields: ""id"") {
                                    id: ID! @external
                        }

                        input BookInput {
                            title: String!
                            overview: String!
                            authorId: String!
                            isbn: Long!
                            publisher: String!
                            publicationDate: Date!
                            pages: Int!
                        }

                        extend type Query {
                            books:[Book!]
                            book(id: ID!): Book
                        }

                        extend type Mutation {
                            createBook(bookInput: BookInput): Book
                        }
                    ", _ =>
                    {
                        _.ServiceProvider = s;
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
                            .For("Book")
                            .ResolveReferenceAsync(async context =>
                            {
                                using var scope = s.CreateScope();
                                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                                var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();

                                logger.LogDebug("Parsing id for aggregate root");

                                var id = Guid.Parse((string)context.Arguments["id"]);

                                logger.LogDebug("Resolving reference for {AggregateRootName} by id {Id}", nameof(Book), id);
                                var book = await bookRepository.RetrieveBookByBookIdAsync(id);

                                var result = book.AsBookType();

                                return result;
                            });
                    });
                })
                .AddGraphQL()
                .AddSystemTextJson();

            return services;
        }
    }
}
