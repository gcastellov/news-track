using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsTrack.WebApi.Components;
using Polly;

namespace NewsTrack.WebApi.HostedServices
{
    internal class SeederHostedService : IHostedService
    {
        private const int MAX_RETRIES = 10;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeederHostedService> _logger;

        public SeederHostedService(IServiceProvider serviceProvider, ILogger<SeederHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            int attempt = 1;

            await Policy
                .Handle<ApplicationException>()
                .RetryAsync(MAX_RETRIES, async (exception, retryCount) => 
                {
                    _logger.LogDebug(exception, "An exception was thrown while seeding required data");
                    attempt = retryCount + 1; 
                    await Task.Delay(1000 * attempt); 
                })
                .ExecuteAsync(async (ct) => await Seed(attempt, ct), cancellationToken);
        }

        private async Task Seed(int attempt, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Attempt #{attempt} when seeding required data");

            using var scope = _serviceProvider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
            
            if (!cancellationToken.IsCancellationRequested)
            {
                await seeder.Initialize();
            }

            _logger.LogInformation("Seeding required data has been completed");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}