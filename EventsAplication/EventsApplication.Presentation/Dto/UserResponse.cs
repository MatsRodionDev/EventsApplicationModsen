namespace EventsAplication.Presentation.Dto
{
    public class UserResponse
    {
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }
    }
}
