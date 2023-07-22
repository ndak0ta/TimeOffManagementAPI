using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Access.Abstractions;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class TimeOffRepository : BaseRepository<TimeOff>, ITimeOffRepository
{

    public TimeOffRepository(TimeOffManagementDBContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TimeOff>> GetByUserIdAsync(string userId)
    {
        return await GetByPropertyAsync(t => t.UserId == userId.ToString());
    }
}
