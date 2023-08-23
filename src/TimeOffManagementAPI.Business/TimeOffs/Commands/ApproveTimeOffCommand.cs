using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Email.Commands;
using TimeOffManagementAPI.Business.Users.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public record ApproveTimeOffCommand : IRequest<TimeOffInfo>
{
    public ApproveTimeOffCommand(int id, bool isApproved)
    {
        Id = id;
        IsApproved = isApproved;
    }

    public int Id { get; set; }
    public bool IsApproved { get; set; }
}

public class ApproveTimeOffCommandHandler : IRequestHandler<ApproveTimeOffCommand, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ApproveTimeOffCommandHandler(ITimeOffRepository timeOffRepository, UserManager<User> userManager, IMediator mediator, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _userManager = userManager;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<TimeOffInfo> Handle(ApproveTimeOffCommand request, CancellationToken cancellationToken)
    {
        var timeOff = await _timeOffRepository.GetByIdAsync(request.Id)
        ?? throw new NotFoundException("Time off not found");

        timeOff.IsApproved = request.IsApproved;
        timeOff.IsPending = false;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        var user = await _userManager.FindByIdAsync(updatedTimeOff.UserId);

        if (updatedTimeOff.IsApproved)
        {
            await _mediator.Send(new UpdateRemaningAnnualTimeOffCommand(user.Id));
            await _mediator.Send(new SendEmailCommand(user.Email, "Time off approved", $"Your time off request has been approved. You can check your time off requests from <a href='https://localhost:5001/timeoff'>here</a>"));
        }
        else
        {
            await _mediator.Send(new SendEmailCommand(user.Email, "Time off rejected", $"Your time off request has been rejected. You can check your time off requests from <a href='https://localhost:5001/timeoff'>here</a>"));
        }

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }
}