using System;
using System.Threading.Tasks;
using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Data.Model;
using NewsTrack.Domain.Exceptions;

namespace NewsTrack.Data.Repositories
{
    internal abstract class RepositoryBase<T, TK> : IRepositoryBase where T: class, IDocument
        where TK: class
    {
        protected const int MaxQuerySize = 10000;
        private readonly ClientManager _clientManager;

        public abstract string IndexName { get; }

        protected RepositoryBase(IDataConfigurationProvider configurationProvider)
        {
            _clientManager = ClientManager.Create(configurationProvider);
        }

        public virtual async Task Initialize()
        {
            var client = GetClient();
            if (!await ExistIndex(client))
            {
                await client.Indices.CreateAsync(IndexName);
            }
        }

        protected ElasticClient GetClient()
        {
            return _clientManager.GetClient<T>(IndexName);
        }

        protected async Task<bool> ExistIndex(ElasticClient client)
        {
            var existResponse = await client.Indices.ExistsAsync(IndexName);
            return existResponse.Exists;
        }

        protected void CheckResponse(IResponse response, Guid? id = null)
        {
            if (response.ApiCall.HttpStatusCode == 404)
                throw new NotFoundException(id);

            if (!response.IsValid)
                throw new ApplicationException("Impossible to get requested data", response.OriginalException);
        }

        protected abstract TK To(T model);
        protected abstract T From(TK entity);
    }
}