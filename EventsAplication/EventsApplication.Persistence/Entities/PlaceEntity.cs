using EventsApplication.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EventsApplication.Persistence.Entities
{
    public class PlaceEntity : Entity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<EventEntity> Events { get; set; } = [];
    }
}
