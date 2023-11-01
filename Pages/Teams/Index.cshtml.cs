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
				if (id != null)
				{
					var team = await Context.Teams
					.Include(t => t.Employees.Where(e => e.TeamID == id))
					.FirstOrDefaultAsync(t => t.TeamId == id);

                    return team;
				}
				else
				{
					var userId = UserManager.GetUserId(User);
					var user = await Context.Employees.FirstOrDefaultAsync(e => e.Id == userId);
					var team = await Context.Teams
					.Include(t => t.Employees.Where(e => e.TeamID == user.TeamID))
					.FirstOrDefaultAsync(t => t.TeamId == user.TeamID);

                    return team;
				}
        }

        private async Task<Dictionary<Job, string>> AcquireTeamsJobs()
        {

                var result = await Context.Jobs
				.Where(j => j.TeamID == Team.TeamId)
				.ToDictionaryAsync(
				j => j,
				j => Context.Employees
						.Where(e => e.Id == j.EmployeeID)
						.Single()
						.FullName
				);

                return result;

        }

        private async Task<Dictionary<Notice, string>> AcquireTeamNotices()
        {

			var result = await Context.Notices
				.Where(n => n.TeamId == Team.TeamId)
				.ToDictionaryAsync(
				x => x,
				x => Context.Employees.Where(
					e => e.Id == x.OwnerID)
				.Single().FullName);

            return result;

        }

        private async Task<Dictionary<Employee, string>> AcquireEmployeesAndRoles()
        {

            var result = await Context.Employees
				.Where(e => e.TeamID == Team.TeamId)
				.ToDictionaryAsync(
				e => e,
				r => string.Join(",", UserManager.GetRolesAsync(r).Result.ToArray())
				);

            return result;

		}
    }
}
