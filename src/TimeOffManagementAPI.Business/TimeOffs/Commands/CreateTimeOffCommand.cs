using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Business.Calendar.Commands;
using TimeOffManagementAPI.Business.Users.Queries;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public class CreateTimeOffCommand : IRequest<TimeOffInfo>
{
    public CreateTimeOffCommand(TimeOffRequest timeOffRequest)
    {
        TimeOffRequest = timeOffRequest;
    }

    public TimeOffRequest? TimeOffRequest { get; set; }
}

public class CreateTimeOffCommandHandler : IRequestHandler<CreateTimeOffCommand, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateTimeOffCommandHandler(ITimeOffRepository timeOffRepository, IMapper mapper, IMediator mediator)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TimeOffInfo> Handle(CreateTimeOffCommand request, CancellationToken cancellationToken)
    {
        var timeOff = _mapper.Map<TimeOff>(request.TimeOffRequest);

        if (!(timeOff.StartDate.Year == DateTime.UtcNow.Year && timeOff.EndDate.Year == DateTime.UtcNow.Year))
            throw new ArgumentException("Start date and end date must be in the same year");

        if (timeOff.StartDate < DateTime.UtcNow)
            throw new ArgumentException("Start date must be in the future");

        timeOff.TotalDays = _mediator.Send(new CountDaysExcludingHolidaysCommand(timeOff.StartDate, timeOff.EndDate)).Result;

        if (timeOff.UserId == null)
            throw new ArgumentException("User id is required");

        if (timeOff.TotalDays < 0)
            throw new ArgumentException("Start date must be before end date");

        var user = await _mediator.Send(new GetUserByIdQuery(timeOff.UserId));

        if (timeOff.TotalDays > user.RemainingAnnualTimeOffs)
            throw new UnprocessableEntityException("You don't have enough time off left: " + user.RemainingAnnualTimeOffs);

        var CreatedTimeOff = await _timeOffRepository.CreateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(CreatedTimeOff);
    }
}


