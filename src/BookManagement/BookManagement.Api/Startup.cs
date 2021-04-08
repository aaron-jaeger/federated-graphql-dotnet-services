using BookManagement.Api.Schema;
using BookManagement.Domain.BookAggregate;
using BookManagement.Infrastructure;
using BookManagement.Infrastructure.Repositories;
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
using System;

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

            services.AddControllers();

            services.AddDbContext<BookManagementContext>(options => 
            {
                options.UseSqlServer(Configuration.GetConnectionString("BookManagement"));
            });

            services.AddScoped<IBookRepository, BookRepository>();

            services.AddMediatR(typeof(Application.Commands.CreateBookCommandHandler).Assembly);

            services
                .AddSingleton<AnyScalarGraphType>()
                .AddSingleton<ServiceGraphType>()
                .AddTransient<BookType>()
                .AddTransient<AuthorType>()
                .AddTransient<BookInput>()
                .AddTransient<BookQuery>()
                .AddTransient<BookMutation>()
                .AddTransient<ISchema>(s =>
                {
                    var books = s.GetRequiredService<IBookRepository>();

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
                            books: [Book!]
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
                            .ResolveReferenceAsync(context =>
                            {
                                var id = Guid.Parse((string)context.Arguments["id"]);
                                return books.RetrieveByIdAsync(id);
                            });
                    });
                })
                .AddGraphQL()
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
}
