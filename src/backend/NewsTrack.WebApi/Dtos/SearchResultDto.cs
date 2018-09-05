using System;

namespace NewsTrack.WebApi.Dtos
{
    public class SearchResultDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}