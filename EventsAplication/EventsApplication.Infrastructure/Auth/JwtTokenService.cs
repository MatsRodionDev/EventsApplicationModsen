using EventsApplication.Application.Common.Interfaces.Providers;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Enums;
using EventsApplication.Application.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventsApplication.Infrastructure.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ICustomClaimsKeysProvider _claimsKeysProvider;

        public JwtTokenService(
            IOptions<JwtOptions> options,
            ICustomClaimsKeysProvider claimsKeysProvider)
        {
            _jwtOptions = options.Value;
            _claimsKeysProvider = claimsKeysProvider;
        }

        public string GenerateAccesToken(Guid userId, Role userRole)
        {
            Claim[] claims =
            [
                new (_claimsKeysProvider.UserRole, userRole.ToString()),
                new (_claimsKeysProvider.UserId, userId.ToString())
                
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccesTokenExpiresMinutes));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public string GenerateRefreshToken(Guid userId, Role userRole)
        {
            Claim[] claims =
            [
                new (_claimsKeysProvider.UserId, userId.ToString()),
                new (_claimsKeysProvider.UserRole, userRole.ToString()),
                new ("refresh", "refresh")
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpiresDays));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }


        public IDictionary<string, object>? GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                return null;
            }

            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => (object)c.Value);

            return claims;
        }
    }
}
