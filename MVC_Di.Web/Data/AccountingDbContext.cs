using Microsoft.EntityFrameworkCore;
using MVC_Di.Models;

namespace MVC_Di.Data;

public class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AccountRecord> AccountRecords => Set<AccountRecord>();
    public DbSet<UserCategory> UserCategories => Set<UserCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Username)
            .IsUnique();

        modelBuilder.Entity<UserCategory>()
            .HasIndex(category => new { category.AppUserId, category.Name })
            .IsUnique();

        modelBuilder.Entity<AccountRecord>()
            .Property(record => record.Amount)
            .HasColumnType("decimal(10,2)");
    }
}
