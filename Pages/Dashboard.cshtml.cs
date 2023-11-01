using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArgoCMS.Pages
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

        public Dictionary<string, int> UserJobStatus { get; set; }
        public Dictionary<string, int> TeamMemberJobCompletion { get; set; }
        public List<Project> Projects { get; set; }
        public List<Team> Teams { get; set; }        
        public List<Notice> Notices { get; set; }
        public List<string> BackgroundColours { get; set; }


        public int TotalEmployees { get; set; }
        public int TotalTeams { get; set; }
        public int TotalJobsCompleted { get; set; }
        public int TotalProjectsCompleted { get; set; }


        private const string BackgroundColourOpacity = "0.9";
        private readonly string[] Colours = new string[]
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

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = UserManager.GetUserId(User);

            var currentUser = await Context.Employees
                .FirstOrDefaultAsync(e => e.Id == userId);

            if (currentUser != null)
            {
                TeamMemberJobCompletion = await GetTeamMemberJobCompletion(currentUser);
                UserJobStatus = GetUserJobStatus(currentUser);

                Projects = await GetProjects(currentUser);
                Teams = await GetTeams(currentUser);
                Notices = await GetNotices();
            }
            else
            {
                return RedirectToPage("/Error");
            }

            BackgroundColours = InitializeBackgroundColours(TeamMemberJobCompletion.Keys.Count());

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

        private async Task<Dictionary<string, int>> GetTeamMemberJobCompletion(Employee currentUser)
        {
            return await Context.Employees
                    .Include(e => e.Jobs)
                    .Where(e => e.TeamID == currentUser.TeamID)
                    .ToDictionaryAsync(
                        teamMember => teamMember.FullName,
                        teamMember => teamMember.Jobs.Count(j => j.JobStatus == JobStatus.Completed));
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

        private List<string> InitializeBackgroundColours(int count)
        {
            List<string> listOfColours = new List<string>();         

            for (int i = 0; i < count; i++)
            {
                string colour = Colours[i % Colours.Length];
                listOfColours.Add(colour);
            }

            return listOfColours;
        }

        private async Task<List<Project>> GetProjects(Employee currentUser)
        {
            return await Context.Projects
                .Include(p => p.TeamProjects)
                .Where(p => p.EmployeeProjects.Any(ep => ep.Employee == currentUser))
                .ToListAsync();
        }

        private async Task<List<Team>> GetTeams(Employee currentUser)
        {
            return await Context.Teams
                .Include(t => t.TeamProjects)
                .Where(t => t.Members.Any(m => m.Id == currentUser.Id))
                .ToListAsync();
        }

        private async Task<List<Notice>> GetNotices()
        {
            return await Context.Notices
                .Where(n => n.PublicityStatus == PublicityStatus.Everyone)
                .ToListAsync();
        }
    }    
}