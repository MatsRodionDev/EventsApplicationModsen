using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Users.Queries.GetRegisteredUser
{
    public class GetRegisteredUserQueryHandler : IRequestHandler<GetRegisteredUserQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRegisteredUserQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetRegisteredUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserReporsitory.GetByIdAsync(request.UserId, cancellationToken);

            if(user is null)
            {
                throw new NotFoundException("User doesn't exist");
            }

            return user;
        }
    }
}
