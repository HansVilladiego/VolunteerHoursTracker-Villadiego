using Microsoft.AspNetCore.Identity;

namespace VolunteerHoursTracker.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<VolunteerLog> VolunteerLogs { get; set; } = new List<VolunteerLog>();
    }
}