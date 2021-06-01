using Microsoft.Extensions.DependencyInjection;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Data.Repositories;

namespace StackUnderflow.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSpecificRepositories(this IServiceCollection services)
        {
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<ILimitsRepository, LimitsRepository>();
        }

        public static void AddGenericRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
