using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Services;

namespace NewsTrack.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ContentController : Controller
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }
        
        [HttpPost]
        [Route("suggestions")]
        public HttpResponseMessage SetSuggestions()
        {
            Task.Run(() => _contentService.SetSuggestions());
            return new HttpResponseMessage(HttpStatusCode.Accepted);            
        }
    }
}