using EventsApplication.Application.Common;
using EventsApplication.Application.Common.Interfaces.Hashers;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;
using EventsApplication.Application.Common.Dto;
using EventsApplication.Application.UseCases.Commands.Users;

namespace EventsApplication.Application.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtOptions _jwtOptions;
        private readonly ICacheService _cacheService;

        public LoginCommandHandler(
            IJwtTokenService jwtTokenService,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IOptions<JwtOptions> options,
            ICacheService cacheService)
        {
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtOptions = options.Value;
            _cacheService = cacheService;
        }

        public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserReporsitory.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                throw new BadRequestException("Invalid login or password");
            }

            var refreshToken = await _cacheService.GetAsync<string>(
                CacheKeysProvider.GetRefreshTokenKey(user.Id),
                cancellationToken);

            if(refreshToken is not null)
            {
                throw new BadRequestException("The email has already been logged in");
            }

            if(!user.IsActivated)
            {
                throw new BadRequestException("Account doesn't activated");
            }

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new BadRequestException("Invalid login or password");
            }

            var accesToken = _jwtTokenService.GenerateAccesToken(user.Id, user.UserRole);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(user.Id, user.UserRole);

            var refreshKey = CacheKeysProvider.GetRefreshTokenKey(user.Id);

            await _cacheService.SetAsync(
                refreshKey, 
                newRefreshToken, 
                TimeSpan.FromDays(_jwtOptions.RefreshTokenExpiresDays), 
                cancellationToken);

            return new TokenResponse(accesToken, newRefreshToken);
        }
    }
}
