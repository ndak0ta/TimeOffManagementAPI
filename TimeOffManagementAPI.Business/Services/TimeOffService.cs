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
    private readonly IEmailService _emailService;

    public TimeOffService(ITimeOffRepository timeOffRepository, IMapper mapper, IUserService userService, IEmailService emailService)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
        _userService = userService;
        _emailService = emailService;
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

        if (!(timeoff.StartDate.Year == DateTime.UtcNow.Year && timeoff.EndDate.Year == DateTime.UtcNow.Year))
            throw new ArgumentException("Start date and end date must be in the same year");

        timeoff.TotalDays = CountDaysExcludingWeekends(timeoff.StartDate, timeoff.EndDate);

        if (timeoff.UserId == null)
            throw new ArgumentException("User id is required");

        if (timeoff.TotalDays < 0)
            throw new ArgumentException("Start date must be before end date");

        var user = await _userService.GetByIdAsync(timeoff.UserId);

        if (timeoff.TotalDays > user.RemainingAnnualTimeOffs)
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

        var result = await _timeOffRepository.UpdateAsync(timeoff);

        if (timeoff.UserId == null)
            throw new NullReferenceException("User id is missing");

        if (result.IsApproved)
        {
            await _userService.UpdateRemaningAnnualTimeOff(timeoff.UserId);
            var user = await _userService.GetByIdAsync(timeoff.UserId);
            await _emailService.SendEmaiAsync(user.Email, "Time off request approved", $"Your time off request from {timeoff.StartDate} to {timeoff.EndDate} has been approved.");
        }
        else if (!result.IsApproved)
        {
            var user = await _userService.GetByIdAsync(timeoff.UserId);
            await _emailService.SendEmaiAsync(user.Email, "Time off request rejected", $"Your time off request from {timeoff.StartDate} to {timeoff.EndDate} has been rejected.");
        }

        return result;
    }

    private int CountDaysExcludingWeekends(DateTime startDate, DateTime endDate)
    {
        int totalDays = 0;

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                totalDays++;
            }
        }

        return totalDays;
    }
}


