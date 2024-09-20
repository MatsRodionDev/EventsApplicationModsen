namespace EventsApplication.Application.Common.Interfaces.Providers
{
    public interface ICustomClaimsKeysProvider
    {
        public string UserId { get; }
        public string UserRole { get; }
    }
}
