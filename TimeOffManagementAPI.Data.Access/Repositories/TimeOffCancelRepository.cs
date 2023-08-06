using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Access.Abstractions;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class TimeOffCancelRepository : BaseRepository<TimeOffCancel>, ITimeOffCancelRepository
{

    public TimeOffCancelRepository(TimeOffManagementDBContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TimeOffCancel>> GetAllByUserIdAsync(string id)
    {
        return await GetByPropertyAsync(t => t.UserId == id);
    }

    public async Task<TimeOffCancel> GetByTimeOffIdAsync(int id)
    {
        return await GetByPropertyFirstAsync(t => t.TimeOffId == id);
    }
}