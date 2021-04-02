using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Compumed.Migrations
{
    class Program
    {
        static int Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Development";
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connectionString =
                args.FirstOrDefault()
                ?? config["ConnectionStrings:CompumedDB"];

            string scriptsPath = null;
            if (args.Length == 2)
            {
                scriptsPath = args[1];
            }

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsFromFileSystem(scriptsPath != null ? Path.Combine(scriptsPath, "Scripts") : Path.Combine(Environment.CurrentDirectory, "Scripts"))
                    .LogToConsole()
                    .Build();
            // Comment the following two lines out
            // before the first run ONLY in case you want
            // to create the database from scratch.
            upgrader.MarkAsExecuted("0001_schema.sql");
            upgrader.MarkAsExecuted("0002_data.sql");

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
