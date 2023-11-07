using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Teams
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

        public List<EmployeeTeam> EmployeeTeams { get; set; }
            = new List<EmployeeTeam>();

        public async Task OnGetAsync()
        {
            var employeeId = UserManager.GetUserId(User);

            var employeeTeams = await Context.EmployeeTeams
                .Include(et => et.Team)
                .Where(et => et.EmployeeId == employeeId)
                .ToListAsync();

            if (employeeTeams != null)
            {
                EmployeeTeams = employeeTeams;
            }
        }
    }
}
