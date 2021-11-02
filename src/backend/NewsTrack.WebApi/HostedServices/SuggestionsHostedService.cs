using Hangfire;
using Microsoft.Extensions.DependencyInjection;
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

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationProvider _configurationProvider;

        public SuggestionsHostedService(IServiceProvider serviceProvider, IConfigurationProvider configurationProvider)
        {
            _serviceProvider = serviceProvider;
            _configurationProvider = configurationProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Expression<Action> call = () => SetSuggestions();
            RecurringJob.AddOrUpdate(JobId, call, _configurationProvider.SuggestionsSchedule);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            RecurringJob.RemoveIfExists(JobId);
            return Task.CompletedTask;
        }

        public Task SetSuggestions()
        {
            using var scope = _serviceProvider.CreateScope();
            var contentService = scope.ServiceProvider.GetRequiredService<IContentService>();
            return contentService.SetSuggestions();
        }
    }
}
