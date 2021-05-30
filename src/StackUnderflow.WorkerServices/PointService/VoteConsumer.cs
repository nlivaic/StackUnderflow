using MassTransit;
using StackUnderflow.Core.Events;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.PointService
{
    class VoteConsumer : IConsumer<VoteCast>
    {
        public Task Consume(ConsumeContext<VoteCast> context)
        {
            //throw new Exception("Bad things happened in consumer.");
            return Task.CompletedTask;
        }
    }
}
