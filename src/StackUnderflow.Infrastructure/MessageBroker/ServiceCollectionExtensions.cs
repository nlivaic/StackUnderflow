using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Events;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiEventPublisher(this IServiceCollection services, string connectionString)
        {
            services.AddMassTransit(x =>
            {
                x.UsingAzureServiceBus((ctx, cfg) =>
                {
                    cfg.Host(connectionString);
                    cfg.Message<SomeEventHappened>(c => c.SetEntityName("some-event-happened"));
                    cfg.SubscriptionEndpoint<SomeEventHappened>("some-event-happened-service", e => e.Consumer<SomeEventHappenedConsumer>());
                });
            });
            services.AddMassTransitHostedService();
            services.AddTransient<IEventPublisher, EventPublisher>();
        }
    }

    public class SomeEventHappenedConsumer : IConsumer<SomeEventHappened>
    {
        public Task Consume(ConsumeContext<SomeEventHappened> context)
        {
            //throw new Exception("Bad things happened in consumer.");
            return Task.CompletedTask;
        }
    }
}
