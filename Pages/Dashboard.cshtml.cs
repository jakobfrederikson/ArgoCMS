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

        public IDictionary<string, JobStatus> UserJobStatus { get; set; }
        public IDictionary<string, int> TeamMemberJobCompletion { get; set; }
        public List<Project> Projects { get; set; }
        public Team Team { get; set; }
        public Employee ReportsTo { get; set; }
        public IList<Notice> Notices { get; set; }
        public List<string> BackgroundColours { get; set; }

        public int TotalEmployees { get; set; }
        public int TotalTeams { get; set; }
        public int TotalJobs { get; set; }
        public int TotalProjects { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = UserManager.GetUserId(User);

            var user = await Context.Employees
                .FirstOrDefaultAsync(e => e.Id == userId);

            var teamJobDict = InitializeTeamJobsDictionary(user);
            var colours = InitializeBackgroundColours(teamJobDict.Keys.Count());
            var jobDict = InitializeUserJobsDictionary();

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

            if (teamJobDict != null)
            {
                TeamMemberJobCompletion = teamJobDict;
            }

            if (colours != null)
            {
                BackgroundColours = colours;
            }

            if (jobDict != null)
            {
                UserJobStatus = jobDict;
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

            TotalEmployees = Context.Employees.Count();
            TotalTeams = Context.Teams.Count();
            TotalJobs = Context.Jobs
                .Where(j => j.JobStatus == JobStatus.Completed)
                .Count();
            TotalProjects = Context.Projects.Count();

            return Page();
        }

        private Dictionary<string, JobStatus> InitializeUserJobsDictionary()
        {
            var jobDict = new Dictionary<string, JobStatus>();

            jobDict["Unread"] = 0;
            jobDict["Read"] = 0;
            jobDict["Working"] = 0;
            jobDict["Submitted"] = 0;
            jobDict["Completed"] = 0;


            return jobDict;
        }

        private Dictionary<string, int> InitializeTeamJobsDictionary(Employee currentUser)
        {
            Employee[] teamMembers = Context.Employees
                                    .Where(e => e.TeamID == currentUser.TeamID)
                                    .Include(e => e.Jobs.Where(j => j.JobStatus == JobStatus.Completed))
                                    .ToArray();

            var teamMemberJobsCompleted = new Dictionary<string, int>();

            foreach (Employee teamMember in teamMembers)
            {
                teamMemberJobsCompleted[teamMember.FullName] = teamMember.Jobs.Count();
            }            

            return teamMemberJobsCompleted;
        }

        private List<string> InitializeBackgroundColours(int count)
        {
            List<string> listOfColours = new List<string>();
            string opacity = "0.9";

            string[] colours = new string[]
            {
                $"rgba(199, 255, 69, {opacity})",
                $"rgba(134, 255, 69, {opacity})",
                $"rgba(69, 255, 122, {opacity})",
                $"rgba(69, 255, 227, {opacity})",
                $"rgba(69, 171, 255, {opacity})",
                $"rgba(69, 75, 255, {opacity})",
                $"rgba(150, 69, 255, {opacity})",
                $"rgba(230, 69, 255, {opacity})"
            };

            int counter = 0;
            for (int i = 0; i < count; i++)
            {
                if (counter >= colours.Length) counter = 0;

                listOfColours.Add(colours[counter]);
                counter++;
            }

            return listOfColours;
        }
    }    
}