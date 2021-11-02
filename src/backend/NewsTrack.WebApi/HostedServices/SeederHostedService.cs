using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsTrack.WebApi.Components;

namespace NewsTrack.WebApi.HostedServices
{
    internal class SeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeederHostedService> _logger;

        public SeederHostedService(IServiceProvider serviceProvider, ILogger<SeederHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
                await seeder.Initialize();
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