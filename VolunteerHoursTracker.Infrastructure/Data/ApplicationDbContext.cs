using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolunteerHoursTracker.Infrastructure.Entities;

namespace VolunteerHoursTracker.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VolunteerLog> VolunteerLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<VolunteerLog>()
                .HasOne(v => v.User)
                .WithMany(u => u.VolunteerLogs)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}