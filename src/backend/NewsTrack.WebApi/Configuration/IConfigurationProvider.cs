namespace NewsTrack.WebApi.Configuration
{
    public interface IConfigurationProvider
    {
        TokenConfiguration TokenConfiguration { get; }
        SmtpConfiguration SmtpConfiguration { get; }
    }
}