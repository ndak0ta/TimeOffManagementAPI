using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.ApplicationUser.Commands;

public record SetAnnualTimeOffCommand : IRequest<bool>
{
    public SetAnnualTimeOffCommand(string userId, int annualTimeOffs)
    {
        UserId = userId;
        AnnualTimeOffs = annualTimeOffs;
    }

    public string UserId { get; set; }
    public int AnnualTimeOffs { get; set; }
};

public class SetAnnualTimeOffCommandHandler : IRequestHandler<SetAnnualTimeOffCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public SetAnnualTimeOffCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(SetAnnualTimeOffCommand setAnnualTimeOffCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(setAnnualTimeOffCommand.UserId)
        ?? throw new ArgumentNullException("User not found");

        user.AnnualTimeOffs = setAnnualTimeOffCommand.AnnualTimeOffs;
        user.AutomaticAnnualTimeOffIncrement = false;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return false;

        return true;
    }
}