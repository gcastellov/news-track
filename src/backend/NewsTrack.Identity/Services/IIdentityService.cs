using System;
using System.Threading.Tasks;
using NewsTrack.Identity.Results;

namespace NewsTrack.Identity.Services
{
    public interface IIdentityService
    {
        Task<SaveIdentityResult> Save(string username, string email, string password1, string password2);
        Task<AuthenticateResult> Authenticate(string email, string password);
        Task<bool> Confirm(string email, string securityStamp);

        Task<ChangePasswordResult> ChangePassword(
            Guid id,
            string currentPassword,
            string password1,
            string password2);
    }
}