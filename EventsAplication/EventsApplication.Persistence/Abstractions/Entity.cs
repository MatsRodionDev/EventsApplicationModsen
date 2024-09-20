using EventsApplication.Application.Common.Interfaces.Entity;

namespace EventsApplication.Persistence.Abstractions
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; } 
    }
}
