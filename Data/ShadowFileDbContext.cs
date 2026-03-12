using Microsoft.EntityFrameworkCore;
using ShadowFile.Models;

namespace ShadowFile.Data;

public class ShadowFileDbContext : DbContext
{
    public ShadowFileDbContext(DbContextOptions<ShadowFileDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, RoleName = "Admin" },
            new Role { Id = 2, RoleName = "Detective" },
            new Role { Id = 3, RoleName = "Supervisor" }
        );
    }
}