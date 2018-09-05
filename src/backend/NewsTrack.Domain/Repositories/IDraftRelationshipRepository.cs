using System;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.Repositories
{
    public interface IDraftRelationshipRepository
    {
        Task SetRelationship(DraftRelationship relationship);
        Task<DraftRelationship> Get(Guid id);
    }
}