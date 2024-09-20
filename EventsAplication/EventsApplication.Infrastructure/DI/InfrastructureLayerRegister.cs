using EventsApplication.Application.Common;
using EventsApplication.Application.Common.Interfaces.Hashers;
using EventsApplication.Application.Common.Interfaces.Providers;
using EventsApplication.Application.Common.Interfaces.Queues;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Enums;
using EventsApplication.Infrastructure.Auth;
using EventsApplication.Infrastructure.BackgroundServices;
using EventsApplication.Infrastructure.Hashers;
using EventsApplication.Infrastructure.Queues;
using EventsApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Minio.DataModel.Args;
using System.Text;

namespace EventsApplication.Infrastructure.DI
{
    public static class InfrastructureLayerRegister
    {
        public static void RegisterInfrastructureLayerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["access"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.AddRequirements(new RoleRequirment(Role.Admin));
                });

                options.AddPolicy("Registered", policy =>
                {
                    policy.AddRequirements(new RoleRequirment(Role.User, Role.Admin));
                });

                options.AddPolicy("User", policy =>
                {
                    policy.AddRequirements(new RoleRequirment(Role.User));
                });
            });

            services.AddSingleton<IAuthorizationHandler,RoleRequirmentHandler>();

            services.AddSingleton<IEventUpdateQueue, UpdateEventQueue>();

            services.AddScoped<ICustomClaimsKeysProvider, CustomClaimsKeysProvider>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddHostedService<UpdatedEventEmailSender>();
        }

    }
}
