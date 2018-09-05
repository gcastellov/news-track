using System;
using System.Collections.Generic;

namespace NewsTrack.WebApi.Dtos
{
    public class NewsDto
    {
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
        public string CreatedBy { get; set; }
    }
}