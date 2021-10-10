using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Application.Services;
using StackUnderflow.Application.Tags;
using StackUnderflow.Application.Users;
using StackUnderflow.Application.Votes;
using StackUnderflow.Core.Interfaces;

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
        }
    }
}
