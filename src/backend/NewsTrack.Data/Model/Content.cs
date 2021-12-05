using System;

namespace NewsTrack.Data.Model
{
    public record Content : IDocument
    {
        public Guid Id { get; set; }
        public string Body { get; set; }        
    }
}