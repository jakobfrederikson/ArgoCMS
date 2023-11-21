using ArgoCMS.Models;
using ArgoCMS.Data;
using ArgoCMS.Authorization;
using Microsoft.Extensions.Configuration;
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
            UserManager<Employee> userManager,
            IConfiguration configuration)
            : base(context, authorizationService, userManager)
        {
            Configuration = configuration;
        }

        private readonly IConfiguration Configuration;

        public PaginatedList<Job> Jobs { get; set; }

        // Sorting filters
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string DueDateSort { get; set; }
        public string JobStatusSort { get; set; }
        public string PriorityLevelSort { get; set; }
        public string CreatedBySort { get; set; }
        public string AssignedToSort { get; set; }
        public string TeamSort { get; set; }
        public string ProjectSort { get; set;}
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            var userId = UserManager.GetUserId(User);

            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            DueDateSort = sortOrder == "DueDate" ? "dueDate_desc" : "DueDate";
            JobStatusSort = sortOrder == "JobStatus" ? "jobStatus_desc" : "JobStatus";
            PriorityLevelSort = sortOrder == "PriorityLevel" ? "priorityLevel_desc" : "PriorityLevel";
            CreatedBySort = sortOrder == "CreatedBy" ? "createdBy_desc" : "CreatedBy";
            AssignedToSort = sortOrder == "AssignedTo" ? "assignedTo_desc" : "AssignedTo";
            TeamSort = sortOrder == "Team" ? "team_desc" : "Team";
            ProjectSort = sortOrder == "Project" ? "project_desc" : "Project";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Job> jobsIQ = Context.Jobs
                    .Where(j => j.OwnerID == userId || j.AssignedEmployeeID == userId)
                    .Include(j => j.Owner)
                    .Include(j => j.AssignedEmployee)
                    .Include(j => j.Team)
                    .Include(j => j.Project);

            if (!String.IsNullOrEmpty(searchString))
            {
                jobsIQ = jobsIQ.Where(j => j.JobName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.JobName);
                    break;
                case "Date":
                    jobsIQ = jobsIQ.OrderBy(j => j.DateCreated);
                    break;
                case "date_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.DateCreated);
                    break;
                case "DueDate":
                    jobsIQ = jobsIQ.OrderBy(j => j.DueDate);
                    break;
                case "dueDate_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.DueDate);
                    break;
                case "JobStatus":
                    jobsIQ = jobsIQ.OrderBy(j => j.JobStatus);
                    break;
                case "jobStatus_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.JobStatus);
                    break;
                case "PriorityLevel":
                    jobsIQ = jobsIQ.OrderBy(j => j.PriorityLevel);
                    break;
                case "priorityLevel_dec":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.PriorityLevel);
                    break;
                case "CreatedBy":
                    jobsIQ = jobsIQ.OrderBy(j => j.OwnerID);
                    break;
                case "createdBy_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.OwnerID);
                    break;
                case "AssignedTo":
                    jobsIQ = jobsIQ.OrderBy(j => j.AssignedEmployeeID);
                    break;
                case "assignedTo_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.AssignedEmployeeID);
                    break;
                case "Team":
                    jobsIQ = jobsIQ.OrderBy(j => j.TeamID);
                    break;
                case "team_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.TeamID);
                    break;
                case "Project":
                    jobsIQ = jobsIQ.OrderBy(j => j.ProjectID);
                    break;
                case "project_desc":
                    jobsIQ = jobsIQ.OrderByDescending(j => j.ProjectID);
                    break;
                default:
                    jobsIQ = jobsIQ.OrderByDescending(j => j.PriorityLevel);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Jobs = await PaginatedList<Job>.CreateAsync(
                jobsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
