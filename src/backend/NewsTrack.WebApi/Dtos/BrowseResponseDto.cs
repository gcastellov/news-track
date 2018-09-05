using System;
using System.Collections.Generic;

namespace NewsTrack.WebApi.Dtos
{
    public class BrowseResponseDto
    {
        public Uri Uri { get; set; }
        public IEnumerable<string> Titles { get; set; }
        public IEnumerable<string> Paragraphs { get; set; }
        public IEnumerable<Uri> Pictures { get; set; }
    }
}