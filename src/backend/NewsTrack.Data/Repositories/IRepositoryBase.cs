using System.Threading.Tasks;

namespace NewsTrack.Data.Repositories
{
    internal interface IRepositoryBase
    {
        Task Initialize();
    }
}