using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Business.Calendar.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public record UpdateTimeOffCommand : IRequest<TimeOffInfo>
{
    public UpdateTimeOffCommand(TimeOffUpdate timeOffUpdate)
    {
        TimeOffUpdate = timeOffUpdate;
    }

    public TimeOffUpdate TimeOffUpdate { get; set; }
}

public class UpdateTimeOffCommandHandler : IRequestHandler<UpdateTimeOffCommand, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateTimeOffCommandHandler(ITimeOffRepository timeOffRepository, IMapper mapper, IMediator mediator)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TimeOffInfo> Handle(UpdateTimeOffCommand request, CancellationToken cancellationToken)
    {
        var timeOff = _mapper.Map<TimeOff>(request.TimeOffUpdate);

        timeOff.TotalDays = _mediator.Send(new CountDaysExcludingHolidaysCommand(timeOff.StartDate, timeOff.EndDate), cancellationToken).Result;

        if (timeOff.Status != TimeOffStates.Pending)
            throw new UnprocessableEntityException("You can't make changes on an approved or rejected time off request");

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }
}