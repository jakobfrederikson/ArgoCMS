using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArgoCMS.Pages.Employees
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

        public class EmployeeViewModel
        {
            public Employee Employee { get; set; }
            public string Role { get; set; }
        }

        public List<EmployeeViewModel> Employees { get; set; }

        public IActionResult OnGet()
        {
            if (Context.Users != null)
            {
                Employees = UserManager.Users.Select(u => new EmployeeViewModel()
                {
                    Employee = u,
                    Role = string.Join(",", UserManager.GetRolesAsync(u).Result.ToArray())
                }).ToList();
            }
            else
            {
                return NotFound($"Context.Users is null");
            }

            return Page();
        }
    }
}
