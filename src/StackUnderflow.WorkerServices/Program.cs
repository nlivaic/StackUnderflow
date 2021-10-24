using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackUnderflow.WorkerServices;
using StackUnderflow.WorkerServices.PointServices;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Events;
using StackUnderflow.Data;
using StackUnderflow.Infrastructure.Caching;
using StackUnderflow.WorkerServices.FaultService;
using StackUnderflow.WorkerServices.PointService;
using System.Reflection;
using StackUnderflow.Application;

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
                    services.AddScoped<IPointService, PointServices.PointService>();
                    services.AddGenericRepository();
                    services.AddSpecificRepositories();
                    services.AddCoreServices();
                    services.AddSingleton<ICache, Cache>();
                    services.AddMemoryCache();
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(Comment).Assembly);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<VoteCastConsumer>();
                        x.AddConsumer<VoteCastFaultConsumer>();
                        x.AddConsumer<FaultConsumer>();
                        x.UsingAzureServiceBus((ctx, cfg) =>
                        {
                            cfg.Host(configuration["CONNECTIONSTRINGS:MESSAGEBROKER:READ"]);
                            // Use the below line if you are not going with
                            // SetKebabCaseEndpointNameFormatter() in the publishing project (see API project),
                            // but have rather given the topic a custom name.
                            //cfg.Message<VoteCast>(configTopology => configTopology.SetEntityName("vote-cast-topic"));
                            cfg.SubscriptionEndpoint<IVoteCast>("vote-cast-consumer", e =>
                            {
                                e.ConfigureConsumer<VoteCastConsumer>(ctx);
                            });
                            // This is here only for show. I have not thought through a proper 
                            // error handling strategy.
                            cfg.SubscriptionEndpoint<Fault<IVoteCast>>("vote-cast-fault-consumer", e =>
                            {
                                e.ConfigureConsumer<VoteCastFaultConsumer>(ctx);
                            });
                            // This is here only for show. I have not thought through a proper 
                            // error handling strategy.
                            cfg.SubscriptionEndpoint<Fault>("fault-consumer", e =>
                            {
                                e.ConfigureConsumer<FaultConsumer>(ctx);
                            });
                            cfg.ConfigureEndpoints(ctx);
                        });
                    });
                    services.AddMassTransitHostedService();
                    services.AddStackUnderflowApplicationHandlers();
                });
    }
}
