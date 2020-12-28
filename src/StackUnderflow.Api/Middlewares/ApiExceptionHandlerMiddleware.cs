using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace StackUnderflow.API.Middlewares
{
    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;
        private readonly ApiExceptionHandlerOptions _options;

        public ApiExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ApiExceptionHandlerMiddleware> logger,
            ApiExceptionHandlerOptions options)
        {
            _next = next;
            _logger = logger;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next/*.Invoke*/(context);
            }
            catch (System.Exception ex)
            {
                HandleError(context, ex);
            }
        }

        private async void HandleError(HttpContext context, Exception ex)
        {
            var apiError = new ApiError
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occurred in the API. Please use provided Id and get in touch with support."
            };
            _options.ApiErrorHandler?.Invoke(context, ex, apiError);
            var innermostException = GetInnermostException(ex);
            _logger.LogError(innermostException, innermostException.Message + " -- {ErrorId}", apiError.Id);
            var result = JsonConvert.SerializeObject(apiError);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(result);
        }

        private Exception GetInnermostException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                var inner = GetInnermostException(ex.InnerException);
            }
            return ex;
        }
    }
}
