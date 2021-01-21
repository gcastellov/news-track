using System;
using Nest;
using NewsTrack.Data.Configuration;

namespace NewsTrack.Data
{
    internal class ClientManager
    {
        private readonly IDataConfigurationProvider _configurationProvider;
        private static readonly object Locker = new object();
        private static ClientManager _manager;

        private ClientManager(IDataConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public ElasticClient GetClient<T>(string indexName) where T: class
        {
            var settings = new ConnectionSettings(_configurationProvider.ConnectionString)
                .DefaultIndex(indexName)
                .DefaultMappingFor<T>(i => i.IndexName(indexName));

            return new ElasticClient(settings);
        }

        public static ClientManager Create(IDataConfigurationProvider configurationProvider)
        {
            lock (Locker)
            {
                if (_manager == null)
                {
                    _manager = new ClientManager(configurationProvider);
                }
            }

            return _manager;
        }
    }
}