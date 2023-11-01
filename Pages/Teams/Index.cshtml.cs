using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArgoCMS.Pages.Teams
{
    public class IndexModel : DependencyInjection_BasePageModel
    {
        public IndexModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<Employee> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public Team Team { get; set; } = default!;
        public IDictionary<Job, string> TeamJobs { get; set;} = default!;
        public IDictionary<Notice, string> TeamNotices { get; set; } = default!;
        public IDictionary<Employee, string> EmployeeAndRole { get; set; } = default!;

        public async Task OnGetAsync(int? id)
        {

            var team = await AcquireTeam(id);
            if (team != null)
            {
                Team = team;
            }

            var teamJobs = await AcquireTeamsJobs();
            if (teamJobs != null)
            {
                TeamJobs = teamJobs;
            }

            var teamNotices = await AcquireTeamNotices();
            if (teamNotices != null)
            {
                TeamNotices = teamNotices;
            }

            var employeeRole = await AcquireEmployeesAndRoles();
            if (employeeRole != null)
            {
                EmployeeAndRole = employeeRole;
            }

        }

        private async Task<Team> AcquireTeam(int? id)
        {

            var query = Context.Teams.
                Include(t => t.Members)
                .Include(t => t.CreatedBy)
                .Include(t=> t.TeamLeader)
                .AsQueryable();

			if (id != null) // Looking at team for another user
			{
				var team = await query.FirstOrDefaultAsync(t => t.TeamId == id);
                return team;
			}
			else // Looking at team for current user
			{
				var userId = UserManager.GetUserId(User);
				var user = await Context.Employees.FirstOrDefaultAsync(e => e.Id == userId);
                var team = await query.FirstOrDefaultAsync(t => t.TeamId == user.TeamID);

                return team;
			}

        }

        private async Task<Dictionary<Job, string>> AcquireTeamsJobs()
        {

			var jobsList = await Context.Jobs
		        .Include(j => j.AssignedEmployee) // Eager load the related Employee
		        .Where(j => j.TeamID == Team.TeamId)
		        .ToListAsync();

			var result = new Dictionary<Job, string>();

			foreach (var job in jobsList)
			{
				result[job] = job.AssignedEmployee?.FullName;
			}

			return result;

		}

        private async Task<Dictionary<Notice, string>> AcquireTeamNotices()
        {

            var result = await Context.Notices
                .Where(n => n.TeamId == Team.TeamId)
                .Include(n => n.Owner) // Eager loading to retrieve owner data
                .ToDictionaryAsync(
                    x => x,
                    x => x.Owner.FullName);

            return result;

        }

        private async Task<Dictionary<Employee, string>> AcquireEmployeesAndRoles()
        {

            var employees = await Context.Employees
                .Where(e => e.TeamID == Team.TeamId)
                .ToListAsync();

            var result = new Dictionary<Employee, string>();

            foreach (var employee in  employees)
            {
                var roles = await UserManager.GetRolesAsync(employee);
                result[employee] = string.Join(",", roles);
            }

            return result;

		}
    }
}
