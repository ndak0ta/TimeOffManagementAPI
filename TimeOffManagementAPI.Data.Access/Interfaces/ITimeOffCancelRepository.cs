using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Interfaces;

public interface ITimeOffCancelRepository
{
    public Task<IEnumerable<TimeOffCancel>> GetAllAsync();
    public Task<IEnumerable<TimeOffCancel>> GetAllByUserIdAsync(string id);
    public Task<TimeOffCancel> GetByIdAsync(int id);
    public Task<TimeOffCancel> GetByTimeOffIdAsync(int id);
    public Task<TimeOffCancel> CreateAsync(TimeOffCancel timeOffCancel);
    public Task<TimeOffCancel> UpdateAsync(TimeOffCancel timeOffCancel);
    public Task DeleteAsync(int id);
}