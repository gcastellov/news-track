using Microsoft.AspNetCore.Mvc;

namespace NewsTrack.WebApi.Controllers
{
    public class PingController : Controller
    {
        [Route("api/ping")]
        public string Get()
        {
            return "PONG";
        }
    }
}