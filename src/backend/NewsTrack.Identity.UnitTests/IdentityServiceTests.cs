using System;
using System.Threading.Tasks;
using Moq;
using NewsTrack.Identity.Encryption;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Results;
using NewsTrack.Identity.Services;
using Xunit;
using FluentAssertions;
using MediatR;

namespace NewsTrack.Identity.UnitTests
{
    public class IdentityServiceTests
    {
        private IIdentityService _identityService;
        private Mock<IIdentityRepository> _identityRepositoryMock;
        private Mock<ICryptoManager> _cryptoManagerMock;
        private Mock<IMediator> _mediatorMock;

        const string Email = "some@email.com";
        const string Pwd = "somepwd";

        public IdentityServiceTests()
        {
            _identityRepositoryMock = new Mock<IIdentityRepository>();
            _cryptoManagerMock = new Mock<ICryptoManager>();
            _mediatorMock = new Mock<IMediator>();
            _identityService = new IdentityService(
                _identityRepositoryMock.Object,
                _cryptoManagerMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task WhenUserAuthenticatesAndUserDoesNotExist_ThenReturnsFailure()
        {          
            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(default(Identity)));

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Failed);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
        }

        [Fact]
        public async Task WhenUserAuthenticatesAndUserIsDisabled_ThenReturnsFailure()
        {
            var identity = new Identity {IsEnabled = false};

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Failed);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
        }

        [Fact]
        public async Task WhenUserAuthenticatesAndUserIsLockedOut_ThenReturnsLockout()
        {
            var identity = new Identity
            {
                IsEnabled = true,
                LockoutEnd = DateTime.UtcNow.AddDays(1)
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Lockout);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
        }

        [Fact]
        public async Task WhenUserAuthenticatesWithWrongPassword_ThenReturnsFailureAndIncreaseAttempt()
        {
            const uint currentAttempts = 2;
            var identity = new Identity
            {
                IsEnabled = true,
                Password = "somepassword",
                AccessFailedCount = currentAttempts
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));
            _identityRepositoryMock.Setup(m => m.Update(identity)).Callback((Identity i) =>
                i.AccessFailedCount.Should().Be(currentAttempts + 1))
                .Returns(Task.CompletedTask);

            _cryptoManagerMock.Setup(m => m.CheckPassword(Pwd, identity.Password)).Returns(() => false);

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Failed);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
            _cryptoManagerMock.Verify(m => m.CheckPassword(Pwd, identity.Password), Times.Once);
            _identityRepositoryMock.Verify(m => m.Update(identity), Times.Once);
        }

        [Fact]
        public async Task WhenUserAuthenticatesWithWrongPassword_ThenIncreaseAttemptAndReturnsLockOut()
        {
            const uint currentAttempts = 5;
            var identity = new Identity
            {
                IsEnabled = true,
                Password = "somepassword",
                AccessFailedCount = currentAttempts
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));
            _identityRepositoryMock.Setup(m => m.Update(identity)).Callback((Identity i) =>
                {
                    i.AccessFailedCount.Should().Be(currentAttempts + 1);
                    i.LockoutEnd.Should().NotBeNull();
                })
                .Returns(Task.CompletedTask);

            _cryptoManagerMock.Setup(m => m.CheckPassword(Pwd, identity.Password)).Returns(() => false);

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Lockout);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
            _cryptoManagerMock.Verify(m => m.CheckPassword(Pwd, identity.Password), Times.Once);
            _identityRepositoryMock.Verify(m => m.Update(identity), Times.Once);
        }

        [Fact]
        public async Task WhenUserAuthenticatesWithGoodPasswordAndIsLockout_ThenReurnsOkAndResetLockOut()
        {
            const uint currentAttempts = 5;
            var identity = new Identity
            {
                IsEnabled = true,
                Password = "somepassword",
                AccessFailedCount = currentAttempts,
                LockoutEnd = DateTime.UtcNow.AddMinutes(-1)
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));
            _identityRepositoryMock.Setup(m => m.Update(identity)).Callback((Identity i) =>
                {
                    i.AccessFailedCount.Should().Be((uint)0);
                    i.LockoutEnd.Should().BeNull();
                })
                .Returns(Task.CompletedTask);

            _cryptoManagerMock.Setup(m => m.CheckPassword(Pwd, identity.Password)).Returns(() => true);

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Ok);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
            _cryptoManagerMock.Verify(m => m.CheckPassword(Pwd, identity.Password), Times.Once);
            _identityRepositoryMock.Verify(m => m.Update(identity), Times.Once);
        }

        [Fact]
        public async Task WhenUserAuthenticatesWithGood_ThenReturnsOk()
        {
            var identity = new Identity
            {
                IsEnabled = true,
                Password = "somepassword"
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));
            _cryptoManagerMock.Setup(m => m.CheckPassword(Pwd, identity.Password)).Returns(() => true);

            var result = await _identityService.Authenticate(Email, Pwd);
            result.Should().Be(AuthenticateResult.Ok);

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
            _cryptoManagerMock.Verify(m => m.CheckPassword(Pwd, identity.Password), Times.Once);
            _identityRepositoryMock.Verify(m => m.Update(identity), Times.Once);
        }

        [Fact]
        public async Task WhenConfirmingGoodSecurityStampAndUserIsDisabled_ThenReturnsTrue()
        {
            var identity = new Identity
            {
                SecurityStamp = "something",
                IsEnabled = false
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));

            var result = await _identityService.Confirm(Email, identity.SecurityStamp);
            result.Should().BeTrue();

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
        }

        [Fact]
        public async Task WhenConfirmingGoodSecurityStampAndUserIsEnabled_ThenReturnsFalse()
        {
            var identity = new Identity
            {
                SecurityStamp = "something",
                IsEnabled = true
            };

            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(identity));

            var result = await _identityService.Confirm(Email, identity.SecurityStamp);
            result.Should().BeFalse();

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
        }


        [Fact]
        public async Task WhenConfirmingGoodSecurityStampAndUserNotExists_ThenReturnsFalse()
        {
            _identityRepositoryMock.Setup(m => m.GetByEmail(Email)).Returns(Task.FromResult(default(Identity)));

            var result = await _identityService.Confirm(Email, "somestamp");
            result.Should().BeFalse();

            _identityRepositoryMock.Verify(m => m.GetByEmail(Email), Times.Once);
        }
    }
}
