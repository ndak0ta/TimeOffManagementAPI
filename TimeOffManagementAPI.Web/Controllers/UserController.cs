using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Business.ApplicationUser.Queries;
using MediatR;
using TimeOffManagementAPI.Business.ApplicationUser.Commands;

namespace TimeOffManagementAPI.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMediator _mediator;

    public UserController(IUserService userService, IMediator mediator)
    {
        _userService = userService;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByTokenAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
        return Created("", await _mediator.Send(new CreateUserCommand(user)));
    }

    [HttpPatch("update-contact")]
    public async Task<IActionResult> UpdateContactAsync([FromBody] UserUpdateContact user)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        user.Id = userId;

        return Ok(await _userService.UpdateContactAsync(user));
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserUpdate userUpdate)
    {
        return Ok(await _mediator.Send(new UpdateUserCommand(userUpdate)));
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("{id}/give-role")]
    public async Task<IActionResult> AddRoleToUserAsync(string id, [FromBody] string role)
    {
        return Ok(await _mediator.Send(new AddRoleToUserCommand(id, role)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        return Ok(await _mediator.Send(new DeleteUserCommand(id)));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePassword changePassword)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

    [HttpGet("role")]
    public async Task<IActionResult> GetRoleAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new ArgumentNullException(userId);

        return Ok(await _userService.GetRoleAsync(userId));
    }
}