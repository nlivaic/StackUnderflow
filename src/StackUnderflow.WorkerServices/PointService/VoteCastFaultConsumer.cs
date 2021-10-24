using System.Threading.Tasks;
using MassTransit;
using StackUnderflow.Core.Events;

namespace StackUnderflow.WorkerServices.PointService
{
    /// <summary>
    /// This is here only for show.
    /// I have not thought through a proper error handling strategy.
    /// Make VoteCastConsumer throw in order to kick error handling off.
    /// </summary>
    public class VoteCastFaultConsumer : IConsumer<Fault<IVoteCast>>
    {
        public Task Consume(ConsumeContext<Fault<IVoteCast>> context) =>
            Task.CompletedTask;
    }
}
