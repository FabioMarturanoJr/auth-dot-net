using AuthJwt.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthJwt.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Seeds(builder);
    }

    private static void Seeds(ModelBuilder builder)
    {
        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>() {  Id = 1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" }, 
            new IdentityRole<int>() { Id = 2, Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" }
        );

        builder.Entity<CustomIdentityUser>().HasData(
            new CustomIdentityUser() { 
                Id = 1, 
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com", 
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEFMqVS7Syo+Ruu6hl/OkvJvTK+/Pcw7ePy4EcccmFZQ/xTtik2Z63PIbd4lmr1ORww==",
                SecurityStamp = "DYPRIWGBDHXXKDPBWP5PWYJK3LPV7NG5",
                ConcurrencyStamp = "cebce4e7-7814-40ff-a88a-ce3d013b5a72",
            }
        );

        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int>() { UserId = 1, RoleId = 1 }
        );
    }
}
