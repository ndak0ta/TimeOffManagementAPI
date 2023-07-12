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
        return Ok("Hello World!");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok("Hello World!");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TimeOffRequest timeOffRequest)
    {
        return Ok("Hello World!");
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

