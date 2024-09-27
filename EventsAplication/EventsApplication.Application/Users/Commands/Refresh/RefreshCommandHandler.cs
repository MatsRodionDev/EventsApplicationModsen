using EventsApplication.Application.Common;
using EventsApplication.Application.Common.Dto;
using EventsApplication.Application.Common.Interfaces.Providers;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Enums;
using EventsApplication.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;

namespace EventsApplication.Application.Users.Commands.Refresh
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, TokenResponse>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICustomClaimsKeysProvider _claimsKeysProvider;
        private readonly JwtOptions _jwtOptions;
        private readonly ICacheService _cacheService;

        public RefreshCommandHandler(
            IJwtTokenService jwtTokenService,
            ICustomClaimsKeysProvider claimsKeysProvider,
            IOptions<JwtOptions> options,
            ICacheService cacheService)
        {
            _jwtTokenService = jwtTokenService;
            _claimsKeysProvider = claimsKeysProvider;
            _jwtOptions = options.Value;
            _cacheService = cacheService;
        }

        public async Task<TokenResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var claims = _jwtTokenService.GetClaimsFromToken(request.RefreshToken);

            if (claims is null)
            {
                throw new BadRequestException("Invalid refresh token");
            }

            var userId = Guid.Parse(claims![_claimsKeysProvider.UserId].ToString()!);

            var refreshKey = CacheKeysProvider.GetRefreshTokenKey(userId);

            var token = await _cacheService.GetAsync<string>(refreshKey, cancellationToken);

            if (token is null || token != request.RefreshToken)
            {
                throw new BadRequestException("Invalid refresh token");
            }

            var userRole = Enum.Parse<Role>(claims[_claimsKeysProvider.UserRole].ToString()!);

            var accesToken = _jwtTokenService.GenerateAccesToken(userId, userRole);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(userId, userRole);

            await _cacheService.SetAsync(refreshKey, refreshToken, TimeSpan.FromDays(_jwtOptions.RefreshTokenExpiresDays), cancellationToken);

            return new TokenResponse(accesToken, refreshToken);
        }
    }
}
