using EventsApplication.Application.Common.Profiles;
using EventsApplication.Application.Places.Commands.CreatePlace;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventsApplication.Application.Common.DI
{
    public static class ApplicationLayerRegister
    {
        public static void RegisterApplicationLayerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(typeof(ApplicationProfile));
        }
    }
}
    