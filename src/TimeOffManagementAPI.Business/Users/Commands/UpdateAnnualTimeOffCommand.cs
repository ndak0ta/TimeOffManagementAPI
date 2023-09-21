using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record UpdaAnnualTimeOffCommand : IRequest<bool>;

public class UpdaAnnualTimeOffCommandHandler : IRequestHandler<UpdaAnnualTimeOffCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpdaAnnualTimeOffCommandHandler> _logger;

    public UpdaAnnualTimeOffCommandHandler(UserManager<User> userManager, ILogger<UpdaAnnualTimeOffCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdaAnnualTimeOffCommand updaAnnualTimeOffCommand, CancellationToken cancellationToken)
    {
        List<User> users = await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);

        foreach (User? user in users)
        {
            if (!user.AutomaticAnnualTimeOffIncrement)
                continue;

            decimal workYear = (DateTime.Now - user.HireDate).Days / 365;

            user.AnnualTimeOffs = Convert.ToInt32(workYear > 14 ? 26 : workYear > 5 ? 20 : workYear > 1 ? 14 : 0);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("Error while updating annual time off");
        }

        _logger.LogInformation($"Annual time off updated successfully");

        return true;
    }
}