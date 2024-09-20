using EventsApplication.Domain.Enums;

namespace EventsAplication.Presentation.Dto
{
    public class UpdateEventDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventTime { get; set; }

        public Guid PlaceId { get; set; }

        public string Category { get; set; } = string.Empty;

        public int MaxParticipants { get; set; }
    }
}
