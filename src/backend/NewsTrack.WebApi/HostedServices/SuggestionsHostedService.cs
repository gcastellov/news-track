using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Hosting;
using NewsTrack.Domain.Services;
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

        public SuggestionsHostedService(IContentService contentService)
        {
            _contentService = contentService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Expression<Action> call = () => _contentService.SetSuggestions();
            RecurringJob.AddOrUpdate(JobId, call, Cron.Hourly);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            RecurringJob.RemoveIfExists(JobId);
            return Task.CompletedTask;
        }
    }
}
