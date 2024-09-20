using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApplication.Persistence.Configurations
{
    public class EventSubscribtionEntityConfiguration : IEntityTypeConfiguration<EventSubscriptionEntity>
    {
        public void Configure(EntityTypeBuilder<EventSubscriptionEntity> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}
