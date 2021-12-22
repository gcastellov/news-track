using System;
using System.Linq;
using System.Threading.Tasks;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Data.Repositories
{
    internal class DraftRelationshipRepository : RepositoryBase<Model.DraftRelationship, DraftRelationship>, IDraftRelationshipRepository
    {
        public override string IndexName => "news-draft-relationship";

        public DraftRelationshipRepository(IDataConfigurationProvider configurationProvider) 
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
                    c => c.Map<Model.DraftRelationship>(descriptor => descriptor.AutoMap()));
            }
        }

        public async Task SetRelationship(DraftRelationship relationship)
        {
            var client = GetClient();
            var model = From(relationship);
            await client.IndexDocumentAsync(model);
        }

        public async Task<DraftRelationship> Get(Guid id)
        {
            var client = GetClient();
            var query = await client.GetAsync<Model.DraftRelationship>(id);

            CheckResponse(query);
            return To(query.Source);
        }

        protected override DraftRelationship To(Model.DraftRelationship model)
        {
            if (model == null) return null;

            return new DraftRelationship
            {
                Id = model.Id,
                Relationship = model.Relationship?.Select(m => new DraftRelationshipItem
                {
                    Id = m.Id,
                    Title = m.Title,
                    Url = new Uri(m.Url, UriKind.Absolute)
                })
            };
        }

        protected override Model.DraftRelationship From(DraftRelationship entity)
        {
            return new Model.DraftRelationship
            {
                Id = entity.Id,
                Relationship = entity.Relationship?.Select(m => new Model.DraftRelationshipItem
                {
                    Id = m.Id,
                    Title = m.Title,
                    Url = m.Url.AbsoluteUri
                })
            };
        }
    }
}