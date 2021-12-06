using System;

namespace NewsTrack.Domain.Entities
{
    public class Comment
    {
        public Comment()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid DraftId { get; set; }
        public Guid? ReplyingTo { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public uint Likes { get; set; }
        public uint Replies { get; set; }

        public static Comment Create(Guid draftId, string content, User createdBy)
        {
            return new Comment
            {
                DraftId = draftId,
                CreatedBy = createdBy,
                Content = content,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
