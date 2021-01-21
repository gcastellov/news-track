using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Repositories;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Data.Repositories
{
    public class ContentRepository : RepositoryBase<Model.Content, Content>, IContentRepository
    {
        public override string IndexName => "news-content";
        
        public ContentRepository(IDataConfigurationProvider configurationProvider) 
            : base(configurationProvider)
        {
        }

        public async Task Save(Content content)
        {
            var client = GetClient();
            var model = From(content);
            await client.IndexDocumentAsync(model);
        }

        public async Task<IDictionary<string, IEnumerable<string>>> GetHighlights(IEnumerable<string> tags)
        {
            var client = GetClient();
            var query = await client.SearchAsync<Model.Content>(
                m => m
                    .From(0)
                    .Size(MaxQuerySize)
                    .Query(q => q                        
                        .Match(mf => mf
                            .Field(f => f.Body)
                            .Query(string.Join(",", tags))
                        )
                    )
                    .Highlight(
                        h => h.Fields(
                            fs => fs.Field(f => f.Body)
                                .Type("plain")
                                .FragmentSize(15)
                                .NumberOfFragments(150)
                                .Fragmenter(HighlighterFragmenter.Simple)
                            )
                    )
            );

            CheckResponse(query);
            return query.Hits.ToDictionary(
                h => h.Id,
                h => h.Highlight.Values.SelectMany(v => v)
            );
        }

        protected override Content To(Model.Content model)
        {
            return new Content
            {
                Id = model.Id,
                Body = model.Body
            };
        }

        protected override Model.Content From(Content entity)
        {
            return new Model.Content
            {
                Id = entity.Id,
                Body = entity.Body
            };
        }
    }
}