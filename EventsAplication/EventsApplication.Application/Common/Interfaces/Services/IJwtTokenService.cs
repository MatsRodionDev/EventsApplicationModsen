using EventsApplication.Domain.Enums;

namespace EventsApplication.Application.Common.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccesToken(Guid userId, Role userRole);
        string GenerateRefreshToken(Guid userId, Role userRole);
        public IDictionary<string, object>? GetClaimsFromToken(string token);
    }
}
