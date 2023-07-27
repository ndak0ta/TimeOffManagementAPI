using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _roleService.GetAllAsync());
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok(await _roleService.GetByIdAsync(id));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByNameAsync(string name)
    {
        return Ok(await _roleService.GetByNameAsync(name));
    }
}