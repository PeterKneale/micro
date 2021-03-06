using AutoMapper;
using MediatR;
using Micro.Services.Tenants.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Services.Tenants
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
            services.AddAuth(_configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDatabase(_configuration.GetSqlConnectionString());
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddCustomHealthChecks(_configuration.GetSqlConnectionString());
            services.AddCustomSwagger();
            services.AddServices();
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
            app.CreateDatabase();
            app.MigrateDatabase();
        }
    }
}
