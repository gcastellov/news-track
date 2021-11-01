using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NewsTrack.WebApi.UnitTests
{
    [TestClass]
    public class ConfigurationProviderTests
    {
        private readonly Configuration.ConfigurationProvider _provider;

        public ConfigurationProviderTests()
        {
            _provider = new Configuration.ConfigurationProvider();
        }

        [TestMethod]
        public void GivenConfiguration_WhenParsing_ThenSetsProperSetting()
        {
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _provider.Set(appConfig);

            Assert.IsNotNull(_provider.SmtpConfiguration);
            Assert.IsNotNull(_provider.TokenConfiguration);
            Assert.IsNotNull(_provider.SuggestionsSchedule);
            Assert.AreNotEqual(_provider.ConnectionString, default);
            Assert.AreNotEqual(_provider.ApiUrl, default);
            Assert.AreNotEqual(_provider.SignInUrl, default);
        }
    }
}
