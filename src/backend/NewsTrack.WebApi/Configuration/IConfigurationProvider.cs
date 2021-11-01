using System;

namespace NewsTrack.WebApi.Configuration
{
    public interface IConfigurationProvider
    {
        TokenConfiguration TokenConfiguration { get; }
        SmtpConfiguration SmtpConfiguration { get; }
        Uri ApiUrl { get; }
        Uri SignInUrl { get; }
        string SuggestionsSchedule { get; }
    }
}