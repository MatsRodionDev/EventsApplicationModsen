using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Users
{
    public record GetRegisteredUserQuery(Guid UserId) : IRequest<User>;
}
