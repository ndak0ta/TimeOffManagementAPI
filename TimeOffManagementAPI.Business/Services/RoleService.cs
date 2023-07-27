using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;

    public RoleService(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
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
}