using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;


namespace TimeOffManagementAPI.Business.Interfaces;

public interface ITimeOffService
{
    public Task<IEnumerable<TimeOff>> GetAllAsync();
    public Task<TimeOff> GetByIdAsync(int id);
    public Task<IEnumerable<TimeOffInfo>> GetByUserIdAsync(string userId);
    public Task<TimeOffInfo> CreateAsync(TimeOffRequest timeOffRequest);
    public Task<TimeOffInfo> UpdateAsync(TimeOffUpdate timeOffUpdate);
    public Task DeleteAsync(int id);
    public Task<TimeOffInfo> ApproveAsync(int id, bool isApproved);
    public Task<TimeOffInfo> CancelRequestAsync(int id, string userId);
    public Task<TimeOffInfo> ApproveCancelRequestAsync(int id);
    public Task<TimeOffInfo> DrawCancelRequestAsync(int id, string userId);
    public Task DeletePastTimeOffAsync();
}