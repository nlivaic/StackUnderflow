using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace StackUnderflow.Infrastructure.HealthCheck
{
    public static class HealthCheckResponses
    {
        public static async Task WriteJsonResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var healthCheckModel = new HealthCheckModel
            {
                Status = report.Status.ToString()
            };
            foreach (var entry in report.Entries)
            {
                healthCheckModel.AddResult(new HealthCheckResultModel
                {
                    Key = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Description = entry.Value.Description,
                });
            }

            var serializedHealthCheckModel = JsonSerializer.Serialize(healthCheckModel, options);
            await context.Response.WriteAsync(serializedHealthCheckModel);
        }
    }
}
