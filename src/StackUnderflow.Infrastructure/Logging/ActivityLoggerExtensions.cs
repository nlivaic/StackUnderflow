using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StackUnderflow.Infrastructure.Logging
{
    public static class ActivityLoggerExtensions
    {
        /// <summary>
        /// Adds a listener so every new Activity is logged.
        /// Makes sure the tracing is W3C Trace Context compliant.
        /// Makes tracing easier due to some components (e.g. HttpClient, MassTransit)
        /// create their own Activity and the attached listener allows end-to-end tracing.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <returns>Host with activity logging configured.</returns>
        public static IHost AddW3CTraceContextActivityLogging(this IHost host)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Activity>>();
                ActivitySource.AddActivityListener(new ActivityListener
                {
                    ShouldListenTo = _ => true,
                    ActivityStarted = activity =>
                    {
                        logger.LogInformation("Activity started: {operationName}.", activity.OperationName);
                    },
                    ActivityStopped = activity =>
                    {
                        logger.LogInformation("Activity stopped: {operationName}.", activity.OperationName);
                    }
                });
            }
            return host;
        }
    }
}
