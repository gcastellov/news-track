using System;
using System.Collections.Generic;

namespace NewsTrack.Data.Model
{
    public class Draft : IDocument
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Paragraphs { get; set; }
        public string Picture { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public long Views { get; set; }
        public long Fucks { get; set; }
        public long Related { get; set; }
        public User User { get; set; }
        public string Website { get; set; }
    }
}