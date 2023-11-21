using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public List<EmployeeViewModel> Employees { get; set; } = new();

        public async Task OnGetAsync()
        {
            var usersList = await UserManager.Users.ToListAsync();
            Employees = usersList.Select(user => new EmployeeViewModel
            {
                Employee = user,
                Role = string.Join(",", UserManager.GetRolesAsync(user).Result.ToArray())
            })
            .OrderBy(e => e.Role)
            .ToList();
        }
    }
}
