namespace EventsApplication.Presentation.Dto
{
    public class EventResponse 
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventTime { get; set; }

        public string Place { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int MaxParticipants { get; set; }

        public bool IsFull { get; set; }

        public bool IsEnded { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}
