using EventsApplication.Application.Common.Dto;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Infrastructure.BackgroundServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EventsApplication.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly EmailOptions _options;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailOptions> options,
            ILogger<EmailService> logger) 
        {
            _smtpClient = new SmtpClient();
            _options = options.Value;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            await _smtpClient.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.SslOnConnect, default);
            await _smtpClient.AuthenticateAsync(_options.From, _options.Password, default);
        }

        public async Task<bool> SendEmail(EmailDto request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_options.From));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                
                await _smtpClient.SendAsync(email, default);

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task Disconnect()
        {
            await _smtpClient.DisconnectAsync(true);
            _smtpClient.Dispose();
        }
    }
}
