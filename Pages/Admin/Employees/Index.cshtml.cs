using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Pages.Admin.Employees
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

        public List<EmployeeViewModel> Employees { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (Context.Users != null)
            {
                var employees = await Context.Employees.ToListAsync();

                foreach (var employee in employees)
                {
                    Employees.Add(new EmployeeViewModel 
                    { 
                        Employee = employee,
                        Role = string.Join(",", UserManager.GetRolesAsync(employee).Result.ToArray())
                    });
                }
            }           
        }
    }
}
