using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, int>> GetTeamJobStatisticsAsync(int teamId)
        {
            Dictionary<string, int> teamJobStats = await _context.EmployeeTeams
                .Where(et => et.TeamId == teamId)
                .Include(et => et.Employee)
                .ThenInclude(e => e.Jobs)
                .ToDictionaryAsync(
                    et => et.Employee.FullName,
                    et => et.Employee.Jobs.Count(job => job.JobStatus == JobStatus.Completed && job.TeamID == teamId)
                );

            return teamJobStats
                .OrderByDescending(kv => kv.Value)
                .ToDictionary(kv => kv.Key, kv => kv.Value); // order the list, putting those with most completed jobs at the front
        }

        public List<string> ListOfColours(int numberOfEmployees)
        {
            string BackgroundColourOpacity = "0.9";
            string[] Colours = new string[]
            {
                $"rgba(199, 255, 69, {BackgroundColourOpacity})",
                $"rgba(134, 255, 69, {BackgroundColourOpacity})",
                $"rgba(69, 255, 122, {BackgroundColourOpacity})",
                $"rgba(69, 255, 227, {BackgroundColourOpacity})",
                $"rgba(69, 171, 255, {BackgroundColourOpacity})",
                $"rgba(69, 75, 255, {BackgroundColourOpacity})",
                $"rgba(150, 69, 255, {BackgroundColourOpacity})",
                $"rgba(230, 69, 255, {BackgroundColourOpacity})"
            };

            List<string> ListOfColours = new List<string>();

            for (int i = 0; i < numberOfEmployees; i++)
            {
                string colour = Colours[i % Colours.Length];
                ListOfColours.Add(colour);
            }

            return ListOfColours;
        }
    }
}
