using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using StackUnderflow.Application;
using StackUnderflow.Application.PointServices;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Events;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Data;
using StackUnderflow.Infrastructure.Caching;
using StackUnderflow.Infrastructure.MessageBroker;
using StackUnderflow.WorkerServices.FaultService;
using StackUnderflow.WorkerServices.PointService;
using StackUnderflow.WorkerServices.PointServices;

namespace StackUnderflow.WorkerServices
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var hostEnvironment = hostContext.HostingEnvironment;
                    services.AddDbContext<StackUnderflowDbContext>(options =>
                    {
                        var connString = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("StackUnderflowDbConnection"))
                        {
                            Username = configuration["POSTGRES_USER"],
                            Password = configuration["POSTGRES_PASSWORD"]
                        };
                        options.UseNpgsql(connString.ConnectionString);
                        if (hostEnvironment.IsDevelopment())
                        {
                            options.EnableSensitiveDataLogging(true);
                        }
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
                            cfg.Host(
                                new MessageBrokerConnectionStringBuilder(
                                    configuration.GetConnectionString("MessageBroker"),
                                    configuration["MessageBroker:Reader:SharedAccessKeyName"],
                                    configuration["MessageBroker:Reader:SharedAccessKey"]).ConnectionString );

                            // Use the below line if you are not going with
                            // SetKebabCaseEndpointNameFormatter() in the publishing project (see API project),
                            // but have rather given the topic a custom name.
                            // cfg.Message<VoteCast>(configTopology => configTopology.SetEntityName("vote-cast-topic"));
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
                    services.AddScoped<ILimitsService, LimitsService>();
                });
    }
}
