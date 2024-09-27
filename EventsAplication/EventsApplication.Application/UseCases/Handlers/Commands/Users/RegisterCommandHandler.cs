using AutoMapper;
using EventsApplication.Application.Common.Dto;
using EventsApplication.Application.Common.Interfaces.Hashers;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using EventsApplication.Application.UseCases.Commands.Users;


namespace EventsApplication.Application.UseCases.Handlers.Commands.Users
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            IEmailService emailService,
            IConfiguration configuration)
        {

            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserReporsitory.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is not null)
            {
                throw new BadRequestException("User with such email already exist");
            }

            var newUser = _mapper.Map<User>(request);

            newUser.PasswordHash = _passwordHasher.Generate(request.Password);

            Console.WriteLine(_passwordHasher.Verify(request.Password, newUser.PasswordHash));
            Console.WriteLine(newUser.PasswordHash);

            newUser.Id = Guid.NewGuid();

            newUser.IsActivated = false;

            var dto = new EmailDto
            {
                To = "rodion.mats11@gmail.com",
                Subject = "Activate account",
                Body = $"<div><a href=\"{_configuration["BaseAppUrl:BaseUrl"]}/api/User/activate/{newUser.Id}\">Activate</a></div>"
            };

            await _emailService.InitializeAsync();
            var isSended = await _emailService.SendEmail(dto);

            if (!isSended)
            {
                await _emailService.Disconnect();

                throw new BadRequestException("Incorrest email");
            }

            await _emailService.Disconnect();

            await _unitOfWork.UserReporsitory.AddAsync(newUser, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
