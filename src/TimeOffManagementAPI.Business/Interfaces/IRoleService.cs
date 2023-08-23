using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(string id);
        Task<Role> GetByNameAsync(string name);
    }
}