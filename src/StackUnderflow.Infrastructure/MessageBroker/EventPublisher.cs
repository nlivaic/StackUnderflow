using StackUnderflow.Common.Interfaces;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public class EventPublisher : IEventPublisher
    {
        public Task PublishVoteCastEvent<T>(T eventToPublish) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
