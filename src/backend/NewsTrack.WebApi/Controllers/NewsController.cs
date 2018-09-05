using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Repositories;
using NewsTrack.WebApi.Dtos;
using System;
using AutoMapper;

namespace NewsTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
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
            if (count > 0)
            {
                var news = await _draftRepository.GetLatest((int)page, (int)count);
                var dtos = _mapper.Map<IEnumerable<NewsDto>>(news);

                var response = new NewsResponseListDto
                {
                    News = dtos,
                    Count = await _draftRepository.Count()
                };

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("mostviewed")]
        public async Task<IActionResult> GetMostViewed([FromQuery]uint page, [FromQuery]uint count)
        {
            if (count > 0)
            {
                var news = await _draftRepository.GetMostViewed((int)page, (int)count);
                var dtos = _mapper.Map<IEnumerable<NewsDto>>(news);

                var response = new NewsResponseListDto
                {
                    News = dtos,
                    Count = await _draftRepository.Count()
                };

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("mostfucked")]
        public async Task<IActionResult> GetMostFucked([FromQuery]uint page, [FromQuery]uint count)
        {
            if (count > 0)
            {
                var news = await _draftRepository.GetMostFucked((int)page, (int)count);
                var dtos = _mapper.Map<IEnumerable<NewsDto>>(news);

                var response = new NewsResponseListDto
                {
                    News = dtos,
                    Count = await _draftRepository.Count()
                };

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("entry/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _draftRepository.Get(id);
            var dto = _mapper.Map<NewsDto>(result);
            return Ok(dto);
        }

        [HttpGet]
        [Route("entry/{id}/relationship")]
        public async Task<IActionResult> GetRelationship(Guid id)
        {
            var result = await _draftRelationshipRepository.Get(id);
            var dtos = _mapper.Map<IEnumerable<NewsDigestDto>>(result?.Relationship);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("entry/{id}/suggestions")]
        public async Task<IActionResult> GetSuggestions(Guid id, [FromQuery]int take)
        {
            var result = await _draftSuggestionsRepository.Get(id);
            if (result?.Drafts != null && result.Drafts.Any())
            {
                result.Drafts = result.Drafts
                    .OrderByDescending(d => d.CreatedAt)
                    .Take(take);

                var results = result.Drafts.ToArray();
                foreach (var draft in results)
                {
                    var entry = await _draftRepository.Get(draft.Id);
                    draft.Uri = entry.Uri;
                    draft.Title = entry.Title;
                }

                result.Drafts = results;
            }

            var dto = _mapper.Map<DraftSuggestionsDto>(result);
            return Ok(dto);
        }

        [HttpGet]
        [Route("entry/{id}/suggestions/all")]
        public async Task<IActionResult> GetAllSuggestions(Guid id, [FromQuery] int take, [FromQuery] int skip)
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
                    .Skip(skip)
                    .Take(take)
                    .Select(d => d.Id)
                    .ToArray();
            }

            return Ok(dto);
        }

        [HttpGet]
        [Route("top/latest")]
        public async Task<IActionResult> GetLatest(int take)
        {
            var news = await _draftRepository.GetLatest(take);
            var dtos = _mapper.Map<IEnumerable<NewsDigestDto>>(news);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("top/viewed")]
        public async Task<IActionResult> GetMostViewed(int take)
        {
            var news = await _draftRepository.GetMostViewed(take);
            var dtos = _mapper.Map<IEnumerable<NewsDigestDto>>(news);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("top/fucking")]
        public async Task<IActionResult> GetMostFucking(int take)
        {
            var news = await _draftRepository.GetMostFucking(take);
            var dtos = _mapper.Map<IEnumerable<NewsDigestDto>>(news);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("top/websites")]
        public async Task<IEnumerable<WebsiteStatsDto>> GetWebsites(int take)
        {
            var websites = (await _draftRepository.GetWebsiteStats(take)).Select(w => new WebsiteStatsDto
            {
                Name = w.Key,
                Count = w.Value
            });

            return websites;
        }

        [HttpPatch]
        [Route("entry/{id}/visit")]
        public async Task<IActionResult> SetVisit(Guid id)
        {
            var result = await _draftRepository.AddViews(id);
            return Ok(new IncrementalResponseDto { Amount = result, IsSuccessful = true });
        }

        [HttpPatch]
        [Route("entry/{id}/fuck")]
        public async Task<IActionResult> SetFuck(Guid id)
        {
            var result = await _draftRepository.AddFuck(id);
            return Ok(new IncrementalResponseDto { Amount = result, IsSuccessful = true });
        }        

    }
}