using System;
using System.Collections.Generic;

namespace NewsTrack.Domain.Entities
{
    public class Draft
    {
        private string _website;

        public Guid Id { get; set; }
        public Uri Uri { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Paragraphs { get; set; }
        public Uri Picture { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public long Views { get; set; }
        public long Fucks { get; set; }
        public long Related { get; set; }
        public User User { get; set; }

        public string Website
        {
            get
            {
                if (_website != null)
                {
                    return _website;
                }
                if (Uri != null)
                {
                    return Uri.Host;
                }

                return null;
            }
            set => _website = value;
        }

        public Draft()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;            
        }        
    }
}