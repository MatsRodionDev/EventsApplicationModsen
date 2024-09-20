using EventsApplication.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EventsApplication.Persistence.Entities
{
    public class EventSubscriptionEntity : Entity
    {
        [Required]
        public DateTime RegisterDate { get; set; }

        [Required]
        public Guid EventId { get; set; }

        public EventEntity? Event { get; set; } 

        [Required]
        public Guid UserId { get; set; }

        public UserEntity? User { get; set; }
    }
}
