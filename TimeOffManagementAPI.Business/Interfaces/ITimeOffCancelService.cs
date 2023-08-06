using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Interfaces;

public interface ITimeOffCancelService
{
    public Task<IEnumerable<TimeOffCancel>> GetAllAsync();
    public Task<IEnumerable<TimeOffCancel>> GetAllByUserIdAsync(string id);
    public Task<TimeOffCancel> GetByTimeOffIdAsync(int id);
    public Task<TimeOffCancel> CreateAsync(TimeOffCancelRequest timeOffCancelRequest);
    public Task DeleteAsync(int id, string userId);
    public Task<TimeOffCancel> ApproveAsync(int id, bool isApproved);
}
