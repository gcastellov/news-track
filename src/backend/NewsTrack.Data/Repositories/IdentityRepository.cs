using System;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Identity;
using NewsTrack.Identity.Repositories;

namespace NewsTrack.Data.Repositories
{
    public class IdentityRepository : RepositoryBase<Model.Identity, Identity.Identity>, IIdentityRepository
    {
        public override string IndexName => "news-identities";

        public IdentityRepository(IDataConfigurationProvider configurationProvider) 
            : base(configurationProvider)
        {
        }

        public async Task Save(Identity.Identity identity)
        {
            var client = GetClient();
            var model = From(identity);
            await client.IndexAsync(model, i => i.Index(IndexName));
        }

        public async Task Update(Identity.Identity identity)
        {
            var client = GetClient();
            var model = From(identity);
            await client.UpdateAsync(DocumentPath<Model.Identity>.Id(model.Id), 
                u => u.Doc(model)
                );
        }

        public async Task<Identity.Identity> Get(Guid id)
        {
            var client = GetClient();
            var model = await client.GetAsync<Model.Identity>(id);
            CheckResponse(model, id);
            return To(model.Source);
        }

        public async Task<Identity.Identity> GetByEmail(string email)
        {
            var request = new SearchRequest<Model.Identity>
            {
                Query = new TermQuery
                {
                    Field = "email",
                    Value = email
                }
            };

            var client = GetClient();
            var query = await client.SearchAsync<Model.Identity>(request);

            CheckResponse(query);
            return query.Documents.Count > 0 ? To(query.Documents.ElementAt(0)) : null;
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            var request = new SearchRequest<Model.Identity>
            {
                Query = new TermQuery
                {
                    Field = "email",
                    Value = email
                }
            };

            var client = GetClient();
            var query = await client.SearchAsync<Model.Identity>(request);

            CheckResponse(query);
            return query.Documents.Count > 0;
        }

        public async Task<bool> ExistsByUsername(string username)
        {
            var request = new SearchRequest<Model.Identity>
            {
                Query = new TermQuery
                {
                    Field = "username",
                    Value = username
                }
            };

            var client = GetClient();
            var query = await client.SearchAsync<Model.Identity>(request);
            return query.Documents.Count > 0;
        }

        public override async Task Initialize()
        {
            var client = GetClient();
            
            if (!await ExistIndex(client))
            {
                await client.Indices.CreateAsync(
                    IndexName, 
                    c => c.Map<Model.Identity>(descriptor => descriptor.AutoMap()));
            }
        }

        protected override Identity.Identity To(Model.Identity model)
        {
            return new Identity.Identity
            {
                Id = model.Id,
                AccessFailedCount = model.AccessFailedCount,
                CreatedAt = model.CreatedAt,
                Email = model.Email,
                IsEnabled = model.IsEnabled,
                LockoutEnd = model.LockoutEnd,
                LastAccessAt = model.LastAccessAt,
                LastAccessFailureAt = model.LastAccessFailureAt,
                Password = model.Password,
                SecurityStamp = model.SecurityStamp,
                Username = model.Username,
                IdType = (IdentityTypes)model.UserType
            };
        }

        protected override Model.Identity From(Identity.Identity entity)
        {
            return new Model.Identity
            {
                Id = entity.Id,
                AccessFailedCount = entity.AccessFailedCount,
                CreatedAt = entity.CreatedAt,
                Email = entity.Email,
                IsEnabled = entity.IsEnabled,
                LockoutEnd = entity.LockoutEnd,
                LastAccessAt = entity.LastAccessAt,
                LastAccessFailureAt = entity.LastAccessFailureAt,
                Password = entity.Password,
                SecurityStamp = entity.SecurityStamp,
                Username = entity.Username,
                UserType = (int)entity.IdType
            };
        }
    }
}