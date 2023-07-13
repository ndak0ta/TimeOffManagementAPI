using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Data.Model;
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

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _timeOffService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await _timeOffService.GetByIdAsync(id));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(int userId)
    {
        return Ok(await _timeOffService.GetByUserIdAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        return Created("", await _timeOffService.CreateAsync(timeOffRequest));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] TimeOffRequest timeOffRequest)
    { 
        return Ok(await _timeOffService.UpdateAsync(timeOffRequest));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        return Ok(await _timeOffService.DeleteAsync(id));
    }
}

