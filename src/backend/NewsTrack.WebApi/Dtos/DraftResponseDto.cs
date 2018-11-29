using System;

namespace NewsTrack.WebApi.Dtos
{
    public class DraftResponseDto
    {
        public Guid Id { get; set; }
        public Uri Url { get; set; }
    }
}