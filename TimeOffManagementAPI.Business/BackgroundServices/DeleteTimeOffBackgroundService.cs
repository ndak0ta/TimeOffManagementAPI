using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Business.Interfaces;
using MediatR;
using TimeOffManagementAPI.Business.TimeOffs.Commands;

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
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new DeletePastTimeOffCommand());
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation($"AnnualTimeOffBackgroundService background task is stopping.");
    }
}
