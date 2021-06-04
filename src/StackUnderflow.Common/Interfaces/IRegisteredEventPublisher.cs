using System.Threading.Tasks;

namespace StackUnderflow.Common.Interfaces
{
    public interface IRegisteredEventPublisher
    {
        Task PublishAll();
    }
}
