using AuthorManagement.Api.Models;
using AuthorManagement.Api.Schemas;
using AuthorManagement.Api.Services;
using GraphQL;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace AuthorManagement.Api
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

            // GraphQL types
            services
                .AddSingleton<AnyScalarGraphType>()
                .AddSingleton<ServiceGraphType>()
                .AddSingleton<IAuthorService, AuthorService>()
                .AddTransient<AuthorQuery>()
                .AddTransient<AuthorInput>()
                .AddTransient<AuthorMutation>()
                .AddTransient<ISchema>(s =>
                {
                    var authors = s.GetRequiredService<IAuthorService>();

                    return FederatedSchema.For(@"
                        input AuthorInput {
                            firstName: String
                            lastName: String
                        }

                        extend type Query {
                            authors: [Author!]
                            author(id: ID!): Author
                        }

                        extend type Mutation {
                            createAuthor(input: AuthorInput): Author
                        }

                        type Author @key(fields: ""id"") {
                            id: ID!
                            firstName: String
                            lastName: String
                            books: [Book]
                        }

                        extend type Book @key(fields: ""id"") {
                            id: ID! @external
                        }
                    ", _ =>
                    {
                        _.ServiceProvider = s;
                        _.Types
                            .Include<AuthorInput>();
                        _.Types
                            .Include<AuthorQuery>();
                        _.Types
                            .Include<AuthorMutation>();
                        _.Types
                            .For("Author")
                            .ResolveReferenceAsync(context =>
                            {
                                var id = Guid.Parse((string)context.Arguments["id"]);
                                var author = authors.GetAuthorById(id);
                                return Task.FromResult(author);
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
