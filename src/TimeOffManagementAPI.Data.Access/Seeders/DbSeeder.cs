using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Seeders;

public static class DbSeeder
{
    public static async void Seed(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        var roles = new List<Role>
        {
            new Role { Name = "Manager" },
            new Role { Name = "Employee" }
        };

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role.Name).Result)
            {
                await roleManager.CreateAsync(role);
            }
        }

        var users = new List<User>
        {
            new User { UserName = "manager", Email = "manager@manager.com", EmailConfirmed = true },
        };

        foreach (var user in users)
        {
            if (userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var result = userManager.CreateAsync(user, "manager").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Manager");
                }
            }
        }
    }
}