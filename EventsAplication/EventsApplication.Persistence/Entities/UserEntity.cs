using EventsApplication.Domain.Enums;
using EventsApplication.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace EventsApplication.Persistence.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserEntity : Entity
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public Role UserRole { get; set; }

        [Required]
        public bool IsActivated { get; set; }

        public List<EventSubscriptionEntity> EventSubscriptions { get; set; } = [];
    }
}
