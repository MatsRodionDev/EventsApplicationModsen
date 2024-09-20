namespace EventsAplication.Presentation.Dto
{
    public class SubscriptionWithUserResponse
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
