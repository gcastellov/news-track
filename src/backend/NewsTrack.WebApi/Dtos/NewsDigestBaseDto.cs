using System;

namespace NewsTrack.WebApi.Dtos
{
    public class NewsDigestBaseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }        
    }
}