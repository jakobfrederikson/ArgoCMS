using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Pages.Admin.Employees
{
    public class DetailsModel : DependencyInjection_BasePageModel
	{
		public DetailsModel(
			ApplicationDbContext context,
			IAuthorizationService authorizationService,
			UserManager<Employee> userManager)
			: base(context, authorizationService, userManager)
		{
		}

		public Employee Employee { get; set; }
		public Employee ReportsTo { get; set; }
		[Display(Name = "Role")]
		public string EmployeeRole { get; set; }

		public async Task<IActionResult> OnGet(string id)
        {
            Employee? _employee = await UserManager.FindByIdAsync(id);

			if (_employee == null)
			{
				return NotFound();
			}
			Employee = _employee;

            Employee? _reportsTo = await UserManager.FindByIdAsync(Employee.ReportsToId);
            if (_reportsTo == null)
            {
				return NotFound($"Could not find user with ID of {Employee.ReportsToId}");
			}
            ReportsTo = _reportsTo;

			EmployeeRole = string.Join(",", UserManager.GetRolesAsync(Employee).Result.ToArray());

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string id)
		{
			var employee = await Context.Users.FirstOrDefaultAsync(
				m => m.Id == id);

            if (employee == null)
            {
				return NotFound();
            }

			var isAuthorized = User.IsInRole(Constants.AdministratorsRole);

			var currentUserId = UserManager.GetUserId(User);

			if (!isAuthorized
				&& currentUserId != Employee.Id)
			{
				return Forbid();
			}
			employee = Employee;
			Context.Users.Update(employee);
			await Context.SaveChangesAsync();

			return RedirectToPage("./Index");
        }
	}
}
