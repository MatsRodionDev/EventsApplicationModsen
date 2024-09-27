using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using MediatR;
using EventsApplication.Application.UseCases.Commands.Users;

namespace EventsApplication.Application.UseCases.Handlers.Commands.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserReporsitory.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException($"User with id {request.Id} doesn't exist");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            _unitOfWork.UserReporsitory.Update(user);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
