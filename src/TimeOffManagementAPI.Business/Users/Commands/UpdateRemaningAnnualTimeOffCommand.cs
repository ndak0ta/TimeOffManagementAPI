using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record UpdateRemaningAnnualTimeOffCommand : IRequest<IdentityResult>
{
    public UpdateRemaningAnnualTimeOffCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
};

public class UpdateRemaningAnnualTimeOffCommandHandler : IRequestHandler<UpdateRemaningAnnualTimeOffCommand, IdentityResult>
{
    private readonly UserManager<User> _userManager;

    public UpdateRemaningAnnualTimeOffCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(UpdateRemaningAnnualTimeOffCommand updateRemaningAnnualTimeOffCommand, CancellationToken cancellationToken)
    {
        User userWithTimeOffs = await _userManager.Users.Include(u => u.TimeOffs).FirstOrDefaultAsync(u => u.Id == updateRemaningAnnualTimeOffCommand.UserId)
        ?? throw new NullReferenceException("User not found");

        int timeOffLeft = userWithTimeOffs.AnnualTimeOffs;

        if (userWithTimeOffs.TimeOffs == null)
            return IdentityResult.Success;

        foreach (TimeOff timeOff in userWithTimeOffs.TimeOffs)
        {
            if (timeOff.Status == TimeOffStates.Approved && timeOff.StartDate.Year == DateTime.Now.Year)
            {
                timeOffLeft -= timeOff.TotalDays;
            }
        }

        userWithTimeOffs.RemainingAnnualTimeOffs = timeOffLeft;

        return await _userManager.UpdateAsync(userWithTimeOffs);
    }
}
