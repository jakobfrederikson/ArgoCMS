using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.api.Dashboard
{
    public class GetTeamJobStatisticsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public GetTeamJobStatisticsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int teamId)
        {
			var teamJobStatistics = _context.EmployeeTeams
				.Where(et => et.TeamId == teamId)
				.Include(et => et.Employee)
				.ThenInclude(e => e.Jobs)
				.ToDictionary(
					et => et.Employee.FullName,
					et => et.Employee.Jobs.Count(job => job.JobStatus == JobStatus.Completed && job.TeamID == teamId)
				);

			return new JsonResult(teamJobStatistics);
		}
    }
}
