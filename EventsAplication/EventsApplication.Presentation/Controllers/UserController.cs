using AutoMapper;
using EventsAplication.Api.Constants;
using EventsAplication.Presentation.Dto;
using EventsApplication.Application.Common.Dto;
using EventsApplication.Application.Common.Interfaces.Providers;
using EventsApplication.Application.Users.Commands.Activate;
using EventsApplication.Application.Users.Commands.Login;
using EventsApplication.Application.Users.Commands.LogOut;
using EventsApplication.Application.Users.Commands.Refresh;
using EventsApplication.Application.Users.Commands.Register;
using EventsApplication.Application.Users.Commands.UpdateUser;
using EventsApplication.Application.Users.Queries.GetRegisteredUser;
using EventsApplication.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EventsAplication.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginCommand> _loginValidator;
        private readonly IValidator<RegisterCommand> _registerValidator;
        private readonly ICustomClaimsKeysProvider _customClaimsKeysProvider;
        private readonly IValidator<UpdateUserDto> _updateUserDtoValidator;

        public UserController(
            IMediator mediator,
            IMapper mapper,
            IValidator<LoginCommand> loginValidator,
            IValidator<RegisterCommand> registerValidator,
            ICustomClaimsKeysProvider customClaimsKeysProvider,
            IValidator<UpdateUserDto> updateUserDtoValidator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _customClaimsKeysProvider = customClaimsKeysProvider;
            _updateUserDtoValidator = updateUserDtoValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            await _loginValidator.ValidateAndThrowAsync(command, cancellationToken);

            var tokens = await _mediator.Send(command, cancellationToken);

            Response.Cookies.Append("access", tokens.AccesToken);
            Response.Cookies.Append("refresh", tokens.RefreshToken);

            return Ok();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            await _registerValidator.ValidateAndThrowAsync(command, cancellationToken);

            await _mediator.Send(command, cancellationToken);

            return Ok("Activated link was sended on your email");
        }

        [Authorize(Policy = Policies.Registered)]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(_customClaimsKeysProvider.UserId)!.Value);

            var command = new LogOutCommand(userId);

            await _mediator.Send(command, cancellationToken);

            Response.Cookies.Delete("access");
            Response.Cookies.Delete("refresh");

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refresh"]!;

            var command = new RefreshCommand(refreshToken);

            var tokens = await _mediator.Send(command, cancellationToken);

            Response.Cookies.Append("access", tokens.AccesToken);
            Response.Cookies.Append("refresh", tokens.RefreshToken);

            return Ok();
        }

        [Authorize(Policy = Policies.Registered)]
        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
        {
            await _updateUserDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

            var userId = Guid.Parse(User.FindFirst(_customClaimsKeysProvider.UserId)!.Value);

            var command = new UpdateUserCommand(userId, dto.FirstName, dto.LastName);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }


        [Authorize(Policy = Policies.Registered)]
        [HttpGet]
        public async Task<IActionResult> GetRegisteredUserAsync(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(_customClaimsKeysProvider.UserId)!.Value);

            var command = new GetRegisteredUserQuery(userId);

            var user = await _mediator.Send(command, cancellationToken);

            var userResponse = _mapper.Map<UserResponse>(user);

            return Ok(userResponse);
        }

        [HttpGet("activate/{id}")]
        public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateCommand(id);

            await _mediator.Send(command, cancellationToken);

            return Ok("Account was activated");
        }

    } 
}
