using System;

namespace NewsTrack.Data.Model
{
    public class Website : IDocument
    {
        public Guid Id { get; set; }
        
        public string Uri { get; set; }
    }
}