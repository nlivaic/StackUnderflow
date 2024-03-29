﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackUnderflow.Identity.DbContexts;
using StackUnderflow.Identity.Services;
using SparkRoseDigital.Infrastructure.Email.DependencyInjection;

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

                var connString = new NpgsqlConnectionStringBuilder(_configuration.GetConnectionString("StackUnderflowIdentityDb"))
                {
                    Username = _configuration["POSTGRES_USER"],
                    Password = _configuration["POSTGRES_PASSWORD"]
                };
                options.UseNpgsql(connString.ConnectionString);
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
            //.AddTestUsers(TestUsers.Users)

            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            builder.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    var connString = new NpgsqlConnectionStringBuilder(_configuration.GetConnectionString("StackUnderflowIdentityDb"))
                    {
                        Username = _configuration["POSTGRES_USER"],
                        Password = _configuration["POSTGRES_PASSWORD"]
                    };
                    builder.UseNpgsql(
                        connString.ConnectionString,
                        options => options.MigrationsAssembly(migrationsAssembly)
                    );
                };
                options.DefaultSchema = "Configuration";
            });
            builder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    var connString = new NpgsqlConnectionStringBuilder(_configuration.GetConnectionString("StackUnderflowIdentityDb"))
                    {
                        Username = _configuration["POSTGRES_USER"],
                        Password = _configuration["POSTGRES_PASSWORD"]
                    };
                    builder.UseNpgsql(
                        connString.ConnectionString,
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
                    AllowAll = false,
                    AllowedOrigins = new List<string>
                    {
                        "http://localhost:3000"
                    }
                };
            });
            services.AddAuthentication().AddFacebook(
                "Facebook",
                options =>
                {
                    options.ClientId = _configuration["Facebook:ClientId"];
                    options.ClientSecret = _configuration["Facebook:ClientSecret"];
                    options.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;
                });
            services.AddEmailService(opts =>
            {
                opts.DoNotReplyEmail = _configuration["EmailSettings:DoNotReply"];
                opts.Host = _configuration["EmailSettings:Host"];
                opts.Password = _configuration["EmailSettings:Password"];
                opts.Port = int.Parse(_configuration["EmailSettings:Port"]);
                opts.Username = _configuration["EmailSettings:Username"];
            });
            // To be used for simple, non-cryptographically secure random number generation.
            services.AddSingleton<Random>(new Random());
            services.AddSingleton<IClaimsMappingFactory, ClaimsMappingFactory>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use headers forwarded by reverse proxy.
            var forwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            forwardedHeaderOptions.KnownNetworks.Clear();
            forwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardedHeaderOptions);

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
        }
    }
}
