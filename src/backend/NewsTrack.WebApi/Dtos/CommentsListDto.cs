using System.Collections.Generic;

namespace NewsTrack.WebApi.Dtos
{
    public class CommentsListDto
    {
        public IEnumerable<CommentDto> Comments { get; set; }
        public long Count { get; set; }
    }
}
