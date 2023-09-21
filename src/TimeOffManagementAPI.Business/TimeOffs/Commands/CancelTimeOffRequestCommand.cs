using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public record CancelTimeOffRequestCommand : IRequest<TimeOffInfo>
{
    public CancelTimeOffRequestCommand(int timeOffId, string? userId)
    {
        TimeOffId = timeOffId;
        UserId = userId;
    }

    public int TimeOffId { get; set; }
    public string? UserId { get; set; }
}

public class CancelTimeOffRequestCommandHandler : IRequestHandler<CancelTimeOffRequestCommand, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CancelTimeOffRequestCommandHandler(ITimeOffRepository timeOffRepository, IMediator mediator, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<TimeOffInfo> Handle(CancelTimeOffRequestCommand request, CancellationToken cancellationToken)
    {
        TimeOff timeOff = await _timeOffRepository.GetByIdAsync(request.TimeOffId)
        ?? throw new NotFoundException("Time off not found");

        if (timeOff.Status != TimeOffStates.Approved)
            throw new UnprocessableEntityException("You can only cancel an approved time off");

        if (timeOff.UserId != request.UserId)
            throw new UnprocessableEntityException("You can only send a cancel request for your own time off");

        timeOff.Status = TimeOffStates.CancelRequested;

        TimeOff updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }
}