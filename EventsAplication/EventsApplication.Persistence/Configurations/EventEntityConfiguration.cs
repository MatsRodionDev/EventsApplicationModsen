using EventsApplication.Domain.Enums;
using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApplication.Persistence.Configurations
{
    public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasMany(e => e.Subscriptions)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EventPlace)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.PlaceId);

            builder.Property(e => e.Category)
                .HasConversion(
                    v => v.ToString(),  
                    v => (EventCategory)Enum.Parse(typeof(EventCategory), v));

            builder.HasData(
                new EventEntity
                {
                    Id = Guid.Parse("2b84fe47-9c93-43f8-b7df-2be289eb1bf1"),
                    Name = "Music Festival",
                    Description = "A great music festival featuring various artists.",
                    EventTime = new DateTime(2024, 6, 15, 18, 0, 0),
                    IsFull = false,
                    PlaceId = Guid.Parse("d9cadc88-99da-4eea-8ce6-9af7431642e1"),
                    Category = EventCategory.Music,
                    MaxParticipants = 50,
                    EventImageName = ""
                },
                new EventEntity
                {
                    Id = Guid.Parse("641db76b-c08d-46e4-9b72-8242662c66a3"),
                    Name = "Tech Conference",
                    Description = "An insightful conference about the latest in technology.",
                    EventTime = new DateTime(2024, 9, 10, 9, 0, 0),
                    IsFull = false,
                    PlaceId = Guid.Parse("d9cadc88-99da-4eea-8ce6-9af7431642e1"),
                    Category = EventCategory.Technology,
                    MaxParticipants = 30,
                    EventImageName = ""
                },
                new EventEntity
                {
                    Id = Guid.Parse("bbf4fa4f-37c1-4fa4-88cf-8f02f67c8353"),
                    Name = "Art Exhibition",
                    Description = "Explore the latest artworks from renowned artists.",
                    EventTime = new DateTime(2024, 7, 20, 10, 0, 0),
                    IsFull = false,
                    PlaceId = Guid.Parse("0225d3f4-33d2-4068-8275-ee56e4681e3e"),
                    Category = EventCategory.Art,
                    MaxParticipants = 20,
                    EventImageName = ""
                },
                new EventEntity
                {
                    Id = Guid.Parse("a267be90-4e7f-4dd8-a55f-e62b5de45b82"),
                    Name = "Health Workshop",
                    Description = "Learn about healthy living and wellness practices.",
                    EventTime = new DateTime(2024, 8, 5, 15, 0, 0),
                    IsFull = false,
                    PlaceId = Guid.Parse("d9cadc88-99da-4eea-8ce6-9af7431642e1"),
                    Category = EventCategory.Health,
                    MaxParticipants = 15,
                    EventImageName = ""
                },
                new EventEntity
                {
                    Id = Guid.Parse("1c6de99a-34ac-4be9-8e61-c3b7aa7a0224"),
                    Name = "Food Festival",
                    Description = "A delightful festival showcasing various cuisines.",
                    EventTime = new DateTime(2024, 10, 10, 12, 0, 0),
                    IsFull = false,
                    PlaceId = Guid.Parse("0225d3f4-33d2-4068-8275-ee56e4681e3e"),
                    Category = EventCategory.Food,
                    MaxParticipants = 1000,
                    EventImageName = ""
                }
            );
        }
    }
}
