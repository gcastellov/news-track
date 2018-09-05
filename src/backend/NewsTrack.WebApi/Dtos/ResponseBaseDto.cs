using System;

namespace NewsTrack.WebApi.Dtos
{
    public class ResponseBaseDto
    {
        public bool IsSuccessful { get; set; }
        public DateTime At { get; set; }

        protected ResponseBaseDto()
        {
            At = DateTime.UtcNow;
        }
    }
}