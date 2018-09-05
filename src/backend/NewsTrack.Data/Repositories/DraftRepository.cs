using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Data.Repositories
{
    public class DraftRepository : RepositoryBase<Model.Draft, Draft>, IDraftRepository
    {
        public override string IndexName => "news-drafts";
        public override string TypeName => "draft";

        private const string GroupByTag = "group_by_tag";
        private const string GroupByWebsite = "group_by_website";

        public DraftRepository(IConfigurationProvider configurationProvider)
            : base(configurationProvider)
        {
        }

        public async Task Save(Draft draft)
        {
            var client = GetClient();
            var model = From(draft);
            await client.IndexAsync(model);
        }

        public async Task<IEnumerable<Draft>> GetLatest(int page, int count)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.Sort(o => o.Descending(m => m.CreatedAt))
                    .From(page*count)
                    .Size(count)
                    .Query(m => m.MatchAll())
            );

            return query.Documents.Select(To);
        }

        public async Task<IEnumerable<Draft>> GetMostViewed(int page, int count)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.Sort(o => o.Descending(m => m.Views).Descending(m => m.CreatedAt))
                    .From(page * count)
                    .Size(count)
                    .Query(m => m.MatchAll())
            );

            return query.Documents.Select(To);
        }

        public async Task<IEnumerable<Draft>> GetMostFucked(int page, int count)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.Sort(o => o.Descending(m => m.Fucks).Descending(m => m.CreatedAt))
                    .From(page * count)
                    .Size(count)
                    .Query(m => m.MatchAll())
            );

            return query.Documents.Select(To);
        }

        public async Task<IEnumerable<Draft>> Get(string website, string pattern, IEnumerable<string> tags, int page, int count)
        {          
            var searchDescriptor = new SearchDescriptor<Model.Draft>()
                .Sort(o => o.Descending(m => m.CreatedAt))
                .From(page * count)
                .Size(count);

            var bQuery = GetSearchBoolQuery(website, pattern, tags);
            searchDescriptor.Query(q => q.Bool(b => bQuery));

            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(searchDescriptor);
            return query.Documents.Select(To);
        }

        public async Task<IEnumerable<Draft>> Get(IEnumerable<string> tags)
        {
            var bQuery = new BoolQuery
            {
                Should = tags.Select(t => (QueryContainer)new MatchQuery
                {
                    Field = Infer.Field<Model.Draft>(f => f.Tags),
                    Query = t
                })
            };

            var searchDescriptor = new SearchDescriptor<Model.Draft>()
                .Sort(o => o.Descending(m => m.CreatedAt))
                .From(0)
                .Size(MaxQuerySize)
                .Query(q => q.Bool(b => bQuery));

            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(searchDescriptor);
            return query.Documents.Select(To);
        }

        public async Task<IEnumerable<Draft>> GetLatest(int take)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.From(0)
                    .Size(take)
                    .Query(m => m.MatchAll())
                    .Sort(o => o.Descending(m => m.CreatedAt))
            );

            return query.Documents.Select(To);
        }

        public async Task<long> Count()
        {
            var client = GetClient();
            var query = await client.CountAsync<Model.Draft>();
            return query.Count;
        }

        public async Task<long> Count(string website, string pattern, IEnumerable<string> tags)
        {
            var countDescriptor = new CountDescriptor<Model.Draft>();
            var bQuery = GetSearchBoolQuery(website, pattern, tags);
            countDescriptor.Query(q => q.Bool(b => bQuery));

            var client = GetClient();
            var query = await client.CountAsync<Model.Draft>(countDescriptor);
            return query.Count;
        }

        public async Task<IEnumerable<string>> GetTags()
        {
            var query = await GetTagsStatsQuery();
            return ((BucketAggregate) query.Aggregations[GroupByTag]).Items
                .Cast<KeyedBucket<object>>()
                .Select(t => t.Key.ToString())
                .ToArray();
        }

        public async Task<IEnumerable<string>> GetTags(Guid id)
        {
            var client = GetClient();
            var query = await client.GetAsync<Model.Draft>(id, 
                m => m.SourceInclude(sf => sf.Tags)
                );

            return query.Source.Tags;
        }

        public async Task<IDictionary<string, long>> GetTagsStats()
        {
            var query = await GetTagsStatsQuery();
            return ((BucketAggregate)query.Aggregations[GroupByTag]).Items
                .Cast<KeyedBucket<object>>()
                .ToDictionary(
                    i => i.Key.ToString(),
                    i => i.DocCount ?? 0
                );
        }

        public async Task<IEnumerable<Draft>> Search(string pattern)
        {
            pattern = pattern?.ToLower();
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d
                    .From(0)
                    .Size(MaxQuerySize)
                    .Source(sf => sf.Includes(i => i.Field(f => f.Id).Field(f => f.Title)))                    
                    .Query(q => q.Wildcard(m => m.Field(f => f.Title).Value(pattern+"*").Strict(false)))                    
            );

            return query.Documents.Select(To);
        }

        public async Task<long> AddViews(Guid id)
        {
            var client = GetClient();
            await client.UpdateAsync(DocumentPath<Model.Draft>.Id(id),
                    u => u.Script(s => s.Inline("ctx._source.views += 1"))
                    .Fields(f => f.Views)
                    );

            var result = await client.GetAsync<Model.Draft>(id);
            return result.Source.Views;
        }

        public async Task<long> AddFuck(Guid id)
        {
            var client = GetClient();
            await client.UpdateAsync(DocumentPath<Model.Draft>.Id(id),
                u => u.Script(s => s.Inline("ctx._source.fucks += 1"))
                    .Fields(f => f.Fucks)
            );

            var result = await client.GetAsync<Model.Draft>(id);
            return result.Source.Fucks;
        }

        public async Task<Draft> Get(Guid id)
        {
            var client = GetClient();
            var query = await client.GetAsync<Model.Draft>(id);
            return To(query.Source);
        }

        public async Task SetRelationship(Guid id, long count)
        {
            var client = GetClient();
            await client.UpdateAsync(DocumentPath<Model.Draft>.Id(id),
                u => u.Script(s => s.Inline($"ctx._source.related = {count}"))
                    .Fields(f => f.Views)
            );
        }

        public async Task<IDictionary<string, long>> GetWebsiteStats(int take)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.Aggregations(
                    a => a.Terms(GroupByWebsite,
                        ags => ags.Field(new Field("website.keyword"))
                    )
                )
            );

            return ((BucketAggregate)query.Aggregations[GroupByWebsite]).Items
                .Cast<KeyedBucket<object>>()
                .ToDictionary(
                    i => i.Key.ToString(),
                    i => i.DocCount ?? 0
                );
        }

        public async Task<IEnumerable<Draft>> GetMostViewed(int take)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.From(0)
                    .Size(take)
                    .Query(m => m.MatchAll())
                    .Sort(o => o.Descending(m => m.Views))
            );

            return query.Documents.Select(To);
        }

        public async Task<IEnumerable<Draft>> GetMostFucking(int take)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.From(0)
                    .Size(take)
                    .Query(m => m.MatchAll())
                    .Sort(o => o.Descending(m => m.Fucks))
            );

            return query.Documents.Select(To);
        }

        protected override Draft To(Model.Draft model)
        {
            var entity = new Draft
            {
                Id = model.Id,
                Title = model.Title,
                CreatedAt = model.CreatedAt,
                Paragraphs = model.Paragraphs,
                Picture = model.Picture,
                Related = model.Related,
                Tags = model.Tags,
                Uri = model.Uri,
                Views = model.Views,
                Fucks = model.Fucks,
                Website = model.Website
            };

            //TODO: replace by object initialize
            if (model.User != null)
            {
                entity.User = new User
                {
                    Id = model.User.Id,
                    Username = model.User.Username
                };
            }

            return entity;
        }

        protected override Model.Draft From(Draft entity)
        {
            return new Model.Draft
            {
                Id = entity.Id,
                Title = entity.Title,
                CreatedAt = entity.CreatedAt,
                Paragraphs = entity.Paragraphs,
                Picture = entity.Picture,
                Related = entity.Related,
                Tags = entity.Tags,
                Uri = entity.Uri,
                Views = entity.Views,
                Fucks = entity.Fucks,
                Website = entity.Website,
                User = new Model.User
                {
                    Id = entity.User.Id,
                    Username = entity.User.Username
                }
            };
        }

        private async Task<ISearchResponse<Model.Draft>> GetTagsStatsQuery()
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Draft>(
                d => d.Aggregations(
                    a => a.Terms(GroupByTag,
                        ags => ags.Field(new Field("tags.keyword"))
                    )
                )
            );
            return query;
        }

        private BoolQuery GetSearchBoolQuery(string website, string pattern, IEnumerable<string> tags)
        {
            var bQuery = new BoolQuery();
            var queries = new List<QueryContainer>();
            if (!string.IsNullOrEmpty(pattern))
            {
                queries.Add(new MultiMatchQuery
                {
                    Query = pattern,
                    Type = TextQueryType.BestFields,
                    Fields = Infer.Field<Model.Draft>(f => f.Title).And(Infer.Field<Model.Draft>(f => f.Paragraphs))
                });
            }
            if (tags != null && tags.Any())
            {
                queries.AddRange(tags.Select(t => (QueryContainer)new MatchQuery
                {
                    Field = Infer.Field<Model.Draft>(f => f.Tags),
                    Query = t
                })
                .ToArray());
            }

            if (!string.IsNullOrEmpty(website))
            {
                queries.Add(new MatchQuery
                {
                    Field = Infer.Field<Model.Draft>(f => f.Website),
                    Query = website
                });
                
                bQuery.Must = queries;
            }
            else
            {
                bQuery.Should = queries;
            }

            return bQuery;
        }
    }
}