using System.Globalization;
using MediatR;

namespace TimeOffManagementAPI.Business.Calendar.Commands;

public class GetEidAlFitrDatesCommand : IRequest<List<DateTime>>
{
}

public class GetEidAlFitrDatesCommandHandler : IRequestHandler<GetEidAlFitrDatesCommand, List<DateTime>>
{
    public Task<List<DateTime>> Handle(GetEidAlFitrDatesCommand request, CancellationToken cancellationToken)
    {
        int year = DateTime.UtcNow.Year;
        var islamicCalendar = new HijriCalendar();
        int month = islamicCalendar.GetMonth(new DateTime(year, 1, 1));

        List<DateTime> ramadanDates = new List<DateTime>();

        while (month != 9)
        {
            year++;
            month = islamicCalendar.GetMonth(new DateTime(year, 1, 1));
        }

        int day = 1;
        for (int i = 0; i < 3; i++)
        {
            while (month == 9)
            {
                day++;
                month = islamicCalendar.GetMonth(new DateTime(year, 1, day));
            }

            ramadanDates.Add(new DateTime(year, 1, day - 1));
            day++;
            month = islamicCalendar.GetMonth(new DateTime(year, 1, day));
        }

        return Task.FromResult(ramadanDates);
    }
}
