using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var managerRole = new IdentityRole("Manager");
        var employeeRole = new IdentityRole("Employee");

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(managerRole);
            await roleManager.CreateAsync(employeeRole);
        }

        if (!userManager.Users.Any())
        {
            var user = new User
            {
                UserName = "admin",
                Email = "",
                FirstName = "Admin",
                LastName = "Admin",
                DateOfBirth = DateTime.Now,
                HireDate = DateTime.Now,
                PhoneNumber = "0000000000",
                Address = "Admin Address",
                AnnualTimeOffs = 0,
                RemainingAnnualTimeOffs = 0,
                AutomaticAnnualTimeOffIncrement = false,
                isActive = true
            };

            await userManager.CreateAsync(user, "Admin123*");
            await userManager.AddToRoleAsync(user, "Manager");
        }
    }
}
