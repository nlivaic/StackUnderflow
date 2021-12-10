﻿using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StackUnderflow.Api.Helpers
{
    public static class ActivityLoggerExtensions
    {
        public static IHost AddActivityLogging(this IHost host)
        {
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
