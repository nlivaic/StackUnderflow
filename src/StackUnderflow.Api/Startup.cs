using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using StackUnderflow.Data;
using StackUnderflow.Data.Repositories;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Services;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Profiles;
using Microsoft.AspNetCore.Mvc.Filters;
using StackUnderflow.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.API.Services.Sorting;
using FluentValidation.AspNetCore;

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
                options.UseNpgsql(_configuration.GetConnectionString("StackUnderflowDbConnection"));
                if (_hostEnvironment.IsDevelopment())
                    options.EnableSensitiveDataLogging(true);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ITagService, TagService>();
            services.AddSingleton<ILimits, Limits>();
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(QuestionProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Please try again later.");
                        // @nl: log.
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
