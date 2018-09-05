using System;
using Nest;
using NewsTrack.Data.Configuration;

namespace NewsTrack.Data
{
    internal class ClientManager
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ClientManager(IConfigurationProvider configurationProvider)
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
    }
}