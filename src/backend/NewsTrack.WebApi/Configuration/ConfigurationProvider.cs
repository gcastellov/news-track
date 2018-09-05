using Microsoft.Extensions.Configuration;

namespace NewsTrack.WebApi.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public TokenConfiguration TokenConfiguration { get; private set; }
        public SmtpConfiguration SmtpConfiguration { get; private set; }

        internal void Set(IConfigurationRoot configuration)
        {
            TokenConfiguration = new TokenConfiguration().Set(configuration);
            SmtpConfiguration = new SmtpConfiguration().Set(configuration);
        }
    }
}