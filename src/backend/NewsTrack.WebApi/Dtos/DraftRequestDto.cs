using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsTrack.WebApi.Dtos
{
    public class DraftRequestDto
    {
        [Url]
        [Required]
        public string Url { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public IEnumerable<string> Paragraphs { get; set; }

        [Url]
        [Required]
        public string Picture { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
