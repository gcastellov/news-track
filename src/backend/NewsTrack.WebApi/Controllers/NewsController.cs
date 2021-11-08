using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Repositories;
using NewsTrack.WebApi.Dtos;
using System;
using AutoMapper;
using NewsTrack.Domain.Entities;

namespace NewsTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : BaseController
    {
        private readonly IDraftRepository _draftRepository;
        private readonly IDraftRelationshipRepository _draftRelationshipRepository;
        private readonly IDraftSuggestionsRepository _draftSuggestionsRepository;
        private readonly IMapper _mapper;

        public NewsController(
            IDraftRepository draftRepository, 
            IDraftRelationshipRepository draftRelationshipRepository, 
            IDraftSuggestionsRepository draftSuggestionsRepository, 
            IMapper mapper)
        {
            _draftRepository = draftRepository;
            _draftRelationshipRepository = draftRelationshipRepository;
            _draftSuggestionsRepository = draftSuggestionsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("latest")]
        public async Task<IActionResult> GetLatest([FromQuery]uint page, [FromQuery]uint count)
        {
            return await GetNews(
                _draftRepository.GetLatest,
                page,
                count
            );
        }

        [HttpGet]
        [Route("mostviewed")]
        public async Task<IActionResult> GetMostViewed([FromQuery]uint page, [FromQuery]uint count)
        {
            return await GetNews(
                _draftRepository.GetMostViewed,
                page,
                count
            );
        }

        [HttpGet]
        [Route("mostfucked")]
        public async Task<IActionResult> GetMostFucked([FromQuery]uint page, [FromQuery]uint count)
        {
            return await GetNews(
                _draftRepository.GetMostFucked,
                page,
                count
            );
        }

        [HttpGet]
        [Route("entry/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await Execute(async () =>
            {
                var result = await _draftRepository.Get(id);
                return _mapper.Map<NewsDto>(result);
            });
        }

        [HttpGet]
        [Route("entry/{id}/relationship")]
        public async Task<IActionResult> GetRelationship(Guid id)
        {
            return await Execute(async () =>
            {
                var result = await _draftRelationshipRepository.Get(id);
                return _mapper.Map<IEnumerable<NewsDigestDto>>(result?.Relationship);
            });
        }

        [HttpGet]
        [Route("entry/{id}/suggestions")]
        public async Task<IActionResult> GetSuggestions(Guid id, [FromQuery]uint take)
        {
            if (take == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                var result = await _draftSuggestionsRepository.Get(id);
                if (result?.Drafts != null && result.Drafts.Any())
                {
                    result.Drafts = result.Drafts
                        .OrderByDescending(d => d.CreatedAt)
                        .Take((int)take);

                    var results = result.Drafts.ToArray();
                    foreach (var draft in results)
                    {
                        var entry = await _draftRepository.Get(draft.Id);
                        draft.Uri = entry.Uri;
                        draft.Title = entry.Title;
                    }

                    result.Drafts = results;
                }

                return _mapper.Map<DraftSuggestionsDto>(result);
            });
        }

        [HttpGet]
        [Route("entry/{id}/suggestions/all")]
        public async Task<IActionResult> GetAllSuggestions(Guid id, [FromQuery] uint take, [FromQuery] uint skip)
        {
            return await Execute(async () =>
            {
                var result = await _draftSuggestionsRepository.Get(id);
                var dto = new DraftSuggestionsIdsDto
                {
                    Id = id,
                    Count = 0
                };

                if (result?.Drafts != null && result.Drafts.Any())
                {
                    dto.Count = result.Drafts.Count();
                    dto.SuggestedIds = result.Drafts
                        .OrderByDescending(d => d.CreatedAt)
                        .Skip((int)skip)
                        .Take((int)take)
                        .Select(d => d.Id)
                        .ToArray();
                }

                return dto;
            });
        }

        [HttpGet]
        [Route("top/latest")]
        public async Task<IActionResult> GetLatest(uint take)
        {
            if (take == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                var news = await _draftRepository.GetLatest((int)take);
                return _mapper.Map<IEnumerable<NewsDigestDto>>(news);
            });
        }

        [HttpGet]
        [Route("top/viewed")]
        public async Task<IActionResult> GetMostViewed(uint take)
        {
            if (take == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                var news = await _draftRepository.GetMostViewed((int)take);
                return _mapper.Map<IEnumerable<NewsDigestDto>>(news);
            });
        }

        [HttpGet]
        [Route("top/fucking")]
        public async Task<IActionResult> GetMostFucking(uint take)
        {
            if (take == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                var news = await _draftRepository.GetMostFucking((int)take);
                return _mapper.Map<IEnumerable<NewsDigestDto>>(news);
            });
        }

        [HttpGet]
        [Route("top/websites")]
        public async Task<IActionResult> GetWebsites(uint take)
        {
            if (take == 0)
            {
                return BadRequest();
            }

            return await Execute(async () =>
            {
                return (await _draftRepository.GetWebsiteStats((int)take)).Select(w => new WebsiteStatsDto
                {
                    Name = w.Key,
                    Count = w.Value
                });
            });
        }

        [HttpPatch]
        [Route("entry/{id}/visit")]
        public async Task<IActionResult> SetVisit(Guid id)
        {
            return await Execute(async () =>
            {
                var result = await _draftRepository.AddViews(id);
                return new IncrementalResponseDto {Amount = result};
            });
        }

        [HttpPatch]
        [Route("entry/{id}/fuck")]
        public async Task<IActionResult> SetFuck(Guid id)
        {
            return await Execute(async () =>
            {
                var result = await _draftRepository.AddFuck(id);
                return new IncrementalResponseDto {Amount = result};
            });
        }

        private async Task<IActionResult> GetNews(Func<int, int, Task<IEnumerable<Draft>>> getDraftsFunc, uint page, uint count)
        {
            if (count > 0)
            {
                return await Execute(async () =>
                {
                    var news = await getDraftsFunc((int)page, (int)count);
                    var dtos = _mapper.Map<IEnumerable<NewsDto>>(news);
                    return new NewsResponseListDto
                    {
                        News = dtos,
                        Count = await _draftRepository.Count()
                    };
                });
            }

            return BadRequest();
        }

    }
}