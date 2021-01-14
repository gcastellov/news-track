using System.Threading.Tasks;

namespace NewsTrack.Data.Repositories
{
    public interface IRepositoryBase
    {
        Task Initialize();
    }
}