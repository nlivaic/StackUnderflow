using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StackUnderflow.Api.Middlewares
{
    public class ApiExceptionHandlerOptions
    {
        public Action<HttpContext, Exception, ApiError> ApiErrorHandler { get; set; }
        public Func<HttpContext, Exception, LogLevel> LogLevelHandler { get; set; }
    }
}
