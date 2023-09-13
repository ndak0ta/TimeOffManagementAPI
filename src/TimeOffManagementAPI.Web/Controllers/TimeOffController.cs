using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Business.Interfaces;
using MediatR;
using TimeOffManagementAPI.Business.TimeOffs.Queries;
using TimeOffManagementAPI.Business.TimeOffs.Commands;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TimeOffController : ControllerBase
{
    private readonly ILogger<TimeOffController> _logger;
    private readonly IMediator _mediator;

    public TimeOffController(ILogger<TimeOffController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _mediator.Send(new GetAllTimeOffsQuery()));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await _mediator.Send(new GetTimeOffByIdQuery(id)));
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _mediator.Send(new GetTimeOffByUserIdQuery(userId)));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        return Ok(await _mediator.Send(new GetTimeOffByUserIdQuery(userId)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(TimeOffRequest timeOffRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId); // TODO mesaj yaz

        timeOffRequest.UserId = userId;

        return Created("", await _mediator.Send(new CreateTimeOffCommand(timeOffRequest)));
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


        return Ok(await _mediator.Send(new UpdateTimeOffCommand(timeOffUpdate)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _mediator.Send(new DeleteTimeOffCommand(id));

        return Ok(new { result });
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveAsync(int id, bool isApproved)
    {
        return Ok(await _mediator.Send(new ApproveTimeOffCommand(id, isApproved)));
    }

    [HttpPost("{id:int}/cancel-request")]
    public async Task<IActionResult> CancelRequestAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _mediator.Send(new CancelTimeOffRequestCommand(id, userId)));
    }

    [HttpPost("{id:int}/approve-cancel/{isApproved:bool}")]
    public async Task<IActionResult> ApproveCancelRequestAsync(int id, bool isApproved)
    {
        return Ok(await _mediator.Send(new ApproveTimeOffCancelCommand(id, isApproved)));
    }

    [HttpPost("{id:int}/cancel-draw")]
    public async Task<IActionResult> DrawCancelRequestAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _mediator.Send(new DrawCancelRequestCommand(id, userId)));
    }
}

