using AutoMapper;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.Services;

public class TimeOffService : ITimeOffService
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly ICalendarService _calendarService;

    public TimeOffService(ITimeOffRepository timeOffRepository, IMapper mapper, IUserService userService, IEmailService emailService, ICalendarService calendarService)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
        _userService = userService;
        _emailService = emailService;
        _calendarService = calendarService;
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

    public async Task<IEnumerable<TimeOffInfo>> GetByUserIdAsync(string userId)
    {
        return _mapper.Map<IEnumerable<TimeOffInfo>>(await _timeOffRepository.GetByUserIdAsync(userId));
    }

    public async Task<TimeOffInfo> CreateAsync(TimeOffRequest timeOffRequest)
    {
        var timeOff = _mapper.Map<TimeOff>(timeOffRequest);

        if (!(timeOff.StartDate.Year == DateTime.UtcNow.Year && timeOff.EndDate.Year == DateTime.UtcNow.Year))
            throw new ArgumentException("Start date and end date must be in the same year");

        if (timeOff.StartDate < DateTime.UtcNow)
            throw new ArgumentException("Start date must be in the future");

        timeOff.TotalDays = CountDaysExcludingHolidays(timeOff.StartDate, timeOff.EndDate);

        if (timeOff.UserId == null)
            throw new ArgumentException("User id is required");

        if (timeOff.TotalDays < 0)
            throw new ArgumentException("Start date must be before end date");

        var user = await _userService.GetByIdAsync(timeOff.UserId);

        if (timeOff.TotalDays > user.RemainingAnnualTimeOffs)
            throw new UnprocessableEntityException("You don't have enough time off left");

        var CreatedTimeOff = await _timeOffRepository.CreateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(CreatedTimeOff);
    }

    public async Task<TimeOffInfo> UpdateAsync(TimeOffUpdate timeOffUpdate)
    {
        var timeOff = _mapper.Map<TimeOff>(timeOffUpdate);

        timeOff.TotalDays = CountDaysExcludingHolidays(timeOff.StartDate, timeOff.EndDate);

        if (timeOff.IsApproved || !timeOff.IsPending && !timeOff.IsApproved)
            throw new UnprocessableEntityException("You can't make changes on an approved or declined time off request");

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }

    public async Task DeleteAsync(int id)
    {
        var timeOff = await _timeOffRepository.GetByIdAsync(id);

        if (timeOff.IsApproved || !timeOff.IsPending && !timeOff.IsApproved)
            throw new UnprocessableEntityException("You can't delete an approved or declined time off request");

        await _timeOffRepository.DeleteAsync(id);
    }

    public async Task<TimeOffInfo> ApproveAsync(int id, bool isApproved)
    {
        var timeoff = await _timeOffRepository.GetByIdAsync(id);

        timeoff.IsApproved = isApproved;

        timeoff.IsPending = false;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeoff);

        if (timeoff.UserId == null)
            throw new NullReferenceException("User id is missing");

        var user = await _userService.GetByIdAsync(timeoff.UserId);
        if (updatedTimeOff.IsApproved)
        {
            await _userService.UpdateRemaningAnnualTimeOff(timeoff.UserId);
            await _emailService.SendEmaiAsync(user.Email, "Time off request approved", $"Your time off request from {timeoff.StartDate} to {timeoff.EndDate} has been approved.");
        }
        else if (!updatedTimeOff.IsApproved)
        {
            await _emailService.SendEmaiAsync(user.Email, "Time off request rejected", $"Your time off request from {timeoff.StartDate} to {timeoff.EndDate} has been rejected.");
        }

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }

    public async Task<TimeOffInfo> CancelRequestAsync(int id, string userId)
    {
        var timeoff = await _timeOffRepository.GetByIdAsync(id);

        if (!timeoff.IsApproved)
            throw new UnprocessableEntityException("You can only cancel an approved or declined time off request");

        if (timeoff.UserId != userId)
            throw new UnprocessableEntityException("You can only cancel your own time off request");

        timeoff.HasCancelRequest = true;
        timeoff.IsPending = true;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeoff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }

    public async Task<TimeOffInfo> ApproveCancelRequestAsync(int id)
    {
        var timeoff = await _timeOffRepository.GetByIdAsync(id);

        if (!timeoff.HasCancelRequest)
            throw new UnprocessableEntityException("You can only approve a cancel request");

        timeoff.IsActive = true;
        timeoff.IsPending = false;
        timeoff.IsApproved = false;
        timeoff.IsCancelled = true;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeoff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }

    public async Task<TimeOffInfo> DrawCancelRequestAsync(int id, string userId)
    {
        var timeOff = await _timeOffRepository.GetByIdAsync(id);

        if (!timeOff.HasCancelRequest)
            throw new UnprocessableEntityException("You can only draw a cancel request");

        if (timeOff.UserId != userId)
            throw new UnprocessableEntityException("You can only draw your own cancel request");

        timeOff.HasCancelRequest = false;

        var updatedTimeOff = await _timeOffRepository.UpdateAsync(timeOff);

        return _mapper.Map<TimeOffInfo>(updatedTimeOff);
    }

    public async Task DeletePastTimeOffAsync()
    {
        var timeOffs = await _timeOffRepository.GetAllAsync();

        foreach (var timeOff in timeOffs)
        {
            if (timeOff.EndDate < DateTime.UtcNow)
            {
                await _timeOffRepository.SoftDeleteAsync(timeOff.Id);
            }
        }
    }

    private int CountDaysExcludingHolidays(DateTime startDate, DateTime endDate)
    {
        int totalDays = 0;

        List<DateTime> eidAlFitrDates = _calendarService.GetEidAlFitrDates();
        List<DateTime> eidAlAdhaDates = _calendarService.GetEidAlAdhaDates();
        List<DateTime> publicHolidays = _calendarService.GetPublicHolidays();

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                if (!(eidAlFitrDates.Contains(date) || eidAlAdhaDates.Contains(date) || publicHolidays.Contains(date)))
                    totalDays++;
            }
        }

        return totalDays;
    }
}


