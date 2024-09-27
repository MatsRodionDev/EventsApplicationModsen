using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Places
{
    public record GetAllPlacesQuery() : IRequest<List<Place>>;
}
