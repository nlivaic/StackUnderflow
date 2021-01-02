using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using StackUnderflow.Api.Seeders;

namespace StackUnderflow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), @"c:\temp\logs\stack-underflow.json", shared: true)
                .CreateLogger();

            try
            {
                Log.Information("Starting up StackUnderflow.");
                CreateHostBuilder(args)
                    .Build()
                    .Seed()
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
