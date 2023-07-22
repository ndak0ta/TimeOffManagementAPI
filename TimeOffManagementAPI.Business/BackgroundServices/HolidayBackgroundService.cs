using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Business.BackgroundServices;

public class HolidayBackgroundService : BackgroundService
{
    private readonly ILogger<HolidayBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public HolidayBackgroundService(ILogger<HolidayBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"HolidayBackgroundService is starting.");

        stoppingToken.Register(() => _logger.LogInformation($" HolidayBackgroundService background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"HolidayBackgroundService task doing background work.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var holidayService = scope.ServiceProvider.GetRequiredService<IHolidayService>();

                await holidayService.UpdateHolidaysFromAPI();
            }

            await Task.Delay(TimeSpan.FromDays(5), stoppingToken);
        }

        _logger.LogInformation($"HolidayBackgroundService background task is stopping.");
    }

    private static TimeSpan TimeUntilNextYear()
    {
        DateTime today = DateTime.Today;
        DateTime nextYear = new DateTime(today.Year + 1, 1, 1);
        TimeSpan timeUntilNextYear = nextYear - today;
        return timeUntilNextYear;
    }
}