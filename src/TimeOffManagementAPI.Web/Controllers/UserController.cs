using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeOffManagementAPI.Business.Users.Commands;
using TimeOffManagementAPI.Business.Users.Queries;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByTokenAsync()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _mediator.Send(new GetUserByIdQuery(userId)));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _mediator.Send(new GetAllUsersQuery()));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery(id)));
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetByUsernameAsync(string username)
    {
        return Ok(await _mediator.Send(new GetUserByUsernameQuery(username)));
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmailAsync(string email)
    {
        return Ok(await _mediator.Send(new GetUserByEmailQuery(email)));
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] UserRegistration user)
    {
        return Created("", await _mediator.Send(new CreateUserCommand(user)));
    }

    [HttpPatch("update-contact")]
    public async Task<IActionResult> UpdateContactAsync([FromBody] UserUpdateContact user)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        user.Id = userId;

        return Ok(await _mediator.Send(new UpdateUserContactCommand(user)));
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserUpdate userUpdate)
    {
        return Ok(await _mediator.Send(new UpdateUserCommand(userUpdate)));
    }

    [Authorize(Roles = "Manager")]
    [HttpPatch("{id}/give-role/{role}")]
    public async Task<IActionResult> AddRoleToUserAsync(string id, string role)
    {
        return Ok(await _mediator.Send(new AddRoleToUserCommand(id, role)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        bool result = await _mediator.Send(new DeleteUserCommand(id));

        return Ok(new { result });
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePassword changePassword)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        changePassword.Id = userId;

        return Ok(await _mediator.Send(new ChangePasswordCommand(changePassword)));
    }

    [HttpPost("{id}/set-annual")]
    public async Task<IActionResult> SetAnnualTimeOffAsync([FromBody] string id, int newAnnualTimeOff)
    {
        return Ok(await _mediator.Send(new SetAnnualTimeOffCommand(id, newAnnualTimeOff)));
    }
}