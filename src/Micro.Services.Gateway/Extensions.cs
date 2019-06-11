using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types;
using HealthChecks.UI.Client;
using Micro.Services.Gateway.GraphQL;
using Micro.Services.Gateway.GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Micro.Services.Gateway
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomGraphQL(this IServiceCollection services)
        {
            services
                .AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService))
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IDocumentExecuter, DocumentExecuter>()
                .AddSingleton<IDocumentWriter, DocumentWriter>()
                .AddSingleton<CustomQuery>()
                .AddSingleton<CustomMutation>()
                .AddSingleton<UserType>()
                .AddSingleton<UserInputType>()
                .AddSingleton<TeamType>()
                .AddSingleton<TeamInputType>()
                .AddSingleton<RoleType>()
                .AddSingleton<RoleInputType>()
                .AddSingleton<ISchema, CustomSchema>();
            services
                .AddGraphQL(_ =>
                {
                    _.EnableMetrics = true;
                    _.ExposeExceptions = true;
                })
                .AddUserContextBuilder(httpContext => new CustomUserContext
                {
                    User = httpContext.User,
                    Headers = httpContext.Request.Headers
                });
            return services;
        }

        public static IServiceCollection AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ITenantsApi, TenantsApi>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetTenantsUrl());
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", $"{Program.AppName}{Program.AppVersion}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Tenant1Jwt);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Tenant2Jwt);
            });
            return services;
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecksUI();
            services.AddHealthChecks()
                .AddCheck("api", () => HealthCheckResult.Healthy());
            return services;
        }

        public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
        {
            return app
                           .UseHealthChecksUI()
                           .UseHealthChecks("/health/alive", new HealthCheckOptions
                           {
                               Predicate = r => true,
                               ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                           })
                           .UseHealthChecks("/health/ready", new HealthCheckOptions
                           {
                               Predicate = r => true,
                               ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                           });
        }

        public static IApplicationBuilder UseCustomMetaEndpoints(this IApplicationBuilder app)
        {
            return app
                           .Map("/app/name", appBuilder =>
                           {
                               appBuilder.Run(async context =>
                               {
                                   await context.Response.WriteAsync(Program.AppName);
                               });
                           })
                           .Map("/app/version", appBuilder =>
                           {
                               appBuilder.Run(async context =>
                               {
                                   await context.Response.WriteAsync(Program.AppVersion);
                               });
                           })
                           .Map("/app/config", appBuilder =>
                           {
                               appBuilder.Run(async context =>
                               {
                                   var d = new Dictionary<string, string>();
                                   var configuration = appBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
                                   foreach (var entry in configuration.AsEnumerable().OrderBy(x => x.Key))
                                   {
                                       d.Add(entry.Key, entry.Value);
                                   }
                                   var json = JsonConvert.SerializeObject(d);
                                   await context.Response.WriteAsync(json);
                               });
                           })
                           .Map("/errors/internal", appBuilder =>
                           {
                               appBuilder.Run(context =>
                               {
                                   throw new Exception("ERROR!");
                               });
                           });
        }

        public static string GetSeqUrl(this IConfiguration configuration)
        {
            return configuration["SeqUrl"];
        }

        public static string GetTenantsUrl(this IConfiguration configuration)
        {
            return configuration["TENANTS_URL"];
        }

        public static string GetContentUrl(this IConfiguration configuration)
        {
            return configuration["CONTENT_URL"];
        }

        public static readonly string Tenant1Jwt = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFkNjNkMGFkNTkwNDNiZWMwMTA2MzBmZWI5OTdmNjc1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NjAyNTUyMTEsImV4cCI6MTU2MDI1ODgxMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDA0IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwNC9yZXNvdXJjZXMiLCJhcGkiXSwiY2xpZW50X2lkIjoicG9zdG1hbiIsInN1YiI6IjMiLCJhdXRoX3RpbWUiOjE1NjAyNTUyMTEsImlkcCI6ImxvY2FsIiwidXNlcl9pZCI6IjMiLCJ0ZW5hbnRfaWQiOiIyIiwicGVybWlzc2lvbiI6WyJ0ZWFtLmNyZWF0ZSIsInRlYW0uZGVsZXRlIiwidGVhbS5lZGl0IiwidGVhbS52aWV3IiwidXNlci5jcmVhdGUiLCJ1c2VyLmRlbGV0ZSIsInVzZXIuZWRpdCIsInVzZXIudmlldyJdLCJzY29wZSI6WyJhcGkiXSwiYW1yIjpbInB3ZCJdfQ.TM_JkdIIxTlGAcTZnffrJBKKMU9PLQewYPpLAS0IcDR2SE8zCy4cCU_7oIy93mfxFtkwkcLMLdJxWBHdrUq8mOMGWzaGnrgZGfbZKIIqArof-TCnPhQD73jewGbhcLdz1Ny3qD8hasp8MPcm11p8I6tk04OXI9C894iuaP8wItNoKk_3_TSU3UGPUzW_POI-fGjPyM1QDDIF5W_Vi9A44kAiWL4BnOAQke0sji4mgf-fXfOxhlzh5hxhjwFMpdfaW3WBf5eR5CRrhH198NqCdot1gia89YesFa0i_g7mpRVJp2g3n_CaT7dJhZ9MUabf1PGibvNR3unOlMcch98qYw";
        public static readonly string Tenant2Jwt = "";
    }
}
