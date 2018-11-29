using System;

namespace NewsTrack.WebApi.Dtos
{
    public class Envelope<T> where T: class
    {
        public bool IsSuccessful { get; set; }
        public DateTime At { get; set; }
        public string ErrorMessage { get; set; }
        public T Payload { get; set; }

        public Envelope()
        {
            At = DateTime.UtcNow;
        }
    }
}