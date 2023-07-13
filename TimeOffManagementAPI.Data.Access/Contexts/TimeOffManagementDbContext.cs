using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Model;

namespace TimeOffManagementAPI.Data.Access.Contexts;

public class TimeOffManagementDBContext : DbContext
{
    public DbSet<TimeOffRequest>? TimeOffRequests { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<Role>? Roles { get; set; }

    public TimeOffManagementDBContext(DbContextOptions<TimeOffManagementDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entitiy =>
        {
            entitiy.HasIndex(u => u.Username).IsUnique();
            entitiy.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(r => r.Name).IsUnique();
        });
    }
}