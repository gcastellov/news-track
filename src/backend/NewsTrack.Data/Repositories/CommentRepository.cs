using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsTrack.Data.Repositories
{
    public class CommentRepository : RepositoryBase<Model.Comment, Comment>, ICommentRepository
    {
        public override string IndexName => "news-messages";

        public CommentRepository(IDataConfigurationProvider configurationProvider) 
            : base(configurationProvider)
        {
        }

        public override async Task Initialize()
        {
            var client = GetClient();

            if (!await ExistIndex(client))
            {
                await client.Indices.CreateAsync(
                    IndexName,
                    c => c.Map<Model.Comment>(descriptor => descriptor.AutoMap()));
            }
        }

        public async Task<IEnumerable<Comment>> GetByDraftId(Guid draftId, int page, int count)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Comment>(
                d => d.Sort(o => o.Descending(m => m.CreatedAt))
                    .From(page * count)
                    .Size(count)
                    .Query(m => m.Term(t => t.DraftId, draftId))
            );

            CheckResponse(query);
            return query.Documents.Select(To);
        }

        public async Task Save(Comment message)
        {
            var client = GetClient();
            var model = From(message);
            await client.IndexDocumentAsync(model);
        }

        public async Task<Comment> Get(Guid messageId)
        {
            var client = GetClient();
            var model = await client.GetAsync<Model.Comment>(messageId);
            CheckResponse(model, messageId);
            return To(model.Source);
        }

        public async Task<IEnumerable<Comment>> GetReplies(Guid messageId, int page, int count)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Comment>(
                d => d.Sort(o => o.Descending(m => m.CreatedAt))
                    .From(page * count)
                    .Size(count)
                    .Query(m => m.Term(t => t.ReplyingTo, messageId))
            );

            CheckResponse(query);
            return query.Documents.Select(To);
        }

        protected override Model.Comment From(Comment entity)
        {
            return new Model.Comment
            {
                Content = entity.Content,
                CreatedAt = entity.CreatedAt,
                CreatedBy = new Model.User
                {
                    Id = entity.CreatedBy.Id,
                    Username = entity.CreatedBy.Username
                },
                Likes = entity.Likes,
                ReplyingTo = entity.ReplyingTo,
                DraftId = entity.DraftId,
                Id = entity.Id,
            };
        }

        protected override Comment To(Model.Comment model)
        {
            return new Comment
            {
                Content = model.Content,
                CreatedAt = model.CreatedAt,
                CreatedBy = new User
                {
                    Id = model.CreatedBy.Id,
                    Username = model.CreatedBy.Username
                },
                Likes = model.Likes,
                ReplyingTo = model.ReplyingTo,
                DraftId = model.DraftId,
                Id = model.Id,
            };
        }        
    }
}
