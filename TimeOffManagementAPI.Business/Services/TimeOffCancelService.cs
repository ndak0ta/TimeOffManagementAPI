using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Exceptions;
using AutoMapper;

namespace TimeOffManagementAPI.Business.Services;

public class TimeOffCancelService : ITimeOffCancelService
{
    private readonly ITimeOffCancelRepository _timeOffCancelRequestRepository;
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TimeOffCancelService(ITimeOffCancelRepository timeOffCancelRequestRepository, ITimeOffRepository timeOffRepository, IUserService userService, IMapper mapper)
    {
        _timeOffCancelRequestRepository = timeOffCancelRequestRepository;
        _timeOffRepository = timeOffRepository;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TimeOffCancel>> GetAllAsync()
    {
        return await _timeOffCancelRequestRepository.GetAllAsync();
    }

    public async Task<IEnumerable<TimeOffCancel>> GetAllByUserIdAsync(string id)
    {
        return await _timeOffCancelRequestRepository.GetAllByUserIdAsync(id);
    }

    public async Task<TimeOffCancel> GetByTimeOffIdAsync(int id)
    {
        return await _timeOffCancelRequestRepository.GetByTimeOffIdAsync(id);
    }

    public async Task<TimeOffCancel> CreateAsync(TimeOffCancelRequest timeOffCancelRequest)
    {
        var timeOffCancel = _mapper.Map<TimeOffCancel>(timeOffCancelRequest);

        timeOffCancel.IsPending = true;
        timeOffCancel.IsApproved = false;

        return await _timeOffCancelRequestRepository.CreateAsync(timeOffCancel);
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var TimeOffCancel = await _timeOffCancelRequestRepository.GetByIdAsync(id)
        ?? throw new NotFoundException("Time off cancelation request not found");

        if (TimeOffCancel.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to delete this time off cancelation request");

        if (TimeOffCancel.IsApproved)
            throw new UnauthorizedAccessException("You are not authorized to delete this time off cancelation request");

        await _timeOffCancelRequestRepository.DeleteAsync(id);
    }

    public async Task<TimeOffCancel> ApproveAsync(int id, bool isApproved)
    {
        var TimeOffCancel = await _timeOffCancelRequestRepository.GetByIdAsync(id)
        ?? throw new NotFoundException("Time off cancelation request not found");

        TimeOffCancel.IsApproved = isApproved;
        TimeOffCancel.IsPending = false;

        var timeOff = await _timeOffRepository.GetByIdAsync(TimeOffCancel.TimeOffId);

        timeOff.IsCancelled = isApproved;

        await _timeOffRepository.UpdateAsync(timeOff);

        if (TimeOffCancel.UserId == null)
            throw new ArgumentNullException("User id missing");

        var user = await _userService.GetByIdAsync(TimeOffCancel.UserId);

        if (isApproved)
        {
            await _userService.UpdateRemaningAnnualTimeOff(user.Id);
        }

        return await _timeOffCancelRequestRepository.UpdateAsync(TimeOffCancel);
    }
}