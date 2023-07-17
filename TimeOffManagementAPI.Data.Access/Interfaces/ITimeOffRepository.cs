using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Interfaces;

public interface ITimeOffRepository
{
    public Task<IEnumerable<TimeOff>> GetAllAsync();
    public Task<TimeOff> GetByIdAsync(int id);
    public Task<IEnumerable<TimeOff>> GetByUserIdAsync(string userId);
    public Task<TimeOff> CreateAsync(TimeOff TimeOff);
    public Task<TimeOff> UpdateAsync(TimeOff TimeOff);
    public Task DeleteAsync(int id);
}