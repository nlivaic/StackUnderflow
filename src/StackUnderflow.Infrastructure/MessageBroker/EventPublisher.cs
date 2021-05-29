using MassTransit;
using StackUnderflow.Common.Interfaces;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishEvent<T>(object eventToPublish) where T : class
        {
            await _publishEndpoint.Publish<T>(eventToPublish);
        }
    }
}
