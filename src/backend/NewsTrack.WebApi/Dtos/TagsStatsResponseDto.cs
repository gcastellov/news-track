using System.Collections.Generic;

namespace NewsTrack.WebApi.Dtos
{
    public class TagsStatsResponseDto
    {
        public IEnumerable<TagsScoreDto> TagsScore { get; set; }
        public long MaxScore { get; set; }
        public double AverageScore { get; set; }
        public long Count { get; set; }
    }
}