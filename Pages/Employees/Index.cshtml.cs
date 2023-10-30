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

        public List<EmployeeViewModel> Employees { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var usersList = await UserManager.Users.ToListAsync();

            var employeeRoleList = new List<EmployeeViewModel>();
            foreach (var user in usersList)
            {
                
                employeeRoleList.Add(new EmployeeViewModel()
                {
                    Employee = user,
                    Role = string.Join(",", UserManager.GetRolesAsync(user).Result.ToArray())
                });
            }

            Employees = employeeRoleList;

            //if (Context.Users != null)
            //{
            //    Employees = await UserManager.Users.Select(u => new EmployeeViewModel()
            //    {
            //        Employee = u,
            //        Role = string.Join(",", UserManager.GetRolesAsync(u).Result.ToArray())
            //    }).ToListAsync();
            //}
        }
    }
}
