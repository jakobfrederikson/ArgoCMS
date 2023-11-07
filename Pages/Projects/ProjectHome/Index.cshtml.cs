using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Projects.ProjectHome
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

        public Project Project { get; set; } = default!;
        public IDictionary<Notice, string> ProjectNotices { get; set; } = default!;
        public IDictionary<Employee, string> EmployeeAndRole { get; set; } = default!;

        public async Task OnGetAsync(int projectId)
        {

            var project = await AcquireProject(projectId);
            if (project != null)
            {
                Project = project;
            }

            var projectNotices = await AcquireProjectNotices();
            if (projectNotices != null)
            {
                ProjectNotices = projectNotices;
            }

            var employeeRole = await AcquireEmployeesAndRoles();
            if (employeeRole != null)
            {
                EmployeeAndRole = employeeRole;
            }

        }

        private async Task<Project> AcquireProject(int id)
        {

            return await Context.Projects.
                Include(p => p.Members)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

        }

        private async Task<Dictionary<Notice, string>> AcquireProjectNotices()
        {

            var result = await Context.Notices
                .Where(n => n.ProjectId == Project.ProjectId)
                .Include(n => n.Owner) // Eager loading to retrieve owner data
                .ToDictionaryAsync(
                    x => x,
                    x => x.Owner.FullName);

            return result;

        }

        private async Task<Dictionary<Employee, string>> AcquireEmployeesAndRoles()
        {
            var projectId = Project.ProjectId;

            var employees = await Context.EmployeesProjects
                .Where(ep => ep.ProjectId == projectId)
                .Select(et => et.Employee)
                .ToListAsync();

            var result = new Dictionary<Employee, string>();

            foreach (var employee in employees)
            {
                var roles = await UserManager.GetRolesAsync(employee);
                result[employee] = string.Join(",", roles);
            }

            return result;

        }

    }
}
