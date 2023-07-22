using TimeOffManagementAPI.Data.Access.Abstractions;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class HolidayRepository : BaseRepository<Holiday>, IHolidayRepository
{
    public HolidayRepository(TimeOffManagementDBContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Holiday>> GetByDate(DateTime date)
    {
        return await GetByPropertyAsync(h => h.Date == date);
    }
}