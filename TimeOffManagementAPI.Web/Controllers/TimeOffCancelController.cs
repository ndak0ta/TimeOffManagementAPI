using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TimeOffCancelController : ControllerBase
{
    private readonly ITimeOffCancelService _timeOffCancelService;
    public TimeOffCancelController(ITimeOffCancelService timeOffCancelService)
    {
        _timeOffCancelService = timeOffCancelService;
    }

    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _timeOffCancelService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByTimeOffIdAsync(int id)
    {
        return Ok(await _timeOffCancelService.GetByTimeOffIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(TimeOffCancelRequest timeOffCancelRequest)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId); // TODO mesaj yaz

        timeOffCancelRequest.UserId = userId;

        return Ok(await _timeOffCancelService.CreateAsync(timeOffCancelRequest));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId); // TODO mesaj yaz

        await _timeOffCancelService.DeleteAsync(id, userId);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ApproveAsync(int id, bool isApproved)
    {
        return Ok(await _timeOffCancelService.ApproveAsync(id, isApproved));
    }
}