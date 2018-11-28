using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Repositories;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : BaseController
    {
        private readonly IDraftRepository _draftRepository;
        private readonly IMapper _mapper;

        public SearchController(IDraftRepository draftRepository, IMapper mapper)
        {
            _draftRepository = draftRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string query)
        {
            return await Execute(async () =>
            {
                var drafts = await _draftRepository.Search(query);
                return _mapper.Map<IEnumerable<SearchResultDto>>(drafts);
            });
        }

        [HttpGet("advanced")]
        public async Task<IActionResult> Advanced(
            [FromQuery]string website,
            [FromQuery]string query, 
            [FromQuery]IEnumerable<string> tags, 
            [FromQuery]uint page,
            [FromQuery]uint count)
        {
            if (count > 0)
            {
                return await Execute(async () =>
                {
                    var labels = (tags ?? new string[0]).ToArray();
                    var news = await _draftRepository.Get(website, query, labels, (int)page, (int)count);
                    var newsDtos = _mapper.Map<IEnumerable<NewsDto>>(news);

                    return new NewsResponseListDto
                    {
                        News = newsDtos,
                        Count = await _draftRepository.Count(website, query, labels)
                    };
                });
            }

            return BadRequest();
        }     
    }
}