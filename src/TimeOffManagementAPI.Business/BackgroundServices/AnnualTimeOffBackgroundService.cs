using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeOffManagementAPI.Business.Users.Commands;

namespace TimeOffManagementAPI.Business.BackgroundServices;

public class AnnualTimeOffBackgroundService : BackgroundService
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

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new UpdaAnnualTimeOffCommand(), stoppingToken);
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation($"AnnualTimeOffBackgroundService background task is stopping.");
    }
}
