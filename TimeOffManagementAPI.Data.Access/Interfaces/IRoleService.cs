using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Interfaces;

public interface IRoleService
{
    public Task<IEnumerable<Role>> GetAllAsync();
    public Task<Role> GetByIdAsync(string id);
    public Task<Role> GetByNameAsync(string name);
    public Task<IdentityResult> CreateAsync(Role role);
    public Task<IdentityResult> UpdateAsync(Role role);
    public Task<IdentityResult> DisableAsync(string id);
    public Task<IdentityResult> AddUserToRoleAsync(int userId, string roleName);
}