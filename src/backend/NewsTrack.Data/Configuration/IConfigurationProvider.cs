namespace NewsTrack.Data.Configuration
{
    public interface IConfigurationProvider
    {
        string ConnectionString { get; set; }
    }
}