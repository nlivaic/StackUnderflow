using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackUnderflow.Api.Helpers;

namespace StackUnderflow.Api.Middlewares
{
    public class UserLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserLoggingMiddleware> _logger;
        private IDisposable _userScope;

        public UserLoggingMiddleware(
            RequestDelegate next,
            ILogger<UserLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _userScope = _logger.BeginScope(
                    new Dictionary<string, string>
                    {
                        {
                            "UserId",
                            context.User.Identity.IsAuthenticated ? context.User.UserId().ToString() : "Anonymous"
                        },
                        {
                            "Claims",
                            string.Join(';', context.User.Claims.Select(claim => claim.ToString()))
                        }
                    });
                await _next(context);
            }
            finally
            {
               _userScope.Dispose();
            }
        }
    }
}
