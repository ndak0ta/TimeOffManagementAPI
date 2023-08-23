using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record UpdaAnnualTimeOffCommand : IRequest<bool>;

public class UpdaAnnualTimeOffCommandHandler : IRequestHandler<UpdaAnnualTimeOffCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public UpdaAnnualTimeOffCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdaAnnualTimeOffCommand updaAnnualTimeOffCommand, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            if (!user.AutomaticAnnualTimeOffIncrement)
                continue;

            decimal workYear = (DateTime.Now - user.HireDate).Days / 365;

            user.AnnualTimeOffs = Convert.ToInt32(workYear > 15 ? 26 : workYear > 5 ? 20 : workYear > 1 ? 14 : 0);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("Error while updating annual time off");
        }

        return true;
    }
}