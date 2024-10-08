﻿using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Users;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Users
{
    public class ActivateCommandHandler : IRequestHandler<ActivateCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActivateCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserReporsitory.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException($"User with id {request.UserId} doesn't exist");
            }

            if (user.IsActivated)
            {
                throw new BadRequestException($"User with id {request.UserId} already was activated ");
            }

            user.IsActivated = true;

            _unitOfWork.UserReporsitory.Update(user);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
