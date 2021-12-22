using System.Threading.Tasks;

namespace NewsTrack.WebApi.Components
{
    internal interface ISeeder
    {
        Task Initialize();
    }
}