using MassTransit;
using StackUnderflow.Core.Events;
using StackUnderflow.Core.Interfaces;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.PointService
{
    class VoteConsumer : IConsumer<VoteCast>
    {
        private readonly IPointService _pointService;

        public VoteConsumer(IPointService pointService)
        {
            _pointService = pointService;
        }

        public async Task Consume(ConsumeContext<VoteCast> context) =>
            await _pointService.CalculateAsync(context.Message.UserId, context.Message.VoteType);
    }
}
