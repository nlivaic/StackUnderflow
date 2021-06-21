using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Application.Votes;

namespace StackUnderflow.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStackUnderflowApplicationHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
            services.AddScoped<IVoteService, VoteService>();
        }
    }
}
