using System;

namespace NewsTrack.WebApi.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid DraftId { get; set; }
        public Guid? ReplyingTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public uint Likes { get; set; }
        public uint Replies { get; set; }
    }
}
