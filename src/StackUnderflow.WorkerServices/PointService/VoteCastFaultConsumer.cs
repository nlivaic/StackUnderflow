using MassTransit;
using StackUnderflow.Core.Events;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.PointService
{
    /// <summary>
    /// This is here only for show.
    /// I have not thought through a proper error handling strategy.
    /// Make VoteCastConsumer throw in order to kick error handling off.
    /// </summary>
    class VoteCastFaultConsumer : IConsumer<Fault<VoteCast>>
    {
        public Task Consume(ConsumeContext<Fault<VoteCast>> context)
        {
            return Task.CompletedTask;
        }
    }
}
