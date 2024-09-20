using EventsApplication.Domain.Enums;

namespace EventsApplication.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public Role UserRole { get; set; } = Role.User;

        public bool IsActivated { get; set; }

        public List<EventSubscription> EventSubscriptions { get; set; } = [];
    }
}
