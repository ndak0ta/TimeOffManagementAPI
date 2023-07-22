using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Interfaces;

public interface IHolidayService
{
    public Task<IEnumerable<Holiday>> GetAllAsync();
    public Task<Holiday> GetByIdAsync(int id);
    public Task<Holiday> CreateAsync(Holiday holiday);
    public Task<Holiday> UpdateAsync(Holiday holiday);
    public Task DeleteAsync(int id);
    public Task UpdateHolidaysFromAPI();
}