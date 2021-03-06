using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace StackUnderflow.Api.Middlewares
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
            context.Request.EnableBuffering();
            try
            {
                await _next(context);
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
            var logLevel = _options.LogLevelHandler?.Invoke(context, ex) ?? LogLevel.Error;
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                string body = String.Empty;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    body = await reader.ReadToEndAsync();
                }
                _logger.Log(logLevel, innermostException, innermostException.Message + " -- {ErrorId} -- {Body}", apiError.Id, body);

            }
            else
            {
                _logger.Log(logLevel, innermostException, innermostException.Message + " -- {ErrorId}", apiError.Id);
            }
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
