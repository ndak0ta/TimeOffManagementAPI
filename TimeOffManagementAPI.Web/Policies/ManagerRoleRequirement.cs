using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Web.Policies;

public class ManagerRoleRequirement : IAuthorizationRequirement { }

public class ManagerRoleHandler : AuthorizationHandler<ManagerRoleRequirement>
{
    private readonly RoleManager<Role> _roleManager;

    public ManagerRoleHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ManagerRoleRequirement requirement)
    {
        var role = await _roleManager.FindByNameAsync("Manager");

        if (context.User.IsInRole("Manager") && role.IsActive)
            context.Succeed(requirement);
        else
            context.Fail();
    }
}
