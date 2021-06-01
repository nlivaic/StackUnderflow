using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Services;

namespace StackUnderflow.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<BaseLimits, Limits>();
            services.AddScoped<ILimitsService, LimitsService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPointService, PointService>();
        }
    }
}
