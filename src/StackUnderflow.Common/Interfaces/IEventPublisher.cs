using System.Threading.Tasks;

namespace StackUnderflow.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishEvent<T>(object eventToPublish)
            where T : class;
    }
}
