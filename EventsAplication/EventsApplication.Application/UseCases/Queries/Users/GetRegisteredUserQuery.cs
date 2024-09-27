using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Users.Queries.GetRegisteredUser
{
    public record GetRegisteredUserQuery(Guid UserId) : IRequest<User>;
}
