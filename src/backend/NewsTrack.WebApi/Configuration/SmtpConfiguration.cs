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
            var section = configuration.GetSection(SectionName);
            if (section != null)
            {
                Host = section.GetValue<string>(Hostname);
                Username = section.GetValue<string>(User);
                From = section.GetValue<string>(Sender);
                Password = section.GetValue<string>(Pwd);
                int.TryParse(section.GetValue<string>(PortNumber), out _port);
            }

            IsSet = Host.HasValue() && Username.HasValue() && From.HasValue() && Password.HasValue() && Port > 0;
            return this;
        }

    }
}