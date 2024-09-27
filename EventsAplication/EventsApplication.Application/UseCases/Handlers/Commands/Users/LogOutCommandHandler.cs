using EventsApplication.Application.Common;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.UseCases.Commands.Users;
using MediatR;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Users
{
    public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
    {
        private readonly ICacheService _cacheService;

        public LogOutCommandHandler(
            ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            var refreshKey = CacheKeysProvider.GetRefreshTokenKey(request.UserId);

            await _cacheService.RemoveAsync(refreshKey, cancellationToken);
        }
    }
}
