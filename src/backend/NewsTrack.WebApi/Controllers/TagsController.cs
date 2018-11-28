using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Repositories;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : BaseController
    {
        private readonly IDraftRepository _draftRepository;

        public TagsController(IDraftRepository draftRepository)
        {
            _draftRepository = draftRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Execute(async () => await _draftRepository.GetTags());
        }

        [HttpGet]
        [Route("stats")]
        public async Task<IActionResult> GetStats()
        {
            return await Execute(async () =>
            {
                var tags = await _draftRepository.GetTagsStats();
                if (tags.Any())
                {
                    return new TagsStatsResponseDto
                    {
                        TagsScore = tags.Select(t => new TagsScoreDto { Tag = t.Key, Score = t.Value }),
                        AverageScore = tags.Values.Average(),
                        MaxScore = tags.Values.Max(),
                        Count = tags.Count
                    };
                }

                return new TagsStatsResponseDto();
            });
        }
    }
}