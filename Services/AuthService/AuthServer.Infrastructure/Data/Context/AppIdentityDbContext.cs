using AuthServer.Infrastructure.Data.Identity;
using AuthService.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Context
{
    public class AppIdentityDbContext : IdentityDbContext<User>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Action> Actions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // seed data user
            modelBuilder.Entity<IdentityRole>()
                .HasData(new IdentityRole { Name = Constants.Roles.Create, NormalizedName = Constants.Roles.Create.ToUpper() },
                new IdentityRole { Name = Constants.Roles.Review, NormalizedName = Constants.Roles.Review.ToUpper() },
                new IdentityRole { Name = Constants.Roles.Confirm, NormalizedName = Constants.Roles.Confirm.ToUpper() });
        }
    }
}
