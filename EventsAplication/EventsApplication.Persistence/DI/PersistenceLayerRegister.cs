using AutoMapper.Extensions.ExpressionMapping;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Persistence.Profiles;
using EventsApplication.Persistence.Repositories;
using EventsApplication.Persistence.UnitOfWorkApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApplication.Persistence.DI
{
    public static class PersistenceLayerRegister 
    {
        public static void RegisterPersistenceDapendencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDBContext))));

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IEventSubscriptionRepository, EventSubscriptionRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(cfg =>
                cfg.AddExpressionMapping(), typeof(PersistenceProfile));
        }
    }
}