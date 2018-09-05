using System;

namespace NewsTrack.WebApi.Dtos
{
    public class DraftResponseDto : ResponseBaseDto
    {
        public Guid Id { get; set; }
        public Uri Url { get; set; }
    }
}