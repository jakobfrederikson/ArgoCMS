using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgoCMS.Pages.Admin.Jobs
{
    [Authorize(Roles = "Administrators")]
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
        public Job Job { get; set; } = default!;
        public IEnumerable<SelectListItem> Employees { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Jobs == null)
            {
                return NotFound();
            }

            var job =  await Context.Jobs.FirstOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }
            Job = job;

            Employees = Context.Employees.Select(
                e => new SelectListItem { Text = e.FullName, Value = e.Id });

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var job = await Context.Jobs
                        .AsNoTracking()
                        .FirstOrDefaultAsync
                            (j => j.JobId == Job.JobId);

            Job.TeamID = Context.Employees
                .Where(e => e.Id == Job.EmployeeID)
                .Select(e => e.TeamID)
                .Single();

            Context.Attach(Job).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(Job.JobId))
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

        private bool JobExists(int id)
        {
          return Context.Jobs.Any(e => e.JobId == id);
        }
    }
}
