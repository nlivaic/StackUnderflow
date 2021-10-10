using MassTransit;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.FaultService
{
    /// <summary>
    /// This is here only for show.
    /// I have not thought through a proper error handling strategy.
    /// Make VoteCastConsumer throw in order to kick error handling off.
    /// </summary>
    class FaultConsumer : IConsumer<Fault>
    {
        public Task Consume(ConsumeContext<Fault> context)
        {
            return Task.CompletedTask;
        }
    }
}
