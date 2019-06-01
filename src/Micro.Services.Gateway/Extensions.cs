using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
                .AddSingleton<TeamInputType>()
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
            => app
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

        public static IApplicationBuilder UseCustomMetaEndpoints(this IApplicationBuilder app)
            => app
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

        public static string GetSeqUrl(this IConfiguration configuration) =>
            configuration["SeqUrl"];

        public static string GetTenantsUrl(this IConfiguration configuration) =>
            configuration["TENANTS_URL"];

        public static string GetContentUrl(this IConfiguration configuration) =>
            configuration["CONTENT_URL"];

        public static readonly string Tenant1Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJ1c2VyX2lkIjoiMSIsInRlbmFudF9pZCI6IjEiLCJwZXJtaXNzaW9uIjpbInVzZXIuY3JlYXRlIiwidXNlci5kZWxldGUiLCJ1c2VyLmVkaXQiLCJ1c2VyLnZpZXciLCJ0ZWFtLmNyZWF0ZSIsInRlYW0uZGVsZXRlIiwidGVhbS5lZGl0IiwidGVhbS52aWV3Il0sIm5iZiI6MTU1OTMwNTUzMSwiZXhwIjoxNTU5OTEwMzMxLCJpYXQiOjE1NTkzMDU1MzF9.YWIbYag1P1rZScc4w49fp7EmeyoSjqZvZF1y_LQbU5k";
        public static readonly string Tenant2Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjMiLCJ1c2VyX2lkIjoiMyIsInRlbmFudF9pZCI6IjIiLCJwZXJtaXNzaW9uIjpbInVzZXIuY3JlYXRlIiwidXNlci5kZWxldGUiLCJ1c2VyLmVkaXQiLCJ1c2VyLnZpZXciLCJ0ZWFtLmNyZWF0ZSIsInRlYW0uZGVsZXRlIiwidGVhbS5lZGl0IiwidGVhbS52aWV3Il0sIm5iZiI6MTU1OTM1MzU2OCwiZXhwIjoxNTU5OTU4MzY4LCJpYXQiOjE1NTkzNTM1Njh9.xWc7TzHS6K1D4oeb5viO9uRn9HM3acztLbJmrqtQCJg";
    }
}
