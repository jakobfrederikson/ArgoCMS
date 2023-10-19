using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgoCMS.Pages.Admin.Employees
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : DependencyInjection_BasePageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<Employee> userManager,
            IConfiguration config)
            : base(context, authorizationService, userManager)
        {
            _configuration = config;
        }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
        

		public IActionResult OnGet()
        {
            var roleStore = new RoleStore<IdentityRole>(Context);
            var allroles = roleStore.Roles;

            Roles = allroles.Select(
                m => new SelectListItem { Value = m.Name, Text = m.Name });

            Employees = Context.Employees.Select(
                e => new SelectListItem { 
                    Text = e.FirstName + " " + e.LastName,
                    Value = e.Id });

            Teams = Context.Teams.Select(
                t => new SelectListItem { Value = t.TeamId.ToString(), Text = t.TeamName });

            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;
		[BindProperty]
		public string Role { get; set; }

		public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
				return Page();
			}

            Employee.EmailConfirmed = true;
            string defaultPassword = _configuration["SeedUserPW"];
            await UserManager.CreateAsync(Employee, defaultPassword);
            await UserManager.AddToRoleAsync(Employee, Role);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
