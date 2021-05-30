using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Events;

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
                    cfg.Message<VoteCast>(c => c.SetEntityName("vote-cast"));
                });
            });
            services.AddMassTransitHostedService();
            services.AddTransient<IEventPublisher, EventPublisher>();
        }
    }
}
