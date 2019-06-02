using Microsoft.AspNetCore.Blazor.Hosting;
using System;
using System.Reflection;

namespace Micro.Services.Web
{
    public class Program
    {
        public static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly string EnvName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}
