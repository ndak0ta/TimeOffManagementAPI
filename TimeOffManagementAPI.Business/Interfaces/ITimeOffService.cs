using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;


namespace TimeOffManagementAPI.Business.Interfaces;

public interface ITimeOffService
{
    public Task<IEnumerable<TimeOff>> GetAllAsync();
    public Task<TimeOff> GetByIdAsync(int id);
    public Task<IEnumerable<TimeOff>> GetByUserIdAsync(string userId);
    public Task<TimeOff> CreateAsync(TimeOffRequest timeOffRequest);
    public Task<TimeOff> UpdateAsync(TimeOffRequest timeOffRequest);
    public Task DeleteAsync(int id);
}