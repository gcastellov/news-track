using System;
using System.ComponentModel.DataAnnotations;

namespace NewsTrack.WebApi.Dtos
{
    public class CreateCommentDto
    {
        [Required]
        public Guid DraftId { get; set; }

        public Guid? ReplyingTo { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Content { get; set; }
    }
}
