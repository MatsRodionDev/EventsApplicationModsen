using EventsApplication.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsApplication.Presentation.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDBContext context = scope
                .ServiceProvider.GetService<ApplicationDBContext>();

            context.Database.Migrate();
        }
    }
}
