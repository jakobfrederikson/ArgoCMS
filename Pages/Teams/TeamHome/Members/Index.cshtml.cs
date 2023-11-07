using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Teams.TeamHome.Members
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

        public List<Employee> TeamMembers { get; set; } = default!;
        public int Id;

        public async Task OnGetAsync(int teamId)
        {
            Id = teamId;

            var teamMembers = await Context.EmployeeTeams
                .Where(et => et.TeamId == teamId)
                .Select(et => et.Employee)
                .ToListAsync();

            if (teamMembers != null)
                TeamMembers = teamMembers;
        }
    }
}
