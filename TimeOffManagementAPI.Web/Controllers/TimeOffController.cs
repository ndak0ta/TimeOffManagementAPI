using System.Security.Claims;
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
    private readonly ILogger<TimeOffController> _logger;
    private readonly ITimeOffService _timeOffService;

    public TimeOffController(ILogger<TimeOffController> logger, ITimeOffService timeOffService)
    {
        _logger = logger;
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
    public async Task<IActionResult> CreateAsync(TimeOffRequest timeOffRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId); // TODO mesaj yaz

        timeOffRequest.UserId = userId;

        return Created("", await _timeOffService.CreateAsync(timeOffRequest));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(TimeOffUpdate timeOffUpdate)
    {
        _logger.LogInformation("UpdateAsync called with {data}", timeOffUpdate);

        if (timeOffUpdate == null)
        {
            _logger.LogWarning("UpdateAsync: timeOffUpdate is null");
            throw new ArgumentNullException(nameof(timeOffUpdate));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        timeOffUpdate.UserId = userId;


        return Ok(await _timeOffService.UpdateAsync(timeOffUpdate));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _timeOffService.DeleteAsync(id);

        return NoContent();
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveAsync(int id, bool isApproved)
    {
        return Ok(await _timeOffService.ApproveAsync(id, isApproved));
    }

    [HttpPost("{id:int}/cancel-request")]
    public async Task<IActionResult> CancelRequestAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _timeOffService.CancelRequestAsync(id, userId));
    }

    [HttpPost("{id:int}/approve-cancel")]
    public async Task<IActionResult> ApproveCancelRequestAsync(int id)
    {
        return Ok(await _timeOffService.ApproveCancelRequestAsync(id));
    }

    [HttpPost("{id:int}/cancel-draw")]
    public async Task<IActionResult> DrawCancelRequestAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _timeOffService.DrawCancelRequestAsync(id, userId));
    }
}

