using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Business.BackgroundServices;

public class DeleteTimeOffBackgroundService : BackgroundService
{
    private readonly ILogger<AnnualTimeOffBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DeleteTimeOffBackgroundService(ILogger<AnnualTimeOffBackgroundService> logger, IServiceProvider serviceProvider)
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
            _logger.LogInformation($"DeleteTimeOffBackgroundService task doing background work.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var timeOffService = scope.ServiceProvider.GetRequiredService<ITimeOffService>();

                await timeOffService.DeletePastTimeOffAsync();
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation($"AnnualTimeOffBackgroundService background task is stopping.");
    }
}
