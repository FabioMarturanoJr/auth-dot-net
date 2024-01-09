using AuthJwt.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthJwt.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>() {  Id = 1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" }, 
            new IdentityRole<int>() { Id = 2, Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" }
        );
    }
}
