using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Data.Repositories
{
    public class WebsiteRepository : RepositoryBase<Model.Website, Website>, IWebsiteRepository
    {
        public override string IndexName => "news-website";
        public override string TypeName => "website";

        public WebsiteRepository(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }

        public async Task Save(IEnumerable<Website> websites)
        {
            if (websites == null)
            {
                throw new InvalidOperationException();
            }

            var client = GetClient();
            foreach (var website in websites)
            {
                await client.IndexAsync(website);
            }
        }

        public async Task Clear()
        {
            var client = GetClient();
            await client.DeleteByQueryAsync<Model.Website>(q => q.MatchAll());
        }

        public async Task<bool> Exists(Uri uri)
        {
            var request = new SearchRequest<Model.Website>
            {
                Query = new TermQuery
                {
                    Field = Infer.Field<Model.Website>(f => f.Uri),
                    Value = uri
                }
            };

            var client = GetClient();
            var query = await client.SearchAsync<Model.Website>(request);

            CheckResponse(query);
            return query.Documents.Count > 0;
        }

        public override void Initialize()
        {
            var client = GetClient();
            if (!client.IndexExists(IndexName).Exists)
            {
                client.CreateIndex(IndexName, c => c
                    .Mappings(ms => ms
                        .Map<Model.Website>(m => m.AutoMap())
                    )
                );
            }
        }

        protected override Website To(Model.Website model)
        {
            return new Website
            {
                Id = model.Id,
                Uri = model.Uri
            };
        }

        protected override Model.Website From(Website entity)
        {
            return new Model.Website
            {
                Id = entity.Id,
                Uri = entity.Uri
            };
        }
    }
}