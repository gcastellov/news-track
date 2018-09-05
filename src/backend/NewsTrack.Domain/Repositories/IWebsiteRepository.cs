using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.Repositories
{
    public interface IWebsiteRepository
    {
        Task Save(IEnumerable<Website> uris);
        Task Clear();
        Task<bool> Exists(Uri uri);
    }
}