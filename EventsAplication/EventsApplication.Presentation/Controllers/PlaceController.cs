using AutoMapper;
using EventsAplication.Api.Constants;
using EventsAplication.Presentation.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventsApplication.Application.UseCases.Commands.Places;
using EventsApplication.Application.UseCases.Queries.Places;

namespace EventsApplication.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlaceController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlacesAsync(CancellationToken cancellationToken)
        {
            var query = new GetAllPlacesQuery();

            var places = await _mediator.Send(query, cancellationToken);

            return Ok(places);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{placeId}")]
        public async Task<IActionResult> DaletePlaceByIdAsync(Guid placeId, CancellationToken cancellationToken)
        {
            var command = new DeletePlaceCommand(placeId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaceAsync(Guid id, [FromBody] UpdatePlaceDto dto, CancellationToken cancellationToken)
        { 
            var command = new UpdatePlaceCommand(id, dto.Name);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePlaceAsync([FromBody] CreatePlaceCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
