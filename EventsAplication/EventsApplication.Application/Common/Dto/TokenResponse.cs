namespace EventsApplication.Application.Common.Dto
{
    public record TokenResponse(
        string AccesToken,
        string RefreshToken);
}
