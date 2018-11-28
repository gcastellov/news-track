using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Repositories;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WebsiteController : BaseController
    {
        private readonly IWebsiteRepository _websiteRepository;

        public WebsiteController(IWebsiteRepository websiteRepository)
        {
            _websiteRepository = websiteRepository;
        }

        [HttpGet]
        [Route("check")]
        public async Task<IActionResult> Check(Uri uri)
        {
            return await Execute(async () =>
            {
                var baseUri = new Uri(uri.Host, UriKind.Relative);
                var isForbidden = await _websiteRepository.Exists(baseUri);
                return new WebsiteDto
                {
                    IsSuccessful = !isForbidden,
                    Uri = uri
                };
            });
        }
    }
}