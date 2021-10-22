using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StackUnderflow.Common.Interfaces;

namespace StackUnderflow.Api.Middlewares
{
    public class EventPublisherMiddleware
    {
        private readonly RequestDelegate _next;

        public EventPublisherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRegisteredEventPublisher eventPublisher)
        {
            await _next(context);
            await eventPublisher.PublishAll();
        }
    }
}
