using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventsApplication.Persistence
{
    public class ApplicationDBContext : DbContext
    {
       

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<EventEntity> Events { get; set; }

        public DbSet<EventSubscriptionEntity> Subscriptions { get; set; }

        public DbSet<PlaceEntity> Places { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
