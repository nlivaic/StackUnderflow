using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using SparkRoseDigital.Infrastructure.Caching;
using SparkRoseDigital.Infrastructure.HealthCheck;
using SparkRoseDigital.Infrastructure.Logging;
using SparkRoseDigital.Infrastructure.MessageBroker;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Middlewares;
using StackUnderflow.Api.Models;
using StackUnderflow.Application;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Application.Questions.Models;
using StackUnderflow.Application.Sorting;
using StackUnderflow.Core;
using StackUnderflow.Core.Entities;
using StackUnderflow.Data;

namespace StackUnderflow.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(configure =>
                {
                    configure.ReturnHttpNotAcceptable = true;
                    configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest));
                    configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status404NotFound));
                    configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(object), StatusCodes.Status406NotAcceptable));
                    configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var actionExecutingContext = actionContext as ActionExecutingContext;
                        var validationProblemDetails = ValidationProblemDetailsFactory.Create(actionContext);
                        if (actionContext.ModelState.ErrorCount > 0
                            && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                        {
                            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            return new UnprocessableEntityObjectResult(validationProblemDetails);
                        }
                        validationProblemDetails.Status = StatusCodes.Status400BadRequest;
                        return new BadRequestObjectResult(validationProblemDetails);
                    };
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddDbContext<StackUnderflowDbContext>(options =>
            {
                var connString = new NpgsqlConnectionStringBuilder(_configuration.GetConnectionString("StackUnderflowDbConnection"))
                {
                    Username = _configuration["POSTGRES_USER"],
                    Password = _configuration["POSTGRES_PASSWORD"]
                };
                options.UseNpgsql(connString.ConnectionString);
                if (_hostEnvironment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(true);
                }
            });

            services.AddGenericRepository();
            services.AddSpecificRepositories();
            services.AddCoreServices();

            services.AddPropertyMappingService(opts =>
                opts.PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<QuestionSummaryGetViewModel, QuestionSummaryGetModel>()
                        .Add(nameof(QuestionSummaryGetViewModel.Username), $"{nameof(User)}.{nameof(User.Username)}")
                        .Add(nameof(QuestionSummaryGetViewModel.HasAcceptedAnswer), nameof(QuestionSummaryGetModel.HasAcceptedAnswer))
                        .Add(nameof(QuestionSummaryGetViewModel.CreatedOn), nameof(QuestionSummaryGetModel.CreatedOn)),
                    new PropertyMapping<AnswerGetViewModel, AnswerGetModel>()
                        .Add(nameof(AnswerGetViewModel.CreatedOn), nameof(AnswerGetModel.CreatedOn))
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddLoggingScopes();
            services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(Comment).Assembly);

            services.AddSingleton<ICache, Cache>();
            services.AddMemoryCache();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "StackUnderflowOpenAPISpecification",
                    new OpenApiInfo
                    {
                        Title = "Stack Underflow API",
                        Version = "v1",
                        Description = "This API allows access to the Stack Underflow Q&A.",
                        Contact = new OpenApiContact
                        {
                            Name = "Nenad Livaic",
                            Url = new Uri("https://github.com/nlivaic")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("https://www.opensource.org/licenses/MIT")
                        },
                        TermsOfService = new Uri("https://www.my-terms-of-service.com")
                    });

                // A workaround for having multiple POST methods on one controller.
                // setupAction.ResolveConflictingActions(r => r.First());
                setupAction.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "StackUnderflow.Api.xml"));
            });

            // Commented out as we are running front end as a standalone app.
            // services.AddSpaStaticFiles(configuration =>
            // {
            //     configuration.RootPath = "ClientApp/build";
            // });
            services.AddCors(o => o.AddPolicy("All", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(Constants.Headers.Pagination);
            }));

            services.AddCors(o => o.AddPolicy("StackUnderflowClient", builder =>
            {
                var allowedOrigins = _configuration["AllowedOrigins"]?.Split(',') ?? Array.Empty<string>();
                builder
                    .WithOrigins(allowedOrigins)
                    .WithHeaders("Authorization", "Content-Type")
                    .WithExposedHeaders(Constants.Headers.Pagination)
                    .WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete);
            }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = _configuration["IdentityProvider:Authority"];       // Our IDP. Middleware uses this to know where to find public keys and endpoints.
                    options.ApiName = _configuration["IdentityProvider:ApiName"];           // Allows the access token validator to check if the access token `audience` is for this API.
                });
            services.AddAuthorization();
            services.AddApiEventPublisher(
                new MessageBrokerConnectionStringBuilder(
                    _configuration.GetConnectionString("MessageBroker"),
                    _configuration["MessageBroker:Writer:SharedAccessKeyName"],
                    _configuration["MessageBroker:Writer:SharedAccessKey"]).ConnectionString);
            services.AddStackUnderflowApplicationHandlers();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services
                .AddHealthChecks()
                .AddDbContextCheck<StackUnderflowDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHostLoggingMiddleware();

            // First use of Logging Exceptions.
            // This instance is here to catch and log any exceptions coming from middlewares
            // executed early in the pipeline.
            app.UseApiExceptionHandler(options =>
            {
                options.ApiErrorHandler = UpdateApiErrorResponse;
                options.LogLevelHandler = LogLevelHandler;
            });

            // Use headers forwarded by reverse proxy.
            app.UseForwardedHeaders();

            // if (env.IsProduction())
            // {
            //    app.UseHsts();
            // }
            app.UseCors("StackUnderflowClient");
            app.UseHttpsRedirection();

            // Commented out as we are running front end as a standalone app.
            // app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/StackUnderflowOpenAPISpecification/swagger.json", "Stack Underflow API");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseUserLoggingMiddleware();

            // Second use of Logging Exceptions.
            // This instance is here to catch and log any exceptions coming from the controllers.
            // The reason for two logging middlewares is we can log user id and claims only
            // after .UseAuthentication() and .UseAuthorization() are executed. So the first
            // .UseApiExceptionHandler() has no access to user id and claims but has access to
            // machine name and thus at least provides some insight into any potential exceptions
            // coming from early in the pipeline. The second .UseApiExceptionHandler() has access
            // to machine name, user id and claims and can log any exceptions from the controllers.
            app.UseApiExceptionHandler(options =>
            {
                options.ApiErrorHandler = UpdateApiErrorResponse;
                options.LogLevelHandler = LogLevelHandler;
            });

            app.UseEndpoints(endpoints =>
            {
                // Liveness check does not include database connectivity check because even a transient
                // error will cause the orchestractor/load balancer to take the service down and restart it.
                // Readiness check includes database connectivity check to tell the orchestractor/load balancer
                // whether all the project dependencies are up and running.
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = _ => false, // No additional health checks.
                });
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponses.WriteJsonResponse
                });
                endpoints.MapControllers();
            });

            // Commented out as we are running front end as a standalone app.
            // app.UseSpa(spa =>
            // {
            //     spa.Options.SourcePath = "ClientApp";
            //     if (env.IsDevelopment())
            //     {
            //         // This is used if starting both front end and back end with the same command.
            //         // spa.UseReactDevelopmentServer(npmScript: "start");
            //         // This is used if starting front end separately from the back end, most likely to get better
            //         // separation. Faster hot reload when changing only front end and not having to go through front end
            //         // rebuild every time you change something on the back end.
            //         spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            //     }
            // });
        }

        /// <summary>
        /// A demonstration of how returned message can be modified.
        /// </summary>
        private void UpdateApiErrorResponse(HttpContext context, Exception ex, ProblemDetails problemDetails)
        {
            // if (ex is LimitNotMappable)
            // {
            //     problemDetails.Detail = "A general error occurred.";
            // }
        }

        /// <summary>
        /// Define cases where a different log level is needed for logging exceptions.
        /// </summary>
        private LogLevel LogLevelHandler(HttpContext context, Exception ex) =>

            // if (ex is Exception)
            // {
            //     return LogLevel.Critical;
            // }
            // return LogLevel.Error;
            LogLevel.Critical;
    }
}
