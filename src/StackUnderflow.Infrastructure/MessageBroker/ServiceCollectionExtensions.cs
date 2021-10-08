using MassTransit;
using MassTransit.Topology;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Common.Interfaces;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiEventPublisher(this IServiceCollection services, string connectionString)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingAzureServiceBus((_, cfg) =>
                {
                    cfg.Host(connectionString);

                });
            });
            services.AddMassTransitHostedService();
            services.AddScoped<EventPublisher>();
            services.AddScoped<IEventPublisher>(x => x.GetRequiredService<EventPublisher>());
            services.AddScoped<IEventRegister>(x => x.GetRequiredService<EventPublisher>());
            services.AddScoped<IRegisteredEventPublisher>(x => x.GetRequiredService<EventPublisher>());
        }
    }
}
