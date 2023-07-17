using AutoMapper;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Services;

public class TimeOffService : ITimeOffService
{
    public readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;

    public TimeOffService(ITimeOffRepository timeOffRepository, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TimeOff>> GetAllAsync()
    {
        return await _timeOffRepository.GetAllAsync();
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
}


