﻿using IdentityUsers.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityUsers.Areas.Identity.Data;

public class IdentityUsersContext : IdentityDbContext<ApplicationUser>
{
    public IdentityUsersContext(DbContextOptions<IdentityUsersContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        RenameIdentityTables(builder);
    }
    protected void RenameIdentityTables(ModelBuilder builder)
    {

        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Security");
        builder.Entity<ApplicationUser>(
            entity =>
            {
                entity.ToTable(name: "Users");
            });
        builder.Entity<IdentityRole>(
            entity =>
            {
                entity.ToTable(name: "Roles");
            });
        builder.Entity<IdentityUserRole<string>>(
            entity =>
            {
                entity.ToTable(name: "UserRoles");
            });
        builder.Entity<IdentityUserClaim<string>>(
            entity =>
            {
                entity.ToTable(name: "UserClaims");
            });
        builder.Entity<IdentityUserLogin<string>>(
            entity =>
            {
                entity.ToTable(name: "UserLogins");
            });
        builder.Entity<IdentityRoleClaim<string>>(
            entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });
        builder.Entity<IdentityUserToken<string>>(
            entity =>
            {
                entity.ToTable(name: "UserTokens");
            });

    }
}
