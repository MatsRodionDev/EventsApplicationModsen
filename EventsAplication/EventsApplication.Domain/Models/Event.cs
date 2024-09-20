using EventsApplication.Domain.Enums;

namespace EventsApplication.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventTime { get; set; }

        public Guid PlaceId { get; set; }

        public Place? EventPlace { get; set; }

        public EventCategory Category { get; set; }

        public int MaxParticipants { get; set; }

        public bool IsFool { get; set; }

        public bool IsEnded { get; set; }

        public List<EventSubscription>? Subscriptions { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string EventImageName { get; set; } = string.Empty;
    }
}
