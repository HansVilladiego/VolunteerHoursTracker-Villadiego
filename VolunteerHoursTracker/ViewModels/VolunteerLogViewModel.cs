using System.ComponentModel.DataAnnotations;

namespace VolunteerHoursTracker.ViewModels
{
    public class VolunteerLogViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Activity is required")]
        [StringLength(200, ErrorMessage = "Activity cannot exceed 200 characters")]
        [Display(Name = "Activity")]
        public string Activity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hours worked is required")]
        [Range(0.5, 24, ErrorMessage = "Hours must be between 0.5 and 24")]
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description (Optional)")]
        public string? Description { get; set; }

        [Display(Name = "Volunteer Name")]
        public string? VolunteerName { get; set; }
    }
}