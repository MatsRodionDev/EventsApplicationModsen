using EventsApplication.Application.Common.Dto;
using EventsApplication.Application.Common.Interfaces.Queues;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventsApplication.Infrastructure.BackgroundServices
{
    public class UpdatedEventEmailSender : BackgroundService
    {
        private readonly IEventUpdateQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UpdatedEventEmailSender> _logger;

        public UpdatedEventEmailSender(
            IEventUpdateQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<UpdatedEventEmailSender> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            { 
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var eventId = await _queue.DequeueAsync(stoppingToken);

                    var subsWithUsers = await unitOfWork.EventSubscriptionRepository.GetSubscriptionsWithUsersByEventIdAsync(eventId, stoppingToken);

                    var @event = await unitOfWork.EventRepository.GetByIdAsync(eventId, stoppingToken);

                    await emailService.InitializeAsync();

                    foreach (var sub in subsWithUsers)
                    {
                        var emailDto = new EmailDto
                        {
                            To = sub.User.Email,
                            Subject = $"Event {@event.Name} was updated",
                            Body = $"<div>Start of event: {@event.EventTime}</div><div>Event place: {@event.EventPlace.Name}</div>"
                        };

                        await emailService.SendEmail(emailDto);
                    }

                    await emailService.Disconnect();
                }
                catch(Exception ex) 
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
