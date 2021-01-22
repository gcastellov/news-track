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

        public WebsiteRepository(IDataConfigurationProvider configurationProvider) : base(configurationProvider)
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
                await client.IndexDocumentAsync(website);
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

        public override async Task Initialize()
        {
            var client = GetClient();
            
            if (!await ExistIndex(client))
            {
                await client.Indices.CreateAsync(
                    IndexName,
                    c => c.Map<Model.Website>(descriptor => descriptor.AutoMap()));
            }
        }

        protected override Website To(Model.Website model)
        {
            return new Website
            {
                Id = model.Id,
                Uri = new Uri(model.Uri, UriKind.Absolute)
            };
        }

        protected override Model.Website From(Website entity)
        {
            return new Model.Website
            {
                Id = entity.Id,
                Uri = entity.Uri.AbsoluteUri
            };
        }
    }
}