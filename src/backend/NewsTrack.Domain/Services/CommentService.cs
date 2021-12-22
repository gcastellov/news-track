using NewsTrack.Common.Validations;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace NewsTrack.Domain.Services
{
    internal class CommentService : ICommentService
    {
        private readonly IDraftRepository _draftRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentService(IDraftRepository draftRepository, ICommentRepository commentRepository)
        {
            _draftRepository = draftRepository;
            _commentRepository = commentRepository;
        }

        public async Task Save(Comment comment)
        {
            comment.Content.CheckIfNull(nameof(comment.Content));

            _ = await _draftRepository.Get(comment.DraftId);

            if (comment.ReplyingTo.HasValue)
            {
                var parent = await _commentRepository.Get(comment.ReplyingTo.Value);
                _ = await _commentRepository.AddReply(parent.Id);
            }

            await _commentRepository.Save(comment);
        }
    }
}
