using System.Threading.Tasks;

namespace NewsTrack.Domain.Services
{
    public interface IContentService
    {
        Task SetSuggestions();
    }
}