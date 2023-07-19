using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;

    public RoleService(RoleManager<Role> roleRepository)
    {
        _roleManager = roleRepository;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<Role> GetByIdAsync(string id)
    {
        return await _roleManager.FindByIdAsync(id);
    }

    public async Task<Role> GetByNameAsync(string name)
    {
        return await _roleManager.FindByNameAsync(name);
    }

    /* Şimdilik devre dışı
    public async Task<IdentityResult> CreateAsync(Role role)
    {
        return await _roleManager.CreateAsync(role);
    }

    public async Task<IdentityResult> UpdateAsync(Role role)
    {
        return await _roleManager.UpdateAsync(role);
    }

    public async Task<IdentityResult> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        return await _roleManager.DeleteAsync(role);
    }


    public async Task<IdentityResult> ToggleDisableAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        role.IsActive = !role.IsActive;

        return await _roleManager.UpdateAsync(role);
    }
    */
}