using Microsoft.AspNetCore.Mvc;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Business.Interfaces;
using MediatR;
using TimeOffManagementAPI.Business.Auth.Commands;

namespace TimeOffManagementAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public AuthController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] UserLogin userLogin)
    {
        return Ok(await _mediator.Send(new LoginCommand(userLogin)));
    }

    /* [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegistration userRegistration)
    {
        return Created("", await _authService.RegisterAsync(userRegistration));
    } */

    /*  [AllowAnonymous]
     [HttpPost("refresh")]
     public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenDto refreshToken)
     {
         return Ok(await _authService.RefreshAsync(refreshToken));
     }

     [Authorize]
     [HttpPost("revoke")]
     public async Task<IActionResult> RevokeAsync([FromBody] RefreshTokenDto refreshToken)
     {
         await _authService.RevokeAsync(refreshToken);

         return NoContent();
     } */
}