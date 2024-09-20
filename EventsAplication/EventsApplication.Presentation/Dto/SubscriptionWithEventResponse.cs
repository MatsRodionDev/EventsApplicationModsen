namespace EventsAplication.Presentation.Dto
{
    public class SubscriptionWithEventResponse
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventTime { get; set; }

        public string EventPlace { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public bool IsFull { get; set; }

        public DateTime RegisterDate { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}
