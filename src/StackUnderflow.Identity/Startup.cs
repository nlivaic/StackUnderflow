﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackUnderflow.Identity.DbContexts;

namespace StackUnderflow.Identity
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("StackUnderflowIdentityDb"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.SignIn.RequireConfirmedEmail = true;
                opts.User.RequireUniqueEmail = false;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_-.1234567890";
            });

            var builder = services
                .AddIdentityServer(options =>
                {
                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients);
            // .AddTestUsers(TestUsers.Users)

            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            builder.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    builder.UseNpgsql(
                        _configuration.GetConnectionString("StackUnderflowIdentityDb"),
                        options => options.MigrationsAssembly(migrationsAssembly)
                    );
                };
                options.DefaultSchema = "Configuration";
            });
            builder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    builder.UseNpgsql(
                        _configuration.GetConnectionString("StackUnderflowIdentityDb"),
                        options => options.MigrationsAssembly(migrationsAssembly)
                    );
                };
                options.DefaultSchema = "Operational";
            });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<IdentityServer4.Services.DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true
                };
            });

        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            SeedTestData(app, _environment);
        }

        private static void SeedTestData(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                return;
            }
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                if (!configurationDbContext.Clients.Any())
                {
                    configurationDbContext.Clients.AddRange(Config.Clients.Select(c => c.ToEntity()));
                }
                if (!configurationDbContext.ApiResources.Any())
                {
                    configurationDbContext.ApiResources.AddRange(Config.ApiResources.Select(c => c.ToEntity()));
                }
                if (!configurationDbContext.ApiScopes.Any())
                {
                    configurationDbContext.ApiScopes.AddRange(Config.ApiScopes.Select(c => c.ToEntity()));
                }
                if (!configurationDbContext.IdentityResources.Any())
                {
                    configurationDbContext.IdentityResources.AddRange(Config.IdentityResources.Select(c => c.ToEntity()));
                }
                configurationDbContext.SaveChanges();
            }
        }
    }
}
