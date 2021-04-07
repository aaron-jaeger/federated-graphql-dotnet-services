using BookManagement.Api.Schema;
using BookManagement.Api.Services;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services
                .AddSingleton<AnyScalarGraphType>()
                .AddSingleton<ServiceGraphType>()
                .AddTransient<BookQuery>()
                .AddSingleton<IBookService, BookService>()
                .AddTransient<ISchema>(s =>
                {
                    var books = s.GetRequiredService<IBookService>();

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

                        extend type Query {
                            books: [Book!]
                            book(id: ID!): Book
                        }

                        extend type Author @key(fields: ""id"") {
                            id: ID! @external
                        }
                    ", _ =>
                    {
                        _.ServiceProvider = s;
                        _.Types
                            .Include<BookQuery>();
                        _.Types
                            .For("Book")
                            .ResolveReferenceAsync(context =>
                            {
                                var id = Guid.Parse((string)context.Arguments["id"]);
                                return books.GetBookByIdAsync(id);
                            });
                    });
                })
                .AddGraphQL()
                .AddSystemTextJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
