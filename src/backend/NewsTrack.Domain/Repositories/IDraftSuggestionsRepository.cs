using System;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.Repositories
{
    public interface IDraftSuggestionsRepository
    {
        Task Save(DraftSuggestions entity);
        Task<DraftSuggestions> Get(Guid id);
    }
}