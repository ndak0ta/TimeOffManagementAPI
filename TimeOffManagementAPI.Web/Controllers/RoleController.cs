using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize(Policy = "ManagerPolicy")]
[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _roleService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok(await _roleService.GetByIdAsync(id));
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByNameAsync(string name)
    {
        return Ok(await _roleService.GetByNameAsync(name));
    }
    /*
    [HttpPatch("toggle-disable/{id}")]
    public async Task<IActionResult> ToggleDisableAsync(string id)
    {
        return Ok(await _roleService.ToggleDisableAsync(id));
    }
    */
}