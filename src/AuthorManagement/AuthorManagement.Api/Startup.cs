using AuthorManagement.Api.Application.Behaviors;
using AuthorManagement.Api.Schemas;
using AuthorManagement.Domain.AuthorAggregate;
using AuthorManagement.Infrastructure;
using AuthorManagement.Infrastructure.Repositories;
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
            services
                .AddControllerServices()
                .AddDbServices(Configuration)
                .AddMediatRServices()
                .AddGraphQLServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthorManagementContext context)
        {
            context.Database.Migrate();

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
            services.AddDbContext<AuthorManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AuthorManagement"));
            });

            services.AddScoped<IAuthorRepository, AuthorRepository>();

            return services;
        }

        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {

            services.AddMediatR(typeof(Application.Commands.CreateAuthorCommandHandler).Assembly);

            // Register pipeline behaviors in the order you want them to run in the pipeline.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorManagementContextTransactionBehavior<,>));

            return services;
        }

        public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
        {
            services
                .AddSingleton<AnyScalarGraphType>()
                .AddSingleton<ServiceGraphType>()
                .AddSingleton<Schemas.Author>()
                .AddSingleton<BookType>()
                .AddSingleton<AuthorInput>()
                .AddSingleton<AuthorQuery>()
                .AddSingleton<AuthorMutation>()
                .AddSingleton(s => FederatedSchemaFactory.BuildFederatedSchema(s))
                .AddGraphQL()
                .AddSystemTextJson();

            return services;
        }
    }
}
