using Microsoft.AspNetCore.Mvc;
using TimeOffManagementAPI.Data.Model;

namespace TimeOffManagementAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class TimeOffController : ControllerBase
{
    public TimeOffController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        throw new KeyNotFoundException("No time off requests found.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok("Hello World!");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(int userId)
    {
        return Ok("Hello World!");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        return Ok("Hello World!"); // created (201) kuıllanmayı unutma
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        return Ok("Hello World!");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        return Ok("Hello World!");
    }
}

