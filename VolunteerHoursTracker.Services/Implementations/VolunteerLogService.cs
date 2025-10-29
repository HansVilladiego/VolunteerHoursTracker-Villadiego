using Microsoft.EntityFrameworkCore;
using VolunteerHoursTracker.Infrastructure.Data;
using VolunteerHoursTracker.Infrastructure.Entities;
using VolunteerHoursTracker.Services.Interfaces;

namespace VolunteerHoursTracker.Services.Implementations
{
    public class VolunteerLogService : IVolunteerLogService
    {
        private readonly ApplicationDbContext _context;

        public VolunteerLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VolunteerLog>> GetAllLogsAsync()
        {
            return await _context.VolunteerLogs
                .Include(v => v.User)
                .OrderByDescending(v => v.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<VolunteerLog>> GetLogsByUserIdAsync(string userId)
        {
            return await _context.VolunteerLogs
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.Date)
                .ToListAsync();
        }

        public async Task<VolunteerLog?> GetLogByIdAsync(int id)
        {
            return await _context.VolunteerLogs
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> CreateLogAsync(VolunteerLog log)
        {
            try
            {
                _context.VolunteerLogs.Add(log);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateLogAsync(VolunteerLog log)
        {
            try
            {
                _context.VolunteerLogs.Update(log);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLogAsync(int id)
        {
            try
            {
                var log = await _context.VolunteerLogs.FindAsync(id);
                if (log == null) return false;

                _context.VolunteerLogs.Remove(log);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Dictionary<string, double>> GetTopVolunteersAsync(int count = 10)
        {
            var topVolunteers = await _context.VolunteerLogs
                .Include(v => v.User)
                .GroupBy(v => new { v.UserId, v.User!.FirstName, v.User.LastName })
                .Select(g => new
                {
                    FullName = $"{g.Key.FirstName} {g.Key.LastName}",
                    TotalHours = g.Sum(v => v.HoursWorked)
                })
                .OrderByDescending(x => x.TotalHours)
                .Take(count)
                .ToDictionaryAsync(x => x.FullName, x => x.TotalHours);

            return topVolunteers;
        }

        public async Task<double> GetTotalHoursByUserIdAsync(string userId)
        {
            return await _context.VolunteerLogs
                .Where(v => v.UserId == userId)
                .SumAsync(v => v.HoursWorked);
        }
    }
}