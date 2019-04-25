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
        public static readonly string EnvName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

        public static int Main(string[] args)
        {
            try
            {
                Console.Title = AppName;

                SetupLogger();

                Log.Information("Starting {AppName} v{AppVersion} in {EnvName}", AppName, AppVersion, EnvName);

                CreateWebHostBuilder(args).Build().Run();

                Log.Information("Stopped {AppName} v{AppVersion} in {EnvName}", AppName, AppVersion, EnvName);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{AppName} v{AppVersion} crashed in {EnvName}", AppName, AppVersion, EnvName);
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
                .Enrich.WithProperty("AppVersion", AppVersion)
                .Enrich.WithProperty("EnvName", EnvName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(configuration.GetSeqUrl())
                .CreateLogger();
        }

        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{EnvName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}