using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsTrack.WebApi.Components;

namespace NewsTrack.WebApi.HostedServices
{
    internal class SeederHostedService : IHostedService
    {
        private readonly ISeeder _seeder;
        private readonly ILogger<SeederHostedService> _logger;

        public SeederHostedService(ISeeder seeder, ILogger<SeederHostedService> logger)
        {
            _seeder = seeder;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _seeder.Initialize();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception has been thrown whilst trying to set default data");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}