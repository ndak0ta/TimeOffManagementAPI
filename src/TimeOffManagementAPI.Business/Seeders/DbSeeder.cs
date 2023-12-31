using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Business.TimeOffs.Queries;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Seeders;

public static class DbSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        RoleManager<Role> roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        List<Role> roles = new()
        {
            new() { Name = "Manager" },
            new() { Name = "Employee" }
        };

        foreach (Role role in roles)
        {
            if (!roleManager.RoleExistsAsync(role.Name).Result)
            {
                await roleManager.CreateAsync(role);
            }
        }

        List<User> users = new()
        {
            new() { UserName = "manager", Email = "manager@manager.com", EmailConfirmed = true },
        };

        foreach (User user in users)
        {
            if (userManager.FindByNameAsync(user.UserName).Result == null)
            {
                IdentityResult result = await userManager.CreateAsync(user, "manager");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Manager");
                }
            }
        }
    }

    public static async Task SeedDevelopment(IServiceProvider serviceProvider)
    {
        UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        RoleManager<Role> roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        IMediator mediator = serviceProvider.GetRequiredService<IMediator>();

        List<Role> roles = new()
        {
            new() { Name = "Manager" },
            new() { Name = "Employee" }
        };

        foreach (Role role in roles)
        {
            if (!roleManager.RoleExistsAsync(role.Name).Result)
            {
                await roleManager.CreateAsync(role);
            }
        }

        List<User> users = new()
        {
            new() {
                UserName = "manager",
                FirstName = "Manager",
                LastName = "Manager",
                Email = "manager@manager.com",
                Address = "Ofis",
                PhoneNumber = "+905xxxxxxxxx",
                DateOfBirth = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now,
             },
            new() {
                UserName = "ali.ihsan.kahraman",
                FirstName = "Ali İhsan",
                LastName = "KAHRAMAN",
                Email = "alihsnkhrmn@protonmail.com",
                Address = "Yalova",
                PhoneNumber = "+905383218734",
                DateOfBirth = DateTime.TryParse("2001-11-30", out DateTime dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-21),
                HireDate = DateTime.TryParse("2021-09-01", out DateTime hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 26,
                RemainingAnnualTimeOffs = 26,
            },
            new() {
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
            new() {
                UserName = "ahmet.oz",
                FirstName = "Ahmet",
                LastName = "ÖZ",
                Email = "ahmetoz@gmail.com",
                Address = "İstanbul",
                PhoneNumber = "+905381231234",
                DateOfBirth = DateTime.TryParse("1989-08-22", out dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-33),
                HireDate = DateTime.TryParse("2010-01-01", out hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 20,
                RemainingAnnualTimeOffs = 20,
            },
            new() {
                UserName = "mehmet.oz",
                FirstName = "Mehmet",
                LastName = "ÖZ",
                Email = "mehmetoz@gmail.com",
                Address = "İzmir",
                PhoneNumber = "+905381231234",
                DateOfBirth = DateTime.TryParse("1990-01-01", out dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-31),
                HireDate = DateTime.TryParse("2014-04-09", out hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 20,
                RemainingAnnualTimeOffs = 20,
            },
            new() {
                UserName = "ayse.kaya",
                FirstName = "Ayşe",
                LastName = "KAYA",
                Email = "aysekaya@hotmail.com",
                Address = "Kocaeli",
                PhoneNumber = "+905381231234",
                DateOfBirth = DateTime.TryParse("1995-12-31", out dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-26),
                HireDate = DateTime.TryParse("2018-01-01", out hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 20,
                RemainingAnnualTimeOffs = 20,
            },
            new() {
                UserName = "fatma.saracoglu",
                FirstName = "Fatma",
                LastName = "SARAÇOĞLU",
                Email = "srcoglu.fatma@yahoo.com",
                Address = "İstanbul",
                PhoneNumber = "+905381231234",
                DateOfBirth = DateTime.TryParse("1998-01-01", out dateOfBirth) ? dateOfBirth : DateTime.Now.AddYears(-23),
                HireDate = DateTime.TryParse("2021-01-01", out hireDate) ? hireDate : DateTime.Now,
                AnnualTimeOffs = 20,
                RemainingAnnualTimeOffs = 20,
            },
        };

        foreach (User user in users)
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

        User user1 = await userManager.FindByNameAsync(users[1].UserName);
        User user2 = await userManager.FindByNameAsync(users[2].UserName);

        List<TimeOffRequest> timeOffs = new()
        {
            new() {
                UserId = user1.Id,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(15),
            },
            new() {
                UserId = user2.Id,
                StartDate = DateTime.Now.AddDays(20),
                EndDate = DateTime.Now.AddDays(25),
            },
            new() {
                UserId = user1.Id,
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(35),
            },
            new() {
                UserId = user2.Id,
                StartDate = DateTime.Now.AddDays(40),
                EndDate = DateTime.Now.AddDays(45),
            },
        };

        foreach (TimeOffRequest timeOff in timeOffs)
        {
            await mediator.Send(new CreateTimeOffCommand(timeOff));
        }


        if (await mediator.Send(new GetTimeOffByUserIdQuery(user1.Id)) is not List<TimeOffInfo> timeOffs1)
            throw new NullReferenceException("TimeOffs is null");

        await mediator.Send(new ApproveTimeOffCommand(timeOffs1[0].Id, true));

        await mediator.Send(new CancelTimeOffRequestCommand(timeOffs1[0].Id, user1.Id));
    }
}
