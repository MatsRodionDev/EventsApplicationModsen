using AutoMapper;
using EventsAplication.Api.Constants;
using EventsAplication.Presentation.Dto;
using EventsApplication.Presentation.Dto;
using EventsApplication.Application.Events.Commands.CreateEvent;
using EventsApplication.Application.Events.Commands.DeleteEvent;
using EventsApplication.Application.Events.Commands.DeleteEventImage;
using EventsApplication.Application.Events.Commands.UpdateEvent;
using EventsApplication.Application.Events.Commands.UpdateEventImage;
using EventsApplication.Application.Events.Queries.GetAll;
using EventsApplication.Application.Events.Queries.GetById;
using EventsApplication.Application.Events.Queries.GetByName;
using EventsApplication.Application.Events.Queries.GetByParameters;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApplication.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public EventController(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            var query = new GetAllEventsQuery();

            var events = await _mediator.Send(query, cancellationToken);

            var eventsResponse = _mapper.Map<List<EventResponse>>(events);

            return Ok(eventsResponse);
        }

        [HttpGet]
        public async Task<List<EventResponse>> GetEventsByParametersAsync([FromQuery] GetEventsByParametersQuery query, CancellationToken cancellationToken)
        {
            var events = await _mediator.Send(query, cancellationToken);

            var eventsResponse = _mapper.Map<List<EventResponse>>(events);

            return eventsResponse;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetEventByIdQuery(id);

            var eventById = await _mediator.Send(query, cancellationToken);

            var eventResponse = _mapper.Map<EventResponse>(eventById);

            return Ok(eventResponse);
        }


        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetEventByNameAsync(string name, CancellationToken cancellationToken)
        {
            var query = new GetEventsByNameQuery(name);

            var eventByName = await _mediator.Send(query, cancellationToken);

            var eventResponse = _mapper.Map<List<EventResponse>>(eventByName);

            return Ok(eventResponse);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPost]
        public async Task CreateEventAsync([FromForm] CreateEventCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPut("id")]
        public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] UpdateEventDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateEventCommand(id, dto.Name, dto.Description, dto.EventTime, dto.PlaceId, dto.Category, dto.MaxParticipants);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteEventCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPatch("{id}/image")]
        public async Task<IActionResult> UpdateImageToEventAsync(Guid id, [FromForm] UpdateEventImageDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateEventImageCommand(id, dto.Image);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id}/image")]
        public async Task<IActionResult> DeleteEventImageByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteEventImageCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent(); 
        }
    }
}
