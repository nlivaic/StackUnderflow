using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Events;
using StackUnderflow.Data;
using StackUnderflow.Infrastructure.Caching;
using StackUnderflow.WorkerServices.PointService;
using System.Reflection;

namespace StackUnderflow.WorkerServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var hostEnvironment = hostContext.HostingEnvironment;
                    services.AddDbContext<StackUnderflowDbContext>(options =>
                    {
                        options.UseNpgsql(configuration.GetConnectionString("StackUnderflowDbConnection"));
                        if (hostEnvironment.IsDevelopment())
                            options.EnableSensitiveDataLogging(true);
                    });
                    services.AddGenericRepository();
                    services.AddSpecificRepositories();
                    services.AddCoreServices();
                    services.AddSingleton<ICache, Cache>();
                    services.AddMemoryCache();
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(Comment).Assembly);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<VoteConsumer>().Endpoint(cfg =>
                        {
                            cfg.Name = "point-service";
                        });
                        x.UsingAzureServiceBus((ctx, cfg) =>
                        {
                            cfg.Host(configuration["CONNECTIONSTRINGS:MESSAGEBROKER:READ"]);
                            cfg.Message<VoteCast>(c => c.SetEntityName("vote-cast"));
                            cfg.ConfigureEndpoints(ctx);
                        });
                    });
                    services.AddMassTransitHostedService();
                });
    }
}
