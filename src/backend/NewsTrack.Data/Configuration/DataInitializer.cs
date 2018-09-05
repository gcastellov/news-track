using System.Collections.Generic;
using NewsTrack.Data.Repositories;

namespace NewsTrack.Data.Configuration
{
    public class DataInitializer : IDataInitializer
    {
        private IEnumerable<IRepositoryBase> _repositories;

        public DataInitializer(IEnumerable<IRepositoryBase> repositories)
        {
            _repositories = repositories;
        }

        public void Initialize()
        {
            if (_repositories != null)
            {
                foreach (var repository in _repositories)
                {
                    repository.Initialize();
                }
            }
        }
    }
}