using Hangfire;
using Microsoft.Extensions.Hosting;
using NewsTrack.Domain.Services;
using NewsTrack.WebApi.Configuration;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NewsTrack.WebApi.HostedServices
{
    internal class SuggestionsHostedService : IHostedService
    {
        private const string JobId = "suggestionsJob";

        private readonly IContentService _contentService;
        private readonly IConfigurationProvider _configurationProvider;

        public SuggestionsHostedService(IContentService contentService, IConfigurationProvider configurationProvider)
        {
            _contentService = contentService;
            _configurationProvider = configurationProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Expression<Action> call = () => _contentService.SetSuggestions();
            RecurringJob.AddOrUpdate(JobId, call, _configurationProvider.SuggestionsSchedule);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            RecurringJob.RemoveIfExists(JobId);
            return Task.CompletedTask;
        }
    }
}
