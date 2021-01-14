using System;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Data.Repositories
{
    public class DraftSuggestionsRepository : RepositoryBase<Model.DraftSuggestions, DraftSuggestions>, IDraftSuggestionsRepository
    {
        public override string IndexName => "news-suggestions";

        public DraftSuggestionsRepository(IConfigurationProvider configurationProvider) 
            : base(configurationProvider)
        {
        }

        public async Task Save(DraftSuggestions entity)
        {
            var client = GetClient();
            var model = From(entity);
            await client.UpdateAsync(DocumentPath<Model.DraftSuggestions>.Id(entity.Id),
                u => u
                .DocAsUpsert()
                .Doc(model)
                .Upsert(model)
            );
        }

        public async Task<DraftSuggestions> Get(Guid id)
        {
            var client = GetClient();
            var model = await client.GetAsync(DocumentPath<Model.DraftSuggestions>.Id(id));

            CheckResponse(model);
            return To(model.Source);
        }

        protected override DraftSuggestions To(Model.DraftSuggestions model)
        {
            if (model == null) return null;

            return new DraftSuggestions
            {
                Id = model.Id,
                Tags = model.Tags,
                Drafts = model.Drafts?.Select(d => new Draft
                {
                    Id = d.Id,
                    CreatedAt = d.CreatedAt
                })
            };
        }

        protected override Model.DraftSuggestions From(DraftSuggestions entity)
        {
            return new Model.DraftSuggestions
            {
                Id = entity.Id,
                Tags = entity.Tags,
                Drafts = entity.Drafts?.Select(d => new Model.DraftReference
                {
                    Id = d.Id,
                    CreatedAt = d.CreatedAt
                })
            };
        }
    }
}