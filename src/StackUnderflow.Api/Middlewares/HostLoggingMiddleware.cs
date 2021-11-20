using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackUnderflow.Api.Helpers;

namespace StackUnderflow.Api.Middlewares
{
    public class HostLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HostLoggingMiddleware> _logger;
        private readonly IScopeInformation _scopeInformation;
        private IDisposable _hostScope;


        public HostLoggingMiddleware(
            RequestDelegate next,
            ILogger<HostLoggingMiddleware> logger,
            IScopeInformation scopeInformation)
        {
            _next = next;
            _logger = logger;
            _scopeInformation = scopeInformation;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            try
            {
                _hostScope = _logger.BeginScope(_scopeInformation.Host);
                await _next(context);
            }
            finally
            {
                _hostScope.Dispose();
            }
        }
    }
}
