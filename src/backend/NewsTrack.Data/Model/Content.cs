using System;

namespace NewsTrack.Data.Model
{
    public class Content : IDocument
    {
        public Guid Id { get; set; }
        public string Body { get; set; }        
    }
}