using System;
using System.IO;
using System.Reflection;
using Micro.Library;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Micro.ServiceA
{
    public class Program
    {
        public static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;

        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            var connection = configuration["CONNECTION_STRING"];
            var seq = configuration["SEQ_URL"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("AppName", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seq)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting {AppName}", AppName);
                WebHost.CreateDefaultBuilder()
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .UseDatabase(connection)
                    .Build()
                    .Run();
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
    }
}