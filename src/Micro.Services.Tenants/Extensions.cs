using System.Data.SqlClient;
using Micro.Services.Tenants.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Services.Tenants
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connection) => 
            services.AddDbContext<DatabaseContext>(ctx => ctx.UseSqlServer(connection, opt => opt.EnableRetryOnFailure()));

        public static void CreateDatabase(this IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.EnsureCreated();
            }
        }
        
        public static string GetSqlConnectionString(this IConfiguration configuration) =>
            configuration["CONNECTION_STRING"];

        public static string GetSqlDatabaseName(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).InitialCatalog;

        public static string GetSqlServerName(this IConfiguration configuration) =>
            new SqlConnectionStringBuilder(configuration.GetSqlConnectionString()).DataSource;

        public static string GetSeqUrl(this IConfiguration configuration) => 
            configuration["SEQ_URL"];
    }
}
