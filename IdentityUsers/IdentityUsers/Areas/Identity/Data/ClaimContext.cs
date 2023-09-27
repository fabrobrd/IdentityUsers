using Microsoft.EntityFrameworkCore;

namespace IdentityUsers.Areas.Identity.Data
{
    public class ClaimContext:DbContext
    {
        public ClaimContext(DbContextOptions<ClaimContext> dbContext):base(dbContext)
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
            builder.Entity<ClaimEntity>(
                entity =>
                {
                    entity.ToTable(name: "ClaimsEntity");
                });
        }
        public DbSet<ClaimEntity> ClaimsEntity { get; set; }   
    }
}
