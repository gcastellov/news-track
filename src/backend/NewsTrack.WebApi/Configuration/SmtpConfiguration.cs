using System.Linq;
using Microsoft.Extensions.Configuration;

namespace NewsTrack.WebApi.Configuration
{
    public class SmtpConfiguration
    {
        public string From { get; private set; }
        public string Host { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Port { get; private set; }

        private const string Hostname = "Host";
        private const string User = "Username";
        private const string Sender = "From";
        private const string Pwd = "Password";
        private const string PortNumber = "Port";
        private const string SectionName = "Smtp";

        internal SmtpConfiguration Set(IConfigurationRoot configuration)
        {
            var section = configuration.GetSection(SectionName).GetChildren().ToArray();
            Host = section.FirstOrDefault(s => s.Key == Hostname)?.Value;
            Username = section.FirstOrDefault(s => s.Key == User)?.Value;
            From = section.FirstOrDefault(s => s.Key == Sender)?.Value;
            Password = section.FirstOrDefault(s => s.Key == Pwd)?.Value;
            Port = int.Parse(section.FirstOrDefault(s => s.Key == PortNumber)?.Value);
            return this;
        }

    }
}