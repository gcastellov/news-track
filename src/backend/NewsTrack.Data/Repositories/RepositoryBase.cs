using Nest;
using NewsTrack.Data.Configuration;
using NewsTrack.Data.Model;

namespace NewsTrack.Data.Repositories
{
    public abstract class RepositoryBase<T, TK> : IRepositoryBase where T: class, IDocument
        where TK: class
    {
        protected const int MaxQuerySize = 10000;
        private readonly IConfigurationProvider _configurationProvider;
        private static ClientManager ClientManager;

        public abstract string IndexName { get; }
        public abstract string TypeName { get; }

        protected RepositoryBase(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            if (ClientManager == null)
            {
                ClientManager = new ClientManager(_configurationProvider);
            }
        }

        public virtual void Initialize()
        {
            var client = GetClient();
            if (!client.IndexExists(IndexName).Exists)
            {
                client.CreateIndex(IndexName);
            }
        }

        protected ElasticClient GetClient()
        {
            return ClientManager.GetClient<T>(IndexName, TypeName);
        }

        protected abstract TK To(T model);
        protected abstract T From(TK entity);
    }
}