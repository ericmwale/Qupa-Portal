using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PremFEPost.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<TranDetails> TranDetails { get; set; }
        public DbSet<BatchDetails> BatchDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("Admin");
            admin.NormalizedName = "Admin";
            var approver = new IdentityRole("Approver");
            approver.NormalizedName = "Approver";
            var initiator = new IdentityRole("Initiator");
            initiator.NormalizedName = "Initiator";
            builder.Entity<IdentityRole>().HasData(admin, approver, initiator);
        }
    }


}
