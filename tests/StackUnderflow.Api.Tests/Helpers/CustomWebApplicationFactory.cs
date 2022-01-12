﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SparkRoseDigital.Infrastructure.MessageBroker;
using StackUnderflow.Data;

namespace StackUnderflow.Api.Tests.Helpers
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public SqliteConnection KeepAliveConnection { get; }

        public CustomWebApplicationFactory()
        {
            KeepAliveConnection = new SqliteConnection("DataSource=:memory:");
            KeepAliveConnection.Open();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            }
            Environment.SetEnvironmentVariable("MessageBroker__Writer__SharedAccessKeyName", "Test");
            Environment.SetEnvironmentVariable("MessageBroker__Writer__SharedAccessKey", "Test");
            Environment.SetEnvironmentVariable("MessageBroker__Reader__SharedAccessKeyName", "Test");
            Environment.SetEnvironmentVariable("MessageBroker__Reader__SharedAccessKey", "Test");
            return base.CreateHostBuilder()
                .ConfigureHostConfiguration(
                    config => config.AddEnvironmentVariables("ASPNETCORE"));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication("Test")
                    .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test", null);
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<StackUnderflowDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(IEventRegister)
                    || d.ServiceType == typeof(IRegisteredEventPublisher)).ToList();
                if (descriptors.Count() == 2)
                {
                    services.Remove(descriptors[0]);
                    services.Remove(descriptors[1]);
                }
                services.AddScoped<IEventRegister, FakeEventRegister>();
                services.AddScoped<IRegisteredEventPublisher, FakeEventRegister>();
                services.AddDbContext<StackUnderflowDbContext>(options =>
                {
                    options.UseSqlite(KeepAliveConnection);
                });
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var ctx = scopedServices.GetRequiredService<StackUnderflowDbContext>();
                ctx.Database.Migrate();
                ctx.Seed();
                ctx.SaveChanges();
            });
        }
    }
}
