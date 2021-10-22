using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StackUnderflow.Common.Interfaces;

namespace StackUnderflow.Api.Middlewares
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;

        public UnitOfWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork uow)
        {
            await _next(context);
            await uow.SaveAsync();
        }
    }
}
