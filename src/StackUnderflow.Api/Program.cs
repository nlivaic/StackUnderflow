using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace StackUnderflow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            Log.Logger = new LoggerConfiguration()

                // Adding Entrypoint here means it is added to every log,
                // regardless if it comes from Hosting or the application itself.
                .Enrich.WithProperty("Entrypoint", Assembly.GetExecutingAssembly().GetName().Name)
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.Seq(configuration["Logs:Url"])
                .CreateLogger();

            try
            {
                Log.Information("Starting up StackUnderflow.");
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application Stack Underflow failed at startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
