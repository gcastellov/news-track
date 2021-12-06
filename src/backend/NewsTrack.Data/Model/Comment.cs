using System;

namespace NewsTrack.Data.Model
{
    public record Comment : IDocument
    {
        public Guid Id { get; set; }
        public Guid DraftId { get; set; }
        public Guid? ReplyingTo { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public uint Likes { get; set; }
        public uint Replies { get; set; }
    }
}
