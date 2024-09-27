using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.Places.Queries.GetAll
{
    public record GetAllPlacesQuery() : IRequest<List<Place>>;
}
