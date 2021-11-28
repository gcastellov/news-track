using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Identity;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Results;
using NewsTrack.Identity.Services;
using NewsTrack.WebApi.Components;
using NewsTrack.WebApi.Configuration;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class IdentityController : BaseController
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
            return await Execute(async () =>
            {
                var id = _identityHelper.Id;
                var identity = await _identityRepository.Get(id);
                return _mapper.Map<IdentityDto>(identity);
            });
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
            var result = await _identityService.ChangePassword(
                id,
                dto.CurrentPassword,
                dto.Password,
                dto.ConfirmPassword);

            if (result == ChangePasswordResult.Ok)
            {
                return Ok(Dtos.Envelope.AsSuccess());
            }

            return Ok(Dtos.Envelope.AsFailure((uint)result));            
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = IdentityPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> Create([FromBody] CreateIdentityDto dto)
        {
            return await CreateNewAccount(dto, IdentityTypes.Contributor);
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] CreateIdentityDto dto)
        {
            return await CreateNewAccount(dto, IdentityTypes.Regular);
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

        private async Task<IActionResult> CreateNewAccount(CreateIdentityDto dto, IdentityTypes type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _identityService.Save(
                dto.Username,
                dto.Email,
                type);

            if (result.Type == SaveIdentityResult.ResultType.Ok)
            {
                return Ok(Dtos.Envelope.AsSuccess());
            }

            return Ok(Dtos.Envelope.AsFailure((uint)result.Type));
        }
    }
}