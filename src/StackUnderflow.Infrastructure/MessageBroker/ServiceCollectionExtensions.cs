using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiEventPublisher(this IServiceCollection services, string connectionString)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingAzureServiceBus((ctx, cfg) =>
                {
                    cfg.Host(connectionString);

                    // Use the below line if you are not going with SetKebabCaseEndpointNameFormatter() above.
                    // Remember to configure the subscription endpoint accordingly (see WorkerServices Program.cs).
                    // cfg.Message<VoteCast>(configTopology => configTopology.SetEntityName("vote-cast-topic"));
                });
            });
            services.AddMassTransitHostedService();
            services.AddScoped<EventPublisher>();
            services.AddScoped<IEventRegister>(x => x.GetRequiredService<EventPublisher>());
            services.AddScoped<IRegisteredEventPublisher>(x => x.GetRequiredService<EventPublisher>());
        }
    }
}
