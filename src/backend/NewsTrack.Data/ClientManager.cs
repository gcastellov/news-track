using System;
using Nest;
using NewsTrack.Data.Configuration;

namespace NewsTrack.Data
{
    internal class ClientManager
    {
        private readonly IConfigurationProvider _configurationProvider;
        private static readonly object Locker = new object();
        private static ClientManager _manager;

        private ClientManager(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public ElasticClient GetClient<T>(string indexName, string typeName) where T: class
        {
            var node = new Uri(_configurationProvider.ConnectionString);
            var settings = new ConnectionSettings(node)
                .DefaultIndex(indexName)
                .InferMappingFor<T>(i => i
                    .IndexName(indexName)
                    .TypeName(typeName)                    
                );

            return new ElasticClient(settings);
        }

        public static ClientManager Create(IConfigurationProvider configurationProvider)
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