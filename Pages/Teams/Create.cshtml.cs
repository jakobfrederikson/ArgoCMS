using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Models.JointEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArgoCMS.Pages.Teams
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : DependencyInjection_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (Team.TeamName == null ||
                Team.TeamDescription == null)
            {
                return Page();
            }

            var currentUser = await UserManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Page();
            }
            
            Team.DateCreated = DateTime.Now;
            Team.CreatedById = currentUser.Id;

            Context.Teams.Add(Team);
            await Context.SaveChangesAsync();

            // Create EmployeeTeam entry
            Context.EmployeeTeams.Add(new EmployeeTeam
            {
                EmployeeId = currentUser.Id,
                TeamId = Team.TeamId,
            });
            Context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
