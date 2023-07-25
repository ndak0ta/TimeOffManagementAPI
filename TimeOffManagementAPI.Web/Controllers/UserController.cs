using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByTokenAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _userService.GetByIdAsync(userId));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetByUsernameAsync(string username)
    {
        return Ok(await _userService.GetByUsernameAsync(username));
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmailAsync(string email)
    {
        return Ok(await _userService.GetByEmailAsync(email));
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] UserRegistration user)
    {
        return Created("", await _userService.CreateAsync(user));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserUpdate user)
    {
        return Ok(await _userService.UpdateAsync(user));
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("hard-update")]
    public async Task<IActionResult> HardUpdateAsync([FromBody] User user)
    {
        return Ok(await _userService.HardUpdateAsync(user));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        return Ok(await _userService.DeleteAsync(id));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePassword changePassword)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        changePassword.Id = userId;

        return Ok(await _userService.ChangePasswordAsync(changePassword));
    }

    [HttpPost("{id}/set-annual")]
    public async Task<IActionResult> SetAnnualTimeOffAsync([FromBody] string id, int newAnnualTimeOff)
    {
        return Ok(await _userService.SetAnnualTimeOffAsync(id, newAnnualTimeOff));
    }

}