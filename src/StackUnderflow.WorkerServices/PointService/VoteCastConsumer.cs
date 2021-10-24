using System.Threading.Tasks;
using MassTransit;
using StackUnderflow.Core.Events;
using StackUnderflow.WorkerServices.PointServices;

namespace StackUnderflow.WorkerServices.PointService
{
    public class VoteCastConsumer : IConsumer<IVoteCast>
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
        /// <param name="context">Consumer context.</param>
        public async Task Consume(ConsumeContext<IVoteCast> context) =>
            await _pointService.CalculateAsync(context.Message.UserId, context.Message.VoteType);
    }
}
