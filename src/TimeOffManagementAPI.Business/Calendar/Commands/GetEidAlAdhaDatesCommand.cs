using System.Globalization;
using MediatR;

namespace TimeOffManagementAPI.Business.Calendar.Commands;

public record GetEidAlAdhaDatesCommand : IRequest<List<DateTime>>
{
}

public class GetEidAlAdhaDatesCommandHandler : IRequestHandler<GetEidAlAdhaDatesCommand, List<DateTime>>
{
    public Task<List<DateTime>> Handle(GetEidAlAdhaDatesCommand request, CancellationToken cancellationToken)
    {
        int year = DateTime.UtcNow.Year;
        var islamicCalendar = new HijriCalendar();
        int month = islamicCalendar.GetMonth(new DateTime(year, 12, 1));

        List<DateTime> eidAlAdhaDates = new List<DateTime>();

        while (month != 12)
        {
            year++;
            month = islamicCalendar.GetMonth(new DateTime(year, 12, 1));
        }

        int day = 1;
        for (int i = 0; i < 3; i++)
        {
            while (month == 12)
            {
                day++;
                month = islamicCalendar.GetMonth(new DateTime(year, 12, day));
            }

            eidAlAdhaDates.Add(new DateTime(year, 12, day - 1));
            day++;
            month = islamicCalendar.GetMonth(new DateTime(year, 12, day));
        }

        return Task.FromResult(eidAlAdhaDates);
    }
}
