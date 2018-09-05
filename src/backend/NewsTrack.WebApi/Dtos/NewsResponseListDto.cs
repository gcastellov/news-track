using System.Collections.Generic;
using NewsTrack.Domain.Entities;

namespace NewsTrack.WebApi.Dtos
{
    public class NewsResponseListDto
    {
        public IEnumerable<NewsDto> News { get; set; }
        public long Count { get; set; }
    }
}