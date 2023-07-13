using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model;

namespace TimeOffManagementAPI.Business.Services;

public class TimeOffService : ITimeOffService
{
    public readonly ITimeOffRepository _timeOffRepository;

    public TimeOffService(ITimeOffRepository timeOffRepository)
    {
        _timeOffRepository = timeOffRepository;
    }

    public async Task<IEnumerable<TimeOffRequest>> GetAllAsync()
    {
        return await _timeOffRepository.GetAllAsync();
    }

    public async Task<TimeOffRequest> GetByIdAsync(int id)
    {
        return await _timeOffRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TimeOffRequest>> GetByUserIdAsync(int userId)
    {
        return await _timeOffRepository.GetByUserIdAsync(userId);
    }

    public async Task<TimeOffRequest> CreateAsync(TimeOffRequest timeOffRequest)
    {
        return await _timeOffRepository.CreateAsync(timeOffRequest);
    }

    public async Task<TimeOffRequest> UpdateAsync(TimeOffRequest timeOffRequest)
    {
        return await _timeOffRepository.UpdateAsync(timeOffRequest);
    }

    public async Task<TimeOffRequest> DeleteAsync(int id)
    {
        var timeOffRequest = await _timeOffRepository.GetByIdAsync(id);

        return await _timeOffRepository.DeleteAsync(timeOffRequest);
    }
}


