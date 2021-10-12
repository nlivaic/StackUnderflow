using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.WorkerServices.PointServices;
using StackUnderflow.WorkerServices.Tags;
using StackUnderflow.WorkerServices.Users;
using StackUnderflow.WorkerServices.Votes;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.WorkerServices
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStackUnderflowApplicationHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILimitsService, LimitsService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}
