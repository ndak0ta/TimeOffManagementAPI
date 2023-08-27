using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public record DrawCancelRequestCommand : IRequest<TimeOffInfo>
{
    public DrawCancelRequestCommand(int id, string userId)
    {
        Id = id;
        UserId = userId;
    }

    public int Id { get; set; }
    public string UserId { get; set; }
};

public class DrawCancelRequestCommandHandler : IRequestHandler<DrawCancelRequestCommand, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;

    public DrawCancelRequestCommandHandler(ITimeOffRepository timeOffRepository, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
    }

    public async Task<TimeOffInfo> Handle(DrawCancelRequestCommand request, CancellationToken cancellationToken)
    {
        var timeOff = await _timeOffRepository.GetByIdAsync(request.Id);

        if (timeOff.Status == TimeOffStates.CancelRequested)
            throw new UnprocessableEntityException("You can only draw a cancel request");

        if (timeOff.UserId != request.UserId)
            throw new UnprocessableEntityException("You can only draw your own cancel request");

        timeOff.Status = TimeOffStates.Approved;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }
}