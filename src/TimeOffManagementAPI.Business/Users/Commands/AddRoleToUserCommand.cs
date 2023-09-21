using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Users.Queries;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record AddRoleToUserCommand : IRequest<UserInfo>
{
    public AddRoleToUserCommand(string userId, string roleName)
    {
        UserId = userId;
        RoleName = roleName;
    }

    public string? UserId { get; set; }
    public string? RoleName { get; set; }
}

public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, UserInfo>
{
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public AddRoleToUserCommandHandler(UserManager<User> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<UserInfo> Handle(AddRoleToUserCommand addRoleToUserCommand, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(addRoleToUserCommand.UserId);

        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

        await _userManager.AddToRoleAsync(user, addRoleToUserCommand.RoleName);

        return await _mediator.Send(new GetUserByIdQuery(user.Id), cancellationToken);
    }
}



