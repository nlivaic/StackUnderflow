using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackUnderflow.Core.Events;
using StackUnderflow.WorkerServices.PointService;

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
                    services.AddHostedService<PointWorker>();

                    services.AddMassTransit(x =>
                    {
                        x.UsingAzureServiceBus((ctx, cfg) =>
                        {
                            cfg.Host(configuration["CONNECTIONSTRINGS:MESSAGEBROKER:READ"]);
                            cfg.Message<VoteCast>(c => c.SetEntityName("vote-cast"));
                            cfg.SubscriptionEndpoint<VoteCast>("point-service", e => e.Consumer<VoteConsumer>());
                        });
                    });
                    services.AddMassTransitHostedService();
                });
    }
}
