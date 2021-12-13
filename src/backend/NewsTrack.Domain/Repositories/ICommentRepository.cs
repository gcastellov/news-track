using NewsTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsTrack.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> Get(Guid messageId);
        Task<IEnumerable<Comment>> GetByDraftId(Guid draftId, int page, int count);
        Task<IEnumerable<Comment>> GetReplies(Guid commentId, int page, int count);
        Task<long> CountByDraftId(Guid draftId);
        Task<long> CountByCommentId(Guid commentId);
        Task<long> AddReply(Guid id);
        Task Save(Comment comment);
    }
}
