﻿using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsTrack.Data.Repositories
{
    internal class CommentRepository : RepositoryBase<Model.Comment, Comment>, ICommentRepository
    {
        public override string IndexName => "news-comments";

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
                    .Query(query => query.Bool(b => b
                        .Must(m => m.Term(t => t.DraftId, draftId))
                        .MustNot(m => m.Exists(e => e.Field(f => f.ReplyingTo)))))

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

        public async Task<Comment> Get(Guid commentId)
        {
            var client = GetClient();
            var model = await client.GetAsync<Model.Comment>(commentId);
            CheckResponse(model, commentId);
            return To(model.Source);
        }

        public async Task<IEnumerable<Comment>> GetReplies(Guid commentId, int page, int count)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Comment>(
                d => d.Sort(o => o.Descending(m => m.CreatedAt))
                    .From(page * count)
                    .Size(count)
                    .Query(m => m.Term(t => t.ReplyingTo, commentId))
            );

            CheckResponse(query);
            return query.Documents.Select(To);
        }

        public async Task<long> AddReply(Guid commentId)
        {
            var client = GetClient();
            await client.UpdateAsync(DocumentPath<Model.Comment>.Id(commentId),
                    u => u.Script(s => s.Source("ctx._source.replies += 1")));

            var result = await client.GetAsync<Model.Comment>(
                commentId, 
                desc => desc.SourceIncludes(f => f.Replies));

            CheckResponse(result, commentId);
            return result.Source.Replies;
        }

        public async Task<long> AddLike(Guid commentId)
        {
            var client = GetClient();
            await client.UpdateAsync(DocumentPath<Model.Comment>.Id(commentId),
                    u => u.Script(s => s.Source("ctx._source.likes += 1")));

            var result = await client.GetAsync<Model.Comment>(
                commentId,
                desc => desc.SourceIncludes(f => f.Likes));

            CheckResponse(result, commentId);
            return result.Source.Likes;
        }

        public async Task<long> CountByDraftId(Guid draftId)
        {
            var client = GetClient();
            var query = await client.CountAsync<Model.Comment>(
                desc => desc.Query(m => m.Term(t => t.DraftId, draftId)));

            CheckResponse(query);
            return query.Count;
        }

        public async Task<long> CountByCommentId(Guid commentId)
        {
            var client = GetClient();
            var query = await client.CountAsync<Model.Comment>(
                desc => desc.Query(m => m.Term(t => t.ReplyingTo, commentId)));

            CheckResponse(query);
            return query.Count;
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
                Replies = entity.Replies,
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
                Replies = model.Replies,
                ReplyingTo = model.ReplyingTo,
                DraftId = model.DraftId,
                Id = model.Id,
            };
        }
    }
}
