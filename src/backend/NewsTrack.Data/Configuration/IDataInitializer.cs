using System.Threading.Tasks;

namespace NewsTrack.Data.Configuration
{
    public interface IDataInitializer
    {
        Task Initialize();
    }
}