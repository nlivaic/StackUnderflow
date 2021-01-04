using System;
using Microsoft.AspNetCore.Builder;

namespace StackUnderflow.Api.Middlewares
{
    public static class ApiExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseApiExceptionHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseApiExceptionHandler(
            this IApplicationBuilder builder,
            Action<ApiExceptionHandlerOptions> configureOptions)
        {
            var options = new ApiExceptionHandlerOptions();
            configureOptions(options);
            return builder.UseMiddleware<ApiExceptionHandlerMiddleware>(options);
        }
    }
}