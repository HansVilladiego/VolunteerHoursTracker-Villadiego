namespace VolunteerHoursTracker.ViewModels
{
    public class TopVolunteersViewModel
    {
        public Dictionary<string, double> TopVolunteers { get; set; } = new();
        public double CurrentUserTotalHours { get; set; }
        public int CurrentUserRank { get; set; }
    }
}