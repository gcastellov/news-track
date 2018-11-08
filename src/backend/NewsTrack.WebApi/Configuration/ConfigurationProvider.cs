using System;
using Microsoft.Extensions.Configuration;

namespace NewsTrack.WebApi.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private const string ApiUrlKey = "ApiUrl";
        private const string SignInUrlKey = "SignInUrl";

        public TokenConfiguration TokenConfiguration { get; private set; }
        public SmtpConfiguration SmtpConfiguration { get; private set; }
        public Uri ApiUrl { get; private set; }
        public Uri SignInUrl { get; private set; }

        internal void Set(IConfigurationRoot configuration)
        {
            TokenConfiguration = new TokenConfiguration().Set(configuration);
            SmtpConfiguration = new SmtpConfiguration().Set(configuration);
            if (Uri.TryCreate(configuration[ApiUrlKey], UriKind.Absolute, out var apiUrl))            
                ApiUrl = apiUrl;
            if (Uri.TryCreate(configuration[SignInUrlKey], UriKind.Absolute, out var signInUrl))
                SignInUrl = signInUrl;
        }
    }
}