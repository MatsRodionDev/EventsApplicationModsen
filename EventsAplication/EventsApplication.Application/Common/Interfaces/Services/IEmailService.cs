using EventsApplication.Application.Common.Dto;

namespace EventsApplication.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailDto request);
        Task InitializeAsync();
        Task Disconnect();
    }
}
