using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;


namespace TimeOffManagementAPI.Business.Interfaces;

public interface ITimeOffService
{
    public Task<IEnumerable<TimeOff>> GetAllAsync();
    public Task<TimeOff> GetByIdAsync(int id);
    public Task<IEnumerable<TimeOffInfo>> GetByUserIdAsync(string userId);
    public Task<TimeOff> CreateAsync(TimeOffRequest timeOffRequest);
    public Task<TimeOff> UpdateAsync(TimeOffUpdate timeOffUpdate);
    public Task DeleteAsync(int id);
    public Task<TimeOff> ApproveAsync(int id, bool isApproved);
}