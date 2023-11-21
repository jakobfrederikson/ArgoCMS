using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Projects
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
        public Project Project { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Projects == null)
            {
                return NotFound();
            }

            var project = await Context.Projects.FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            Project = project;

            var employeesInProject = Context.EmployeesProjects
                .Where(ep => ep.ProjectId == id)
                .Select(ep => ep.Employee);
            ViewData["OwnerID"] = new SelectList(employeesInProject, "Id", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Clear all unnecessary validation checks.
            // https://stackoverflow.com/questions/24879672/how-to-exclude-navigation-properties-validations-from-model-state-on-web-api
            ModelState.Remove($"Project.{nameof(Project.Owner)}");
            ModelState.Remove($"Project.{nameof(Project.Jobs)}");
            ModelState.Remove($"Project.{nameof(Project.Members)}");
            ModelState.Remove($"Project.{nameof(Project.EmployeeProjects)}");
            ModelState.Remove($"Project.{nameof(Project.TeamProjects)}");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(Project).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(Project.ProjectId))
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

        private bool ProjectExists(int id)
        {
            return (Context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
