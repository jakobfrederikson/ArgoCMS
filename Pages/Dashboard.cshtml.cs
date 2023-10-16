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

        public IDictionary<string, JobStatus> Jobs { get; set; }
        public List<Project> Projects { get; set; }
        public Team Team { get; set; }
        public Employee ReportsTo { get; set; }
        public IList<Notice> Notices { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Could not find user with id of: {userId}");
            }

            var jobDict = InitializeDictionary();

            foreach (var job in Context.Jobs)
            {
                if (job.EmployeeID == userId)
                {
                    jobDict[job.JobStatus.ToString()]++;
                }
            }

            var projects = await Context.Projects
                                .Include(e => e.Employees)
                                .Where(e => e.Employees.Contains(user))
                                .ToListAsync();
            var team = Context.Teams
                        .FirstOrDefault(t => t.TeamId == user.TeamID);
            var reportsTo = await UserManager.FindByIdAsync(user.ReportsToId);

            var notices = Context.Notices.ToList();

            if (jobDict != null)
            {
                Jobs = jobDict;
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

            if (notices != null)
            {
                Notices = notices;
            }

            return Page();
        }

        private Dictionary<string, JobStatus> InitializeDictionary()
        {
            var jobDict = new Dictionary<string, JobStatus>();

            jobDict["Unread"] = 0;
            jobDict["Read"] = 0;
            jobDict["Working"] = 0;
            jobDict["Submitted"] = 0;
            jobDict["Completed"] = 0;


            return jobDict;
        }
    }    
}