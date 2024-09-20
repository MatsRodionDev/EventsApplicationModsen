﻿using AutoMapper;
using EventsAplication.Api.Constants;
using EventsAplication.Presentation.Dto;
using EventsApplication.Presentation.Dto;
using EventsApplication.Application.Places.Commands.CreatePlace;
using EventsApplication.Application.Places.Commands.DeletePlace;
using EventsApplication.Application.Places.Commands.UpdatePlace;
using EventsApplication.Application.Places.Queries.GetAll;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApplication.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdatePlaceDto> _updatePlaceDtoValidator;
        private readonly IValidator<CreatePlaceCommand> _createPlaceCommandValidator;

        public PlaceController(
            IMediator mediator,
            IValidator<CreatePlaceCommand> createPlaceCommandValidator,
            IValidator<UpdatePlaceDto> updatePlaceDtoValidator)
        {
            _mediator = mediator;
            _createPlaceCommandValidator = createPlaceCommandValidator;
            _updatePlaceDtoValidator = updatePlaceDtoValidator;
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
            await _updatePlaceDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

            var command = new UpdatePlaceCommand(id, dto.Name);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePlaceAsync([FromBody] CreatePlaceCommand command, CancellationToken cancellationToken)
        {
            await _createPlaceCommandValidator.ValidateAndThrowAsync(command,cancellationToken);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
