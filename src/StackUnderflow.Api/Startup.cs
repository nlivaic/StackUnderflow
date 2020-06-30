using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackUnderflow.Core.Entities;
using AutoMapper;
using StackUnderflow.Data;
using StackUnderflow.Data.Repositories;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Services;
using StackUnderflow.Common.Interfaces;

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
            services.AddControllers();

            services.AddDbContext<StackUnderflowDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("StackUnderflowDbConnection"));
                if (_hostEnvironment.IsDevelopment())
                    options.EnableSensitiveDataLogging(true);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<Tag>, Repository<Tag>>();        // @nl
            services.AddScoped<IRepository<User>, Repository<User>>();        // @nl
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddTransient<ITagService, TagService>();
            services.AddSingleton<ILimits, Limits>();
            services.AddTransient<IVoteable, Voteable>();
            services.AddTransient<ICommentable, Commentable>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(QuestionRepository).Assembly);
            services.AddTransient<IQuestionService, QuestionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
