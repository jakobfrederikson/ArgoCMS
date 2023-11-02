using ArgoCMS.Models;
using ArgoCMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArgoCMS.Pages.Jobs
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

        public Dictionary<Job, string> Jobs { get;set; } = default!;
        public Dictionary<string, string> CreatedByIdToFullName { get; set; } = default!;
        public Dictionary<string, string> AssignedToIdToFullName { get; set; } = default!;
        public Dictionary<int, string> TeamsToFullName { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = UserManager.GetUserId(User);
            if (Context.Jobs != null)
            {
                var jobsQuery = Context.Jobs
                    .Where(j => j.JobStatus != JobStatus.Completed)
                    .Where(j => j.OwnerID == userId || j.AssignedEmployeeID == userId)
                    .OrderByDescending(j => (int)j.PriorityLevel);

                Jobs = await GetJobsWithStatusCssClassAsync(jobsQuery);
            }

            var createdByIds = Jobs.Keys.Select(job => job.OwnerID).Distinct();
            var assignedToIds = Jobs.Keys.Select(job => job.AssignedEmployeeID).Distinct();
            var teamIds = Jobs.Keys.Select(job => job.TeamID).Distinct();

            var employees = await Context.Employees
                .Where(e => createdByIds.Contains(e.Id) || assignedToIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id, e => e.FullName);

            var teams = await Context.Teams
                .Where(t => teamIds.Contains(t.TeamId))
                .ToDictionaryAsync(t => t.TeamId, t => t.TeamName);

            var createdByIdToFullName = new Dictionary<string, string>();
            var assignedToIdToFullName = new Dictionary<string, string>();
            var teamsToFullName = new Dictionary<int, string>();

            foreach (var job in Jobs.Keys)
            {
                if (employees.ContainsKey(job.OwnerID))
                {
                    createdByIdToFullName[job.OwnerID] = employees[job.OwnerID];
                }

                if (employees.ContainsKey(job.AssignedEmployeeID))
                {
                    assignedToIdToFullName[job.AssignedEmployeeID] = employees[job.AssignedEmployeeID];
                }

                if (teams.ContainsKey(job.TeamID))
                {
                    teamsToFullName[job.TeamID] = teams[job.TeamID];
                }
            }

            CreatedByIdToFullName = createdByIdToFullName;
            AssignedToIdToFullName = assignedToIdToFullName;
            TeamsToFullName = teamsToFullName;
        }

        private async Task<Dictionary<Job, string>> GetJobsWithStatusCssClassAsync(IQueryable<Job> query)
        {
            return await query
                .ToDictionaryAsync(
                    j => j,
                    GetJobStatusCssClass
                );
        }

        private string GetJobStatusCssClass(Job job)
        {
            switch (job.JobStatus)
            {
                case JobStatus.Unread:
                    return "table-primary";
                case JobStatus.Read:
                    return "table-danger";
                case JobStatus.Working:
                    return "table-warning";
                case JobStatus.Submitted:
                    return "table-success";
                default:
                    return "";
            }
        }
    }
}
