using AutoMapper;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using MediatR;
using Micro.Services.Gateway.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddCustomGraphQL();
            services.AddCustomHttpClient();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCustomHealthChecks();
            app.UseCustomMetaEndpoints();
            app.UseAuthentication();
            app.UseGraphQL<ISchema>("/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/playground"
            });
            app.UseMvc();
        }
    }
}
