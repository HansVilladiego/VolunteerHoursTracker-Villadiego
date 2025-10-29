using VolunteerHoursTracker.Infrastructure.Entities;

namespace VolunteerHoursTracker.Services.Interfaces
{
    public interface IVolunteerLogService
    {
        Task<IEnumerable<VolunteerLog>> GetAllLogsAsync();
        Task<IEnumerable<VolunteerLog>> GetLogsByUserIdAsync(string userId);
        Task<VolunteerLog?> GetLogByIdAsync(int id);
        Task<bool> CreateLogAsync(VolunteerLog log);
        Task<bool> UpdateLogAsync(VolunteerLog log);
        Task<bool> DeleteLogAsync(int id);
        Task<Dictionary<string, double>> GetTopVolunteersAsync(int count = 10);
        Task<double> GetTotalHoursByUserIdAsync(string userId);
    }
}