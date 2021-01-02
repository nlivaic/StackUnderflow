using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using StackUnderflow.Api.Helpers;

namespace StackUnderflow.Api.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly IScopeInformation _scopeInformation;
        private readonly ILogger<LoggingFilter> _logger;
        private IDisposable _hostScope;
        private IDisposable _userScope;

        public LoggingFilter(
            IScopeInformation scopeInformation,
            ILogger<LoggingFilter> logger)
        {
            _scopeInformation = scopeInformation;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _userScope = _logger.BeginScope(
                new Dictionary<string, string>
                {
                    { "UserId", "Anonymous" },
                    { "Claims", ""/*context.HttpContext.User.Claims*/}
                });
            _hostScope = _logger.BeginScope(_scopeInformation.Host);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _hostScope?.Dispose();
            _userScope?.Dispose();
        }
    }
}