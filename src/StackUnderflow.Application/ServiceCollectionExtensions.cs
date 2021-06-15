using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace StackUnderflow.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStackUnderflowApplicationHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
        }

    }
}
