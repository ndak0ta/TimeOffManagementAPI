using AutoMapper;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.Services;

public class TimeOffService : ITimeOffService
{
    public readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public TimeOffService(ITimeOffRepository timeOffRepository, IMapper mapper, IUserService userService)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<IEnumerable<TimeOff>> GetAllAsync()
    {
        var result = await _timeOffRepository.GetAllAsync();
        return result.Where(t => t.IsActive);
    }

    public async Task<IEnumerable<TimeOff>> GetAllPasiveAsync()
    {
        var result = await _timeOffRepository.GetAllAsync();
        return result.Where(t => !t.IsActive);
    }

    public async Task<IEnumerable<TimeOff>> GetAllPendingAsync()
    {
        var result = await _timeOffRepository.GetAllAsync();
        return result.Where(t => t.IsActive && t.IsPending);
    }

    public async Task<TimeOff> GetByIdAsync(int id)
    {
        return await _timeOffRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TimeOff>> GetByUserIdAsync(string userId)
    {
        return await _timeOffRepository.GetByUserIdAsync(userId);
    }

    public async Task<TimeOff> CreateAsync(TimeOffRequest timeOffRequest)
    {
        var timeoff = _mapper.Map<TimeOff>(timeOffRequest);

        if (timeoff.StartDate.Year == DateTime.UtcNow.Year && timeoff.EndDate.Year == DateTime.UtcNow.Year)
            throw new ArgumentException("Start date and end date must be in the same year");

        timeoff.TotalDays = (timeoff.EndDate - timeoff.StartDate).Days;

        if (timeoff.UserId == null)
            throw new ArgumentException("User id is required");

        if (timeoff.TotalDays < 0)
            throw new ArgumentException("Start date must be before end date");

        var user = await _userService.GetByIdAsync(timeoff.UserId);

        var timeOffLeft = await _userService.TimeOffLeftAsync(user.Id);

        if (timeoff.TotalDays > timeOffLeft)
            throw new UnprocessableEntityException("You don't have enough time off left");

        return await _timeOffRepository.CreateAsync(timeoff);
    }

    public async Task<TimeOff> UpdateAsync(TimeOffRequest timeOffRequest)
    {
        var timeoff = _mapper.Map<TimeOff>(timeOffRequest);

        return await _timeOffRepository.UpdateAsync(timeoff);
    }

    public async Task DeleteAsync(int id)
    {
        await _timeOffRepository.DeleteAsync(id);
    }

    public async Task<TimeOff> ApproveAsync(int id, bool isApproved)
    {
        var timeoff = await _timeOffRepository.GetByIdAsync(id);

        timeoff.IsApproved = isApproved;

        timeoff.IsPending = false;

        return await _timeOffRepository.UpdateAsync(timeoff);
    }
}


