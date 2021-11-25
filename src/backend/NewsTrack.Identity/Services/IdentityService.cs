using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using NewsTrack.Common.Validations;
using NewsTrack.Identity.Encryption;
using NewsTrack.Identity.Events;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Results;
using static NewsTrack.Identity.Results.SaveIdentityResult.ResultType;

namespace NewsTrack.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private const string EmailPattern =
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        private readonly IIdentityRepository _identityRepository;
        private readonly ICryptoManager _cryptoManager;
        private readonly IMediator _mediator;

        public IdentityService(
            IIdentityRepository identityRepository,
            ICryptoManager cryptoManager,
            IMediator mediator)
        {
            _identityRepository = identityRepository;
            _cryptoManager = cryptoManager;
            _mediator = mediator;
        }

        public async Task<SaveIdentityResult> Save(string username, string email, IdentityTypes type)
        {
            string password = Guid.NewGuid().ToString("N").Substring(0, 8);
            var result = await Create(username, email, password, password, type);
            
            if (result.Type == Ok)
            {
                await _mediator.Publish(AccountCreated.From(result.Identity, password));
            }

            return result;
        }

        public async Task<SaveIdentityResult> Save(string username, string email, string password1, string password2, IdentityTypes type)
        {
            var result = await Create(username, email, password1, password2, type);
            if (result.Type == Ok)
            {
                await _mediator.Publish(AccountCreated.From(result.Identity));
            }

            return result;
        }

        public async Task<AuthenticateResult> Authenticate(string email, string password)
        {
            email.CheckIfNull(nameof(email));
            password.CheckIfNull(nameof(password));

            var identity = await _identityRepository.GetByEmail(email);

            if (identity == null || !identity.IsEnabled)
            {
                return AuthenticateResult.Failed;
            }
            if (identity.LockoutEnd > DateTime.UtcNow)
            {
                return AuthenticateResult.Lockout;
            }
            if (!_cryptoManager.CheckPassword(password, identity.Password))
            {
                var status = AuthenticateResult.Failed;
                identity.AccessFailedCount++;
                identity.LastAccessFailureAt = DateTime.UtcNow;
                if (identity.AccessFailedCount > 5)
                {                    
                    identity.LockoutEnd = DateTime.UtcNow.AddMinutes(5);
                    status = AuthenticateResult.Lockout;
                    await _mediator.Publish(AccountLocked.From(identity));
                }

                await _identityRepository.Update(identity);
                return status;
            }

            if (identity.AccessFailedCount > 0)
            {
                identity.AccessFailedCount = 0;
                identity.LockoutEnd = null;
            }

            identity.LastAccessAt = DateTime.UtcNow;
            await _identityRepository.Update(identity);
            return AuthenticateResult.Ok;
        }

        public async Task<bool> Confirm(string email, string securityStamp)
        {
            var identity = await _identityRepository.GetByEmail(email);
            if (identity?.IsEnabled == false && identity.SecurityStamp == securityStamp)
            {
                await _mediator.Publish(AccountConfirmed.From(identity));
                identity.IsEnabled = true;
                await _identityRepository.Update(identity);
                return true;
            }

            return false;
        }

        public async Task<ChangePasswordResult> ChangePassword(
            Guid id, 
            string currentPassword, 
            string password1,
            string password2)
        {
            var identity = await _identityRepository.Get(id);
            if (password1 != password2)
            {
                return ChangePasswordResult.PasswordsDontMatch;
            }
            if (!_cryptoManager.CheckPassword(currentPassword, identity.Password))
            {
                return ChangePasswordResult.InvalidCurrentPassword;
            }

            identity.Password = _cryptoManager.HashPassword(password1);
            await _identityRepository.Update(identity);
            return ChangePasswordResult.Ok;
        }

        private async Task<SaveIdentityResult> Create(
            string username, 
            string email, 
            string password1, 
            string password2, 
            IdentityTypes type)
        {
            username.CheckIfNull(nameof(username));
            email.CheckIfNull(nameof(email));
            password1.CheckIfNull(nameof(password1));
            password2.CheckIfNull(nameof(password2));

            if (!Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase))
            {
                return SaveIdentityResult.Create(InvalidEmail);
            }
            if (password1 != password2)
            {
                return SaveIdentityResult.Create(PasswordsDontMatch);
            }
            if (await _identityRepository.ExistsByUsername(username))
            {
                return SaveIdentityResult.Create(InvalidUsername);
            }
            if (await _identityRepository.ExistsByEmail(email))
            {
                return SaveIdentityResult.Create(InvalidEmail);
            }

            var identity = new Identity
            {
                Username = username,
                Email = email,
                IdType = type,
                Password = _cryptoManager.HashPassword(password1),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await _identityRepository.Save(identity);
            return SaveIdentityResult.Create(identity, Ok);
        }
    }
}