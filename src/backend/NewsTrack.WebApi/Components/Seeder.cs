using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Services;
using NewsTrack.Identity;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Results;
using NewsTrack.Identity.Services;

namespace NewsTrack.WebApi.Components
{
    public class Seeder : ISeeder
    {
        private const string Username = "Username";
        private const string Email = "Email";
        private const string Password = "Password";
        private const string AdminSection = "Admin";
        private const string WebsitesSection = "Websites";

        private readonly IDataInitializer _dataInitializer;
        private readonly IIdentityService _identityService;
        private readonly IWebsiteService _websiteService;
        private readonly IIdentityRepository _identityRepository;
        private readonly IConfigurationRoot _configuration;

        public Seeder(
            IDataInitializer dataInitializer, 
            IIdentityService identityService, 
            IWebsiteService websiteService,
            IIdentityRepository identityRepository,
            IConfigurationRoot configuration
            )
        {
            _dataInitializer = dataInitializer;            
            _identityService = identityService;
            _websiteService = websiteService;
            _identityRepository = identityRepository;
            _configuration = configuration;
        }

        public void Initialize()
        {
            _dataInitializer.Initialize();

            var adminSettings = _configuration.GetSection(AdminSection).GetChildren().ToArray();
            if (adminSettings.Any())
            {                
                SetAdmin(adminSettings);
            }

            var websitesSection = _configuration.GetSection(WebsitesSection).GetChildren().ToArray();
            if (websitesSection.Any())
            {
                SetWebsites(websitesSection);
            }
        }

        private void SetAdmin(IConfigurationSection[] settings)
        {
            var username = settings.FirstOrDefault(s => s.Key == Username)?.Value;
            var email = settings.FirstOrDefault(s => s.Key == Email)?.Value;
            var password = settings.FirstOrDefault(s => s.Key == Password)?.Value;

            if (!_identityRepository.ExistsByUsername(username).Result)
            {
                var result = _identityService.Save(username, email, password, password, IdentityTypes.Admin).Result;
                if (result != SaveIdentityResult.Ok)
                {
                    throw new Exception("Impossible to create the admin user. Check this out!");
                }
            }
        }

        private void SetWebsites(IConfigurationSection[] settings)
        {
            var uris = settings.Select(url => new Uri(url.Value, UriKind.Relative)).ToArray();
            _websiteService.Save(uris);
        }
    }
}