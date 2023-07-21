using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Business.BackgroundServices;

public class AnnualTimeOffBackgroundService : BackgroundService, IDisposable
{
    private readonly ILogger<AnnualTimeOffBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AnnualTimeOffBackgroundService(ILogger<AnnualTimeOffBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"AnnualTimeOffBackgroundService is starting.");

        stoppingToken.Register(() => _logger.LogInformation($" AnnualTimeOffBackgroundService background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"AnnualTimeOffBackgroundService task doing background work.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                await userService.UpdateAnnualTimeOffAsync();
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation($"AnnualTimeOffBackgroundService background task is stopping.");
    }
}
