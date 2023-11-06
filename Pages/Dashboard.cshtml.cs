using ArgoCMS.Data;
using ArgoCMS.Hubs;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArgoCMS.Pages
{
    public class DashboardModel : DependencyInjection_BasePageModel
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        public DashboardModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            IHubContext<NotificationHub> notificationHub,
            UserManager<Employee> userManager)
            : base(context, authorizationService, userManager)
        {
            _notificationHub = notificationHub; 
        }

        public int theId { get; set; }
        public Employee CurrentUser { get; set; }
        public Dictionary<string, int> UserJobStatus { get; set; }
        public Dictionary<string, int> TeamMemberJobCompletion { get; set; }
        public Dictionary<string, int> Test { get; set; }
        public List<Project> Projects { get; set; }
        public List<Team> Teams { get; set; }        
        public List<Notice> Notices { get; set; }
        public List<string> BackgroundColours { get; set; }


        public int TotalEmployees { get; set; }
        public int TotalTeams { get; set; }
        public int TotalJobsCompleted { get; set; }
        public int TotalProjectsCompleted { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var userId = UserManager.GetUserId(User);

            var currentUser = await Context.Employees
                .Include(e => e.EmployeeTeams)
                .ThenInclude(et => et.Team)
                .Include(e => e.EmployeeProjects)
                .FirstOrDefaultAsync(e => e.Id == userId);
                        
            if (currentUser != null)
            {
                UserJobStatus = GetUserJobStatus(currentUser);
                Projects = await GetProjects(currentUser);
                Teams = await GetTeams(currentUser);
                Notices = await GetNotices();

                CurrentUser = currentUser;
            }
            else
            {
                return RedirectToPage("/Error");
            }

            TotalEmployees = Context.Employees.Count();
            TotalTeams = Context.Teams.Count();
            TotalJobsCompleted = Context.Jobs
                .Where(j => j.JobStatus == JobStatus.Completed)
                .Count();
            TotalProjectsCompleted = Context.Projects
                .Where(p => p.ProjectStatus == ProjectStatus.Completed)
                .Count();

            return Page();
        }

        private Dictionary<string, int> GetUserJobStatus(Employee currentUser)
        {
            var jobDict = InitializeUserJobsDictionary();

            jobDict["Unread"] = GetUserJobCountByStatus(currentUser, JobStatus.Unread);
            jobDict["Read"] = GetUserJobCountByStatus(currentUser, JobStatus.Read);
            jobDict["Working"] = GetUserJobCountByStatus(currentUser, JobStatus.Working);
            jobDict["Submitted"] = GetUserJobCountByStatus(currentUser, JobStatus.Submitted);
            jobDict["Completed"] = GetUserJobCountByStatus(currentUser, JobStatus.Completed);

            return jobDict;
        }

        private int GetUserJobCountByStatus(Employee currentUser, JobStatus status)
        {
            return Context.Jobs.Count(job => job.AssignedEmployeeID == currentUser.Id && job.JobStatus == status);
        }

        private Dictionary<string, int> InitializeUserJobsDictionary()
        {
            var jobDict = new Dictionary<string, int>();

            jobDict["Unread"] = 0;
            jobDict["Read"] = 0;
            jobDict["Working"] = 0;
            jobDict["Submitted"] = 0;
            jobDict["Completed"] = 0;

            return jobDict;
        }   

        private async Task<List<Project>> GetProjects(Employee currentUser)
        {
            return await Context.Projects
                .Include(p => p.TeamProjects)
                .ThenInclude(tp => tp.Team)
                .Where(p => p.EmployeeProjects.Any(ep => ep.Employee == currentUser))
                .ToListAsync();
        }

        private async Task<List<Team>> GetTeams(Employee currentUser)
        {
            return await Context.Teams
                .Include(t => t.TeamProjects)
                .ThenInclude(p => p.Project)
                .Where(t => t.Members.Any(m => m.Id == currentUser.Id))
                .ToListAsync();
        }

        private async Task<List<Notice>> GetNotices()
        {
            return await Context.Notices
                .Include(n => n.Owner)
                .Where(n => n.PublicityStatus == PublicityStatus.Everyone)
                .ToListAsync();
        }

        public Dictionary<string, int> GetTeamJobStatistics(int teamId)
        {
            var teamJobStatistics = Context.EmployeeTeams
                .Where(et => et.TeamId == teamId)
                .Include(et => et.Employee)
                .ThenInclude(e => e.Jobs)
                .ToDictionary(
                    et => et.Employee.FullName,
                    et => et.Employee.Jobs.Count(job => job.JobStatus == JobStatus.Completed)
                );
            return teamJobStatistics;
        }
    }
}