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
            var userId = UserManager.GetUserId(User);

            var user = await Context.Employees.FirstOrDefaultAsync(e => e.Id == userId);

            var team = new Team();

            if (id != null)
            {
                team = await Context.Teams
                .Include(t => t.Employees.Where(e => e.TeamID == id))
                .FirstOrDefaultAsync(t => t.TeamId == id);
            }
            else
            {
                team = await Context.Teams
                .Include(t => t.Employees.Where(e => e.TeamID == user.TeamID))
                .FirstOrDefaultAsync(t => t.TeamId == user.TeamID);
            }
            

            if (team != null)
            {
                Team = team;
            }

            var teamJobs = await Context.Jobs
                .Where(j => j.TeamID == Team.TeamId)
                .ToDictionaryAsync(
                j => j,
                j => Context.Employees
                        .Where(e => e.Id == j.EmployeeID)
                        .Single()
                        .FullName
                );

            if (teamJobs != null)
            {
                TeamJobs = teamJobs;
            }

            var teamNotices = await Context.Notices
                .Where(n => n.TeamId == Team.TeamId)
                .ToDictionaryAsync(
                x => x, 
                x => Context.Employees.Where(
                    e => e.Id == x.OwnerID)
                .Single().FullName);

            if (teamNotices != null)
            {
                TeamNotices = teamNotices;
            }

            var employeeRole = await Context.Employees
                .Where(e => e.TeamID == Team.TeamId)
                .ToDictionaryAsync(
                e => e,
                r => string.Join(",", UserManager.GetRolesAsync(r).Result.ToArray())
                );

            if (employeeRole != null)
            {
                EmployeeAndRole = employeeRole;
            }
        }
    }
}
