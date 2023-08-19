using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.ApplicationUser.Commands;

public record DeleteUserCommand : IRequest<bool>
{
    public DeleteUserCommand(string userId)
    {
        UserId = userId;
    }

    public string? UserId { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(DeleteUserCommand deleteUserCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(deleteUserCommand.UserId);

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.IsActive = false;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}