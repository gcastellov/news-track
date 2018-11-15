using System.Linq;
using Microsoft.Extensions.Configuration;
using NewsTrack.Common.Validations;

namespace NewsTrack.WebApi.Configuration
{
    public class SmtpConfiguration
    {
        private int _port;

        public string From { get; private set; }
        public string Host { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Port => _port;
        public bool IsSet { get; private set; }

        private const string Hostname = "Host";
        private const string User = "Username";
        private const string Sender = "From";
        private const string Pwd = "Password";
        private const string PortNumber = "Port";
        private const string SectionName = "Smtp";

        internal SmtpConfiguration Set(IConfigurationRoot configuration)
        {
            var config = configuration.GetSection(SectionName);
            if (config != null)
            {
                var section = config.GetChildren().ToArray();
                Host = section.FirstOrDefault(s => s.Key == Hostname)?.Value;
                Username = section.FirstOrDefault(s => s.Key == User)?.Value;
                From = section.FirstOrDefault(s => s.Key == Sender)?.Value;
                Password = section.FirstOrDefault(s => s.Key == Pwd)?.Value;
                int.TryParse(section.FirstOrDefault(s => s.Key == PortNumber)?.Value, out _port);
            }

            IsSet = Host.HasValue() && Username.HasValue() && From.HasValue() && Password.HasValue() && Port > 0;
            return this;
        }

    }
}