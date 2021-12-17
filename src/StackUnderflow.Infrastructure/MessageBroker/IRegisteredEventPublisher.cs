using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public interface IRegisteredEventPublisher
    {
        Task PublishAll();
    }
}
