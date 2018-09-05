using System;
using System.Collections.Generic;

namespace NewsTrack.Browser.Dtos
{
    public class ResponseDto
    {
        public ResponseDto(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
        public IEnumerable<string> Titles { get; set; }
        public IEnumerable<string> Paragraphs { get; set; }
        public IEnumerable<Uri> Pictures { get; set; }
    }
}
