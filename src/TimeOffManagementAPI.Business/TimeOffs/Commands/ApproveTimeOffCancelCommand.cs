using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public record ApproveTimeOffCancelCommand : IRequest<TimeOffInfo>
{
    public ApproveTimeOffCancelCommand(int timeOffId, bool isApproved)
    {
        TimeOffId = timeOffId;
        IsApproved = isApproved;
    }

    public int TimeOffId { get; set; }
    public bool IsApproved { get; set; }
}

public class ApproveTimeOffCancelCommandHandler : IRequestHandler<ApproveTimeOffCancelCommand, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;

    public ApproveTimeOffCancelCommandHandler(ITimeOffRepository timeOffRepository, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
    }

    public async Task<TimeOffInfo> Handle(ApproveTimeOffCancelCommand request, CancellationToken cancellationToken)
    {
        var timeOff = await _timeOffRepository.GetByIdAsync(request.TimeOffId)
        ?? throw new NotFoundException("Time off not found");

        if (timeOff.Status == TimeOffStates.Cancelled)
            throw new UnprocessableEntityException("You can only approve a cancel request");

        timeOff.Status = request.IsApproved ? TimeOffStates.Cancelled : TimeOffStates.CancelRejected;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }
}