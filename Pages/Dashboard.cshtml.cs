using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyManagementSystem.Pages
{
    public class DashboardModel : DependencyInjection_BasePageModel
    {
        public DashboardModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Job> Jobs { get; set; }
        public List<Project> Projects { get; set; }
        public Team Team { get; set; }
        public Employee ReportsTo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Could not find user with id of: {userId}");
            }

            var jobs = await Context.Jobs
                                            .Where(a => a.EmployeeID == user.Id)
                                            .ToListAsync();
            var projects = await Context.Projects
                                .Include(e => e.Employees)
                                .Where(e => e.Employees.Contains(user))
                                .ToListAsync();
            var team = Context.Teams
                        .FirstOrDefault(t => t.TeamId == user.TeamID);
            var reportsTo = await UserManager.FindByIdAsync(user.ReportsToId);

            if (jobs != null)
            {
                Jobs = jobs;
            }

            if (projects != null)
            {
                Projects = projects;
            }

            if (team != null)
            {
                Team = team;
            }

            if (reportsTo != null)
            {
                ReportsTo = reportsTo;
            }

            return Page();
        }
    }
}
