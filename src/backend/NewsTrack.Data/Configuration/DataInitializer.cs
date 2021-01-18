using System.Collections.Generic;
using System.Threading.Tasks;
using NewsTrack.Data.Repositories;

namespace NewsTrack.Data.Configuration
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IEnumerable<IRepositoryBase> _repositories;

        public DataInitializer(IEnumerable<IRepositoryBase> repositories)
        {
            _repositories = repositories;
        }

        public async Task Initialize()
        {
            if (_repositories != null)
            {
                foreach (var repository in _repositories)
                {
                    await repository.Initialize();
                }
            }
        }
    }
}