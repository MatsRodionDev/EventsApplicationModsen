﻿using EventsApplication.Domain.Enums;
using EventsApplication.Domain.Models;
using MediatR;

namespace EventsApplication.Application.UseCases.Queries.Events
{
    public record GetEventsByParametersQuery(
        DateTime? Date,
        EventCategory? Category,
        Guid? PlaceId,
        string Name = "",
        int PageSize = 5,
        int PageNumber = 1) : IRequest<List<Event>>;
}
