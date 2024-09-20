namespace EventsApplication.Application.Users.Commands
{
    public record TokenResponse(
        string AccesToken,
        string RefreshToken);
}
