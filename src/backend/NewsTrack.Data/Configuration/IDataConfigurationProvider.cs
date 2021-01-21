using System;

namespace NewsTrack.Data.Configuration
{
    public interface IDataConfigurationProvider
    {
        Uri ConnectionString { get; }
    }
}