using System;
using System.Linq;
using System.Threading.Tasks;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Data.Repositories
{
    public class DraftRelationshipRepository : RepositoryBase<Model.DraftRelationship, DraftRelationship>, IDraftRelationshipRepository
    {
        public override string IndexName => "news-draft-relationship";
        public override string TypeName => "draft-relationship";

        public DraftRelationshipRepository(IConfigurationProvider configurationProvider) 
            : base(configurationProvider)
        {
        }

        public async Task SetRelationship(DraftRelationship relationship)
        {
            var client = GetClient();
            var model = From(relationship);
            await client.IndexAsync(model);
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
                    Url = m.Url
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
                    Url = m.Url
                })
            };
        }
    }
}