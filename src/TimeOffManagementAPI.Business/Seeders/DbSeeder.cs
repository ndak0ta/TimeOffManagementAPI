using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Business.TimeOffs.Queries;

namespace TimeOffManagementAPI.Business.Seeders;

public static class DbSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
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
                var result = await userManager.CreateAsync(user, "manager");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Manager");
                }
            }
        }
    }

    public static async Task SeedDevelopment(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

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
            new User {
                UserName = "manager",
                FirstName = "Manager",
                LastName = "Manager",
                Email = "manager@manager.com",
                Address = "Ofis",
                PhoneNumber = "+905xxxxxxxxx",
                DateOfBirth = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now,
             },
            new User {
                UserName = "ali.ihsan.kahraman",
                FirstName = "Ali İhsan",
                LastName = "KAHRAMAN",
                Email = "alihsnkhrmn@protonmail.com",
                Address = "Yalova",
                PhoneNumber = "+905383218734",
                DateOfBirth = DateTime.TryParse("2001-11-30", out var dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-21),
                HireDate = DateTime.TryParse("2021-09-01", out var hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 26,
                RemainingAnnualTimeOffs = 26,
            },
            new User {
                UserName = "mehmet.karakus",
                FirstName = "Mehmet",
                LastName = "KARAKUŞ",
                Email = "mehmet.karakus@hotmail.com",
                Address = "İstanbul",
                PhoneNumber = "+905381231234",
                DateOfBirth = DateTime.TryParse("1996-05-12", out dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-21),
                HireDate = DateTime.TryParse("2004-03-27", out hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 20,
                RemainingAnnualTimeOffs = 20,
            },
        };

        foreach (var user in users)
        {
            if (user.Email == null)
                throw new NullReferenceException("Email is null");

            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                await userManager.CreateAsync(user, "123456");

                if (user.UserName == "manager")
                    await userManager.AddToRoleAsync(user, "Manager");
                else
                    await userManager.AddToRoleAsync(user, "Employee");
            }
        }

        var user1 = await userManager.FindByNameAsync(users[1].UserName);
        var user2 = await userManager.FindByNameAsync(users[2].UserName);

        var timeOffs = new List<TimeOffRequest>
        {
            new TimeOffRequest {
                UserId = user1.Id,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(15),
            },
            new TimeOffRequest {
                UserId = user2.Id,
                StartDate = DateTime.Now.AddDays(20),
                EndDate = DateTime.Now.AddDays(25),
            },
            new TimeOffRequest {
                UserId = user1.Id,
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(35),
            },
            new TimeOffRequest {
                UserId = user2.Id,
                StartDate = DateTime.Now.AddDays(40),
                EndDate = DateTime.Now.AddDays(45),
            },
        };

        foreach (var timeOff in timeOffs)
        {
            await mediator.Send(new CreateTimeOffCommand(timeOff));
        }


        if (await mediator.Send(new GetTimeOffByUserIdQuery(user1.Id)) is not List<TimeOffInfo> timeOffs1)
            throw new NullReferenceException("TimeOffs is null");

        await mediator.Send(new ApproveTimeOffCommand(timeOffs1[0].Id, true));

        await mediator.Send(new CancelTimeOffRequestCommand(timeOffs1[0].Id, user1.Id));
    }
}
