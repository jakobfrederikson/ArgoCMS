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

        public IDictionary<Job, string> Jobs { get;set; } = default!;
        public IDictionary<string, string> CreatedByIdToFullName { get; set; } = default!;
        public IDictionary<string, string> AssignedToIdToFullName { get; set; } = default!;
        public IDictionary<int, string> TeamsToFullName { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = UserManager.GetUserId(User);
            if (Context.Jobs != null)
            {
                Jobs = await Context.Jobs
                    .Where(j => j.JobStatus != JobStatus.Completed)
                    .Where(j => j.OwnerID == userId || j.EmployeeID == userId)
                    .OrderByDescending(j => (int) (j.PriorityLevel))
                    .ToDictionaryAsync
                    (
                        j => j,
                        j =>
                        {
                            switch (j.JobStatus)
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
                    );
            }

            var createdByIdToFullName = new Dictionary<string, string>();
            var assignedToIdToFullName = new Dictionary<string, string>();
            var teamsToFullName = new Dictionary<int, string>();
            foreach (var job in Jobs.Keys)
            {
                if (!createdByIdToFullName.ContainsKey(job.OwnerID))
                {
                    createdByIdToFullName[job.OwnerID] = Context.Employees
                                .FirstOrDefault(j => j.Id == job.OwnerID).FullName;
                }

                if (!assignedToIdToFullName.ContainsKey(job.EmployeeID))
                {
                    assignedToIdToFullName[job.EmployeeID] = Context.Employees
                                .FirstOrDefault(j => j.Id == job.EmployeeID).FullName;
                }

                if (!teamsToFullName.ContainsKey(job.TeamID))
                {
                    teamsToFullName[job.TeamID] = Context.Teams
                                .FirstOrDefault(t => t.TeamId == job.TeamID).TeamName;
                }
            }

            CreatedByIdToFullName = createdByIdToFullName;       
            AssignedToIdToFullName = assignedToIdToFullName;
            TeamsToFullName = teamsToFullName;
        }
    }
}
