using System;
using System.Threading.Tasks;

namespace NewsTrack.Identity.Repositories
{
    public interface IIdentityRepository
    {
        Task Save(Identity identity);
        Task Update(Identity identity);
        Task<Identity> Get(Guid id);
        Task<Identity> GetByEmail(string email);
        Task<bool> ExistsByEmail(string email);
        Task<bool> ExistsByUsername(string username);
    }
}