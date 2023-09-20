using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record ChangePasswordCommand : IRequest<bool>
{
    public ChangePasswordCommand(UserChangePassword userChangePassword)
    {
        UserChangePassword = userChangePassword;
    }

    public UserChangePassword? UserChangePassword { get; set; }
};

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ChangePasswordCommand changePasswordCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(changePasswordCommand.UserChangePassword?.Id)
        ?? throw new ArgumentNullException(nameof(changePasswordCommand.UserChangePassword), "User not found");

        if (!await _userManager.CheckPasswordAsync(user, changePasswordCommand.UserChangePassword?.OldPassword))
            throw new UnauthorizedAccessException("Password is incorrect");

        var result = await _userManager.ChangePasswordAsync(
            user,
            changePasswordCommand.UserChangePassword?.OldPassword,
            changePasswordCommand.UserChangePassword?.NewPassword
        );

        if (!result.Succeeded)
            return false;

        return true;
    }
}
