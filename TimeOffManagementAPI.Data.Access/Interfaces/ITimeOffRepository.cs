using TimeOffManagementAPI.Data.Model;

namespace TimeOffManagementAPI.Data.Access.Interfaces;

public interface ITimeOffRepository
{
    Task<IEnumerable<TimeOffRequest>> GetAllAsync();
    Task<TimeOffRequest> GetByIdAsync(int id);
    Task<IEnumerable<TimeOffRequest>> GetByUserIdAsync(int userId);
    Task<TimeOffRequest> CreateAsync(TimeOffRequest timeOffRequest);
    Task<TimeOffRequest> UpdateAsync(TimeOffRequest timeOffRequest);
    Task<TimeOffRequest> DeleteAsync(TimeOffRequest timeOffRequest);
}