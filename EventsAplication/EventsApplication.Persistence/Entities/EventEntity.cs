using EventsApplication.Domain.Enums;
using EventsApplication.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EventsApplication.Persistence.Entities
{
    [Index(nameof(Category))]
    public class EventEntity : Entity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventTime { get; set; }

        [Required]
        public Guid PlaceId { get; set; }

        public PlaceEntity? EventPlace { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        [Required]
        public bool IsFull { get; set; }

        public List<EventSubscriptionEntity> Subscriptions { get; set; } = [];

        public string EventImageName { get; set; } = string.Empty;

        [Required]
        public EventCategory Category { get; set; }
    }
}

