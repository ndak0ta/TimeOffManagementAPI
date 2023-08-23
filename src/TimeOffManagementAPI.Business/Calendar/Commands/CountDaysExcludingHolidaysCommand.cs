using MediatR;

namespace TimeOffManagementAPI.Business.Calendar.Commands;

public record CountDaysExcludingHolidaysCommand : IRequest<int>
{
    public CountDaysExcludingHolidaysCommand(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class CountDaysExcludingHolidaysCommandHandler : IRequestHandler<CountDaysExcludingHolidaysCommand, int>
{
    private readonly IMediator _mediator;

    public CountDaysExcludingHolidaysCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<int> Handle(CountDaysExcludingHolidaysCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        int totalDays = 0;

        List<DateTime> eidAlFitrDates = _mediator.Send(new GetEidAlFitrDatesCommand()).Result;
        List<DateTime> eidAlAdhaDates = _mediator.Send(new GetEidAlAdhaDatesCommand()).Result;
        List<DateTime> publicHolidays = _mediator.Send(new GetPublicHolidaysCommand()).Result;

        for (DateTime date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                if (!(eidAlFitrDates.Contains(date) || eidAlAdhaDates.Contains(date) || publicHolidays.Contains(date)))
                    totalDays++;
            }
        }

        return totalDays;
    }
}