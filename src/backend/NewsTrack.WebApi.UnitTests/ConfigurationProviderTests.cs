using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;
using FluentAssertions;

namespace NewsTrack.WebApi.UnitTests
{
    
    public class ConfigurationProviderTests
    {
        private readonly Configuration.ConfigurationProvider _provider;

        public ConfigurationProviderTests()
        {
            _provider = new Configuration.ConfigurationProvider();
        }

        [Fact]
        public void GivenConfiguration_WhenParsing_ThenSetsProperSetting()
        {
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _provider.Set(appConfig);

            _provider.SmtpConfiguration.Should().NotBeNull();
            _provider.TokenConfiguration.Should().NotBeNull();
            _provider.SuggestionsSchedule.Should().NotBeNull();
            _provider.ConnectionString.Should().NotBe(default);
            _provider.ApiUrl.Should().NotBe(default);
            _provider.SignInUrl.Should().NotBe(default);
        }
    }
}
