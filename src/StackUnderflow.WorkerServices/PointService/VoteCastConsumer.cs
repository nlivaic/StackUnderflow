using MassTransit;
using StackUnderflow.WorkerServices.PointServices;
using StackUnderflow.Core.Events;
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

        /// <summary>
        /// Make this method throw if you want to see MassTransit error handling.
        /// in action.
        /// </summary>
        public async Task Consume(ConsumeContext<VoteCast> context) =>
            await _pointService.CalculateAsync(context.Message.UserId, context.Message.VoteType);
    }
}
