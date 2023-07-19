using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Web.Policies;

public class ManagerRoleRequirement : IAuthorizationRequirement { }

public class ManagerRoleHandler : AuthorizationHandler<ManagerRoleRequirement>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public ManagerRoleHandler(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ManagerRoleRequirement requirement)
    {
        var role = await _roleManager.FindByNameAsync("Manager");
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (await _userManager.IsInRoleAsync(user, "Manager") && role.IsActive)
            context.Succeed(requirement);
        else
            context.Fail();
    }
}
