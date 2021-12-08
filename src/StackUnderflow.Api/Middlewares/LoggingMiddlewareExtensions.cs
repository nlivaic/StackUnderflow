using Microsoft.AspNetCore.Builder;

namespace StackUnderflow.Api.Middlewares
{
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseHostLoggingMiddleware(
            this IApplicationBuilder builder) => builder.UseMiddleware<HostLoggingMiddleware>();

        public static IApplicationBuilder UseUserLoggingMiddleware(
            this IApplicationBuilder builder) => builder.UseMiddleware<UserLoggingMiddleware>();
    }
}
