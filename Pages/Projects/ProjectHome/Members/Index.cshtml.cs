using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Projects.ProjectHome.Members
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

        public List<Employee> ProjectMembers { get; set; } = default!;
        public int Id;

        public async Task OnGetAsync(int projectId)
        {
            Id = projectId;

            var teamMembers = await Context.EmployeesProjects
                .Where(et => et.ProjectId == projectId)
                .Select(et => et.Employee)
                .ToListAsync();

            if (teamMembers != null)
                ProjectMembers = teamMembers;
        }
    }
}
