using System.Threading.Tasks;

namespace NewsTrack.WebApi.Components
{
    public interface ISeeder
    {
        Task Initialize();
    }
}