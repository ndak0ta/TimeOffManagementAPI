using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Contexts;

public class TimeOffManagementDBContext : IdentityDbContext<User, Role, string>
{
    public DbSet<TimeOff>? TimeOffs { get; set; }

    public TimeOffManagementDBContext(DbContextOptions<TimeOffManagementDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TODO seed işlemini test et

        /* List<Role> roles = new List<Role>
        {
            new Role { Name = "Manager", NormalizedName = "MANAGER"},
            new Role { Name = "Employee", NormalizedName = "EMPLOYEE"}
        };

        modelBuilder.Entity<Role>().HasData(roles);

        List<User> users = new List<User>
        {
            new User { UserName = "admin", Email = "", FirstName = "Admin", LastName = "Admin", DateOfBirth = DateTime.Now, HireDate = DateTime.Now, PhoneNumber = "0000000000", Address = "Admin Address", AnnualTimeOffs = 0, RemainingAnnualTimeOffs = 0, AutomaticAnnualTimeOffIncrement = false, isActive = true },
            new User { UserName = "employee", Email = "", FirstName = "Employee", LastName = "Employee", DateOfBirth = DateTime.Now, HireDate = DateTime.Now, PhoneNumber = "0000000000", Address = "Employee Address", AnnualTimeOffs = 0, RemainingAnnualTimeOffs = 0, AutomaticAnnualTimeOffIncrement = false, isActive = true}
        };

        var passwordHasher = new PasswordHasher<User>();

        foreach (var user in users)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, "123456");
            user.SecurityStamp = Guid.NewGuid().ToString();
        }

        modelBuilder.Entity<User>().HasData(users);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = roles[0].Id, UserId = users[0].Id },
            new IdentityUserRole<string> { RoleId = roles[1].Id, UserId = users[1].Id }
        ); */
    }
}