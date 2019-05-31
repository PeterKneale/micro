using AutoMapper;
using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using MediatR;
using Micro.Services.Gateway.Exceptions;
using Micro.Services.Gateway.GraphQL;
using Micro.Services.Gateway.GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Services.Gateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddCustomHealthChecks();
            services.AddCustomSwagger();

            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();


            services.AddSingleton<MicroData>();
            services.AddSingleton<MicroQuery>();
            services.AddSingleton<MicroMutation>();
            services.AddSingleton<MicroUserType>();
            services.AddSingleton<MicroUserInputType>();
            services.AddSingleton<MicroTeamType>();
            services.AddSingleton<MicroTeamInputType>();
            services.AddSingleton<ISchema, MicroSchema>();

            services.AddGraphQL(_ =>
            {
                _.EnableMetrics = true;
                _.ExposeExceptions = true;
            })
            .AddUserContextBuilder(httpContext => new UserContext { User = httpContext.User });

        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCustomHealthChecks();
            app.UseCustomSwagger();
            app.UseCustomMetaEndpoints();
            app.UseAuthentication();
            app.UseMvc();

            app.UseGraphQL<ISchema>("/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/query"
            });
        }
    }
}
