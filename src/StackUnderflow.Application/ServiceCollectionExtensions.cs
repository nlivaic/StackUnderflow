using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Application.Pipelines;
using StackUnderflow.Application.PointServices;
using StackUnderflow.Application.Tags;
using StackUnderflow.Application.Users;
using StackUnderflow.Application.Votes;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.WorkerServices.Users;

namespace StackUnderflow.Application
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
            services.AddPipelines();

            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        }
    }
}
