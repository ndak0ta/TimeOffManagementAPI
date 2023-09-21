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
        TimeOff timeOff = _mapper.Map<TimeOff>(request.TimeOffRequest);

        if (timeOff.StartDate < DateTime.UtcNow)
            throw new ArgumentException("Start date must be in the future");

        if (timeOff.StartDate > timeOff.EndDate)
            throw new ArgumentException("Start date must be before end date");

        if (!(timeOff.StartDate.Year == DateTime.UtcNow.Year && timeOff.EndDate.Year == DateTime.UtcNow.Year))
            throw new ArgumentException("Start date and end date must be in the same year");

        timeOff.TotalDays = _mediator.Send(new CountDaysExcludingHolidaysCommand(timeOff.StartDate, timeOff.EndDate), cancellationToken).Result;

        if (timeOff.UserId == null)
            throw new ArgumentException("User id is required");


        UserInfo user = await _mediator.Send(new GetUserByIdQuery(timeOff.UserId), cancellationToken);

        if (timeOff.TotalDays > user.RemainingAnnualTimeOffs)
            throw new UnprocessableEntityException("You don't have enough time off left");

        TimeOff CreatedTimeOff = await _timeOffRepository.CreateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(CreatedTimeOff);
    }
}


