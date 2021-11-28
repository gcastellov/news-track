using System;

namespace NewsTrack.WebApi.Dtos
{
    public class Envelope<T> : Envelope
        where T: class
    {
        public T Payload { get; set; }
    }

    public class Envelope
    {
        public bool IsSuccessful { get; set; }
        public DateTime At { get; set; }
        public Error Error { get; set; }
        
        public Envelope()
        {
            At = DateTime.UtcNow;
        }

        public static Envelope AsFailure(uint errorCode) => Envelope.AsFailure(null, errorCode);

        public static Envelope AsFailure(string errorMessage, uint errorCode = 0)
        {
            return new Envelope
            {
                IsSuccessful = false,
                Error = new Error
                {
                    Code = errorCode,
                    Message = errorMessage
                }
            };
        }

        public static Envelope AsSuccess() => new Envelope { IsSuccessful = true };
    }
}