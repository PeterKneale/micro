using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Micro.Services.Tenants
{
    public class Program
    {
        public static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;

        public static int Main(string[] args)
        {
            try
            {
                SetupLogger();

                Log.Information("Starting {AppName}", AppName);

                CreateWebHostBuilder(args).Build().Run();

                Log.Information("Stopped {AppName}", AppName);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{AppName} crashed", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .UseKestrel();

        private static void SetupLogger()
        {
            var configuration = GetConfiguration();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("AppName", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(configuration["SEQ_URL"])
                .CreateLogger();
        }

        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}