using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace NewsTrack.WebApi.Configuration
{
    public class TokenConfiguration
    {
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public string Key { get; private set; }
        public SymmetricSecurityKey SigningKey { get; private set; }

        private const string TokenIssuer = "Issuer";
        private const string TokenAudience = "Audience";
        private const string TokenKey = "Key";
        private const string SectionName = "Tokens";

        internal TokenConfiguration Set(IConfigurationRoot configuration)
        {
            var section = configuration.GetSection(SectionName);
            Issuer = section.GetValue<string>(TokenIssuer);
            Audience = section.GetValue<string>(TokenAudience);
            Key = section.GetValue<string>(TokenKey);
            SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            return this;
        }
    }
}