﻿using EventsApplication.Application.Common;
using EventsApplication.Application.Common.Interfaces.Services;
using MediatR;

namespace EventsApplication.Application.Users.Commands.LogOut
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
