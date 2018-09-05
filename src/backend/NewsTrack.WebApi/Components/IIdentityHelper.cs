using System;

namespace NewsTrack.WebApi.Components
{
    public interface IIdentityHelper
    {
        Guid Id { get; }
        string Username { get; }
    }
}