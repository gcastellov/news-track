using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.Services
{
    public interface IDraftService
    {
        Task Save(Draft draft, string body);
        Task SetRelationships(Guid id, IEnumerable<DraftRelationshipItem> items);
    }
}