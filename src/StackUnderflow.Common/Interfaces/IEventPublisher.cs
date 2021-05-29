using System.Threading.Tasks;

namespace StackUnderflow.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishVoteCastEvent<T>(T eventToPublish) where T : class;
    }
}
