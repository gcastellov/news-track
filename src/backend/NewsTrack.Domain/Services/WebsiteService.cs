using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Domain.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IWebsiteRepository _websiteRepository;

        public WebsiteService(IWebsiteRepository websiteRepository)
        {
            _websiteRepository = websiteRepository;
        }

        public async Task Save(IEnumerable<Uri> uris)
        {
            if (uris == null)
            {
                throw new ArgumentNullException(nameof(uris));
            }

            var websites = uris.Select(u => new Website
            {
                Id = Guid.NewGuid(),
                Uri = u
            });

            await _websiteRepository.Clear().ContinueWith(t => _websiteRepository.Save(websites));
        }
    }
}