using Microsoft.AspNetCore.Mvc;
using TimeOffManagementAPI.Data.Model;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Web.Controllers;

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

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        return Ok(await _timeOffService.DeleteAsync(timeOffRequest));
    }
}

