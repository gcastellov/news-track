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
        Task<IEnumerable<Comment>> GetReplies(Guid messageId, int page, int count);
        Task Save(Comment message);
    }
}
