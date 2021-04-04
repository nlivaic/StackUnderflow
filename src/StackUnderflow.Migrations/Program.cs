using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace StackUnderflow.Migrations
{
    class Program
    {
        static int Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Development";
            Console.WriteLine($"Environment: {env}.");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connectionStringStackUnderflow =
                args.FirstOrDefault()
                ?? config["ConnectionStrings:StackUnderflowDbConnection"];

            var upgraderStackUnderflow =
                DeployChanges.To
                    .PostgresqlDatabase(connectionStringStackUnderflow)
                    .WithScriptsFromFileSystem(Path.Combine(Environment.CurrentDirectory, "StackUnderflowScripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading Stack Underflow.");
            if (env != "Development")
            {
                Console.WriteLine($"Skipping 0005_InitialData.sql since we are not in Development environment.");
                upgraderStackUnderflow.MarkAsExecuted("0005_InitialData.sql");
            }
            var resultStackUnderflow = upgraderStackUnderflow.PerformUpgrade();

            if (!resultStackUnderflow.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Stack Underflow upgrade error: {resultStackUnderflow.Error}");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            var connectionStringStackUnderflowIdentity =
                args.Length == 2 ? args[1] : config["ConnectionStrings:StackUnderflowIdentityDb"];

            var upgraderStackUnderflowIdentity =
                DeployChanges.To
                    .PostgresqlDatabase(connectionStringStackUnderflowIdentity)
                    .WithScriptsFromFileSystem(Path.Combine(Environment.CurrentDirectory, "StackUnderflowIdentityScripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading Stack Underflow Identity.");
            if (env != "Development")
            {
                upgraderStackUnderflowIdentity.MarkAsExecuted("0004_InitialData.sql");
                Console.WriteLine($"Skipping 0004_InitialData.sql since we are not in Development environment.");
            }
            var resultStackUnderflowIdentity = upgraderStackUnderflowIdentity.PerformUpgrade();

            if (!resultStackUnderflowIdentity.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Stack Underflow Identity upgrade error: {resultStackUnderflowIdentity.Error}");
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
