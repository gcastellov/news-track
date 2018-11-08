using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Identity;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Services;
using NewsTrack.WebApi.Components;
using NewsTrack.WebApi.Configuration;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        private readonly IIdentityHelper _identityHelper;
        private readonly IIdentityRepository _identityRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public IdentityController(
            IIdentityHelper identityHelper, 
            IIdentityRepository identityRepository,
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityHelper = identityHelper;
            _identityRepository = identityRepository;
            _identityService = identityService;
            _mapper = mapper;            
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var id = _identityHelper.Id;
            var identity = await _identityRepository.Get(id);
            var dto = _mapper.Map<IdentityDto>(identity);
            dto.IsSuccessful = true;
            return Ok(dto);
        }

        [HttpPost]
        [Route("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var id = _identityHelper.Id;
            var result = await _identityService.ChangePassword(id, dto.CurrentPassword, dto.Password, dto.ConfirmPassword);
            var response = ChangePasswordResponseDto.Create(result);
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = IdentityPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> Create([FromBody] CreateIdentityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _identityService.Save(
                dto.Username,
                dto.Email,
                dto.Password,
                dto.ConfirmPassword,
                IdentityTypes.Contributor
            );

            var response = CreateIdentityResponseDto.Create(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("confirm/{email}/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm(string email, string id, [FromQuery] string go)
        {
            if (!await _identityService.Confirm(email, id))
            {
                return BadRequest();
            }

            return Redirect(go);
        }
    }
}