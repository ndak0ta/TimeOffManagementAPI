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
public class TimeOffController : ControllerBase
{
    private readonly ITimeOffService _timeOffService;

    public TimeOffController(ITimeOffService timeOffService)
    {
        _timeOffService = timeOffService;
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _timeOffService.GetAllAsync());
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await _timeOffService.GetByIdAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _timeOffService.GetByUserIdAsync(userId));
    }


    [Authorize(Roles = "Manager")]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        return Ok(await _timeOffService.GetByUserIdAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId); // TODO mesaj yaz

        return Created("", await _timeOffService.CreateAsync(timeOffRequest, userId));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId == null)
            throw new ArgumentNullException(); // TODO mesaj yaz

        return Ok(await _timeOffService.UpdateAsync(timeOffRequest, userId));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _timeOffService.DeleteAsync(id);

        return NoContent();
    }
}

