using AutoMapper;
using EventsAplication.Api.Constants;
using EventsAplication.Presentation.Dto;
using EventsApplication.Application.Common.Interfaces.Providers;
using EventsApplication.Application.Subscriptions.Commands.CreateSubscriptionForRegisteredUser;
using EventsApplication.Application.Subscriptions.Commands.DeleteSubscription;
using EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsOfRegisteredUser;
using EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsWithUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EventsAplication.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICustomClaimsKeysProvider _customClaimsKeysProvider;

        public SubscriptionController
            (IMediator mediator, 
            IMapper mapper,
            ICustomClaimsKeysProvider customClaimsKeysProvider)
        {
            _mediator = mediator;
            _mapper = mapper;
            _customClaimsKeysProvider = customClaimsKeysProvider;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventSubscriptions(Guid eventId,CancellationToken cancellationToken)
        {
            var query = new GetAllSubscriptionsWithUserQuery(eventId);

            var subscriptions = await _mediator.Send(query, cancellationToken);

            var subscriptionsRespons = _mapper.Map<List<SubscriptionWithUserResponse>>(subscriptions);

            return Ok(subscriptionsRespons);
        }

        [Authorize(Policy = Policies.Registered)]
        [HttpGet]
        public async Task<IActionResult> GetSubscriptionsOfUser(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(_customClaimsKeysProvider.UserId)!.Value);

            var query = new GetAllSubscriptionsOfRegisteredUserQuery(userId);

            var subscriptions = await _mediator.Send(query, cancellationToken);

            var subscriptionsRespons = _mapper.Map<List<SubscriptionWithEventResponse>>(subscriptions);

            return Ok(subscriptionsRespons);
        }

        [Authorize(Policy = Policies.User)]
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody]Guid eventId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(_customClaimsKeysProvider.UserId)!.Value);

            var command = new CreateSubscriptionForRegisteredUserCommand(userId, eventId);

            await _mediator.Send(command, cancellationToken);

            return Created();
        }

        [Authorize(Policy = Policies.User)]
        [HttpDelete("event/{id}")]
        public async Task<IActionResult> DeleteSubscription(Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(_customClaimsKeysProvider.UserId)!.Value);

            var command = new DeleteSubscriptionCommand(id, userId);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
