using MediatR;

namespace TimeOffManagementAPI.Business.Calendar.Commands;

public class GetPublicHolidaysCommand : IRequest<List<DateTime>>
{
}

public class GetPublicHolidaysCommandHandler : IRequestHandler<GetPublicHolidaysCommand, List<DateTime>>
{
    private readonly IMediator _mediator;

    public GetPublicHolidaysCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<DateTime>> Handle(GetPublicHolidaysCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        List<DateTime> publicHolidays = new()
        {
            new DateTime(DateTime.UtcNow.Year, 1, 1),
            new DateTime(DateTime.UtcNow.Year, 5, 1)
        };

        return publicHolidays;
    }
}

