using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Contexts;

public class TimeOffManagementDBContext : IdentityDbContext<User>
{
    public DbSet<TimeOffRequest>? TimeOffRequests { get; set; }

    public TimeOffManagementDBContext(DbContextOptions<TimeOffManagementDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}