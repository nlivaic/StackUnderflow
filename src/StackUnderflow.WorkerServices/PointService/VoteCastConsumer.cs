using MassTransit;
using StackUnderflow.Core.Events;
using StackUnderflow.Core.Interfaces;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.PointService
{
    class VoteCastConsumer : IConsumer<VoteCast>
    {
        private readonly IPointService _pointService;

        public VoteCastConsumer(IPointService pointService)
        {
            _pointService = pointService;
        }

        public async Task Consume(ConsumeContext<VoteCast> context) =>
            throw new System.Exception("foofoofoo");
            //await _pointService.CalculateAsync(context.Message.UserId, context.Message.VoteType);
    }
}
