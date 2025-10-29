using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerHoursTracker.Infrastructure.Entities
{
    public class VolunteerLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Activity { get; set; } = string.Empty;

        [Required]
        [Range(0.5, 24)]
        public double HoursWorked { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string? Description { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}