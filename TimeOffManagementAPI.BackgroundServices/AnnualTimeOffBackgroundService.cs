using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.BackgroundServices;

public class AnnualTimeOffBackgroundService : BackgroundService
{
    private readonly ILogger<AnnualTimeOffBackgroundService> _logger;
    private readonly IUserService _userService;

    public AnnualTimeOffBackgroundService(ILogger<AnnualTimeOffBackgroundService> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"AnnualTimeOffBackgroundService is starting.");

        stoppingToken.Register(() => _logger.LogInformation($" AnnualTimeOffBackgroundService background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"AnnualTimeOffBackgroundService task doing background work.");

            await _userService.UpdateAnnualTimeOffAsync();

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation($"AnnualTimeOffBackgroundService background task is stopping.");
    }
}
