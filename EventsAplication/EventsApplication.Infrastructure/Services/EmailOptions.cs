﻿
namespace EventsApplication.Infrastructure.Services
{
    public class EmailOptions
    {
        public string Host { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
