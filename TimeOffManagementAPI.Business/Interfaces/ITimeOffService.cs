using TimeOffManagementAPI.Data.Model;

namespace TimeOffManagementAPI.Business.Interfaces;

public interface ITimeOffService
{
    public Task<IEnumerable<TimeOffRequest>> GetAllAsync();
    public Task<TimeOffRequest> GetByIdAsync(int id);
    public Task<IEnumerable<TimeOffRequest>> GetByUserIdAsync(int userId);
    public Task<TimeOffRequest> CreateAsync(TimeOffRequest timeOffRequest);
    public Task<TimeOffRequest> UpdateAsync(TimeOffRequest timeOffRequest);
    public Task<TimeOffRequest> DeleteAsync(TimeOffRequest timeOffRequest);
}