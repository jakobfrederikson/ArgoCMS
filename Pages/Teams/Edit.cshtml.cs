using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Teams
{
    public class EditModel : DependencyInjection_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Teams == null)
            {
                return NotFound();
            }

            var team = await Context.Teams.FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            Team = team;
            
            ViewData["CreatedById"] = new SelectList(Context.Employees, "Id", "FullName");

            var employeesInTeam = Context.EmployeeTeams
                .Where(et => et.TeamId == id)
                .Select(et => et.Employee)
                .ToList();
            ViewData["TeamLeaderId"] = new SelectList(employeesInTeam, "Id", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Clear all unnecessary validation checks.
            // https://stackoverflow.com/questions/24879672/how-to-exclude-navigation-properties-validations-from-model-state-on-web-api
            ModelState.Remove($"Team.{nameof(Team.CreatedBy)}");
            ModelState.Remove($"Team.{nameof(Team.Jobs)}");
            ModelState.Remove($"Team.{nameof(Team.Members)}");
            ModelState.Remove($"Team.{nameof(Team.TeamLeader)}");
            ModelState.Remove($"Team.{nameof(Team.TeamProjects)}");
            ModelState.Remove($"Team.{nameof(Team.EmployeeTeams)}");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(Team).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(Team.TeamId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TeamExists(int id)
        {
            return (Context.Teams?.Any(e => e.TeamId == id)).GetValueOrDefault();
        }
    }
}
