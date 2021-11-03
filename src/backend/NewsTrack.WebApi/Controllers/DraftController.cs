using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Browser;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Services;
using NewsTrack.WebApi.Components;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DraftController : BaseController
    {
        private readonly IDraftService _draftService;
        private readonly IBroswer _broswer;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public DraftController(
            IDraftService draftService, 
            IBroswer broswer, 
            IIdentityHelper identityHelper, 
            IMapper mapper)
        {
            _draftService = draftService;
            _broswer = broswer;
            _identityHelper = identityHelper;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DraftRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return await Execute(async () =>
                {
                    var content = await _broswer.GetContent(request.Url);

                    var draft = new Draft
                    {
                        Uri = new Uri(request.Url),
                        Picture = new Uri(request.Picture),
                        Title = request.Title,
                        Paragraphs = request.Paragraphs,
                        Tags = request.Tags,
                        User = new User
                        {
                            Id = _identityHelper.Id,
                            Username = _identityHelper.Username
                        }
                    };

                    await _draftService.Save(draft, content);
                    return _mapper.Map<DraftResponseDto>(draft);
                });
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}/relationship")]
        public async Task<IActionResult> SetRelationship(Guid id, [FromBody] IEnumerable<NewsDigestBaseDto> relationship)
        {
            if (ModelState.IsValid)
            {
                return await Execute(async () =>
                {
                    var items = relationship.ToArray();
                    if (items.Length > 0)
                    {
                        await _draftService.SetRelationships(id, items.Select(i => new DraftRelationshipItem
                        {
                            Id = i.Id,
                            Title = i.Title,
                            Url = i.Url
                        }));                        
                    }

                    return new DraftRelationshipResponseDto { Id = id };
                });
            }

            return BadRequest();
        }        
    }
}