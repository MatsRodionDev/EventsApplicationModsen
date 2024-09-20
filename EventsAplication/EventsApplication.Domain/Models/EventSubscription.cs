namespace EventsApplication.Domain.Models
{
    public class EventSubscription
    {
        public Guid Id { get; set; }

        public DateTime RegisterDate { get; set; }

        public User User { get; set; }

        public Guid UserId { get; set; }

        public Event Event { get; set; }

        public Guid EventId { get; set; }
    }
}
