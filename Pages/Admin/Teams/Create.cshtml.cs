using Microsoft.AspNetCore.Mvc;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArgoCMS.Pages.Admin.Teams
{
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
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Team.DateCreated = DateTime.Now;

            Context.Teams.Add(Team);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
