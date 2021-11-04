using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Browser;
using NewsTrack.Common.Validations;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BrowserController : BaseController
    {
        private readonly IBroswer _broswer;
        private readonly IMapper _mapper;

        public BrowserController(IBroswer broswer, IMapper mapper)
        {
            _broswer = broswer;
            _mapper = mapper;
        }

        [HttpGet("browse")]
        public async Task<IActionResult> Get(Uri url)
        {
            try
            {
                url.CheckIfNull(nameof(url));

                if (!url.IsAbsoluteUri)
                {
                    return BadRequest();
                }

                return await Execute(async () =>
                {
                    var response = await _broswer.Get(url.AbsoluteUri);
                    return _mapper.Map<BrowseResponseDto>(response);
                });
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }
    }
}