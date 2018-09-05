using System.Collections.Generic;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.Repositories
{
    public interface IContentRepository
    {
        Task Save(Content content);
        Task<IDictionary<string, IEnumerable<string>>> GetHighlights(IEnumerable<string> tags);
    }
}