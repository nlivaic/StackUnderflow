using System;
using Microsoft.AspNetCore.Http;

namespace StackUnderflow.API.Middlewares
{
    public class ApiExceptionHandlerOptions
    {
        public Action<HttpContext, Exception, ApiError> ApiErrorHandler { get; set; }
    }
}
