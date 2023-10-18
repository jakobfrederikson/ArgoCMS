using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Pages.Employees
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

		public IEnumerable<SelectListItem> Roles { get; set; }
		public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Display(Name = "Team name")]
            public int TeamID { get; set; }

            [Display(Name = "Role")]
            public string EmployeeRole { get; set; }

            [Display(Name = "Personal email")]
            public string PersonalEmail { get; set; }

            [Display(Name = "Reports to")]
            public string ReportsToId { get; set; }

            [Display(Name = "User name")]
            public string UserName { get; set; }

            [Display(Name = "Employment date")]
            [DataType(DataType.Date)]
            public DateTime EmploymentDate { get; set; }
        }

        private async Task LoadAsync(Employee user)
        {
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PersonalEmail = user.PersonalEmail,
                ReportsToId = user.ReportsToId,
                UserName = user.UserName,
                EmploymentDate = user.EmploymentDate,
                TeamID = user.TeamID
            };

            Input.EmployeeRole = string.Join(",", UserManager.GetRolesAsync(user).Result.ToArray());
        }

		public async Task<IActionResult> OnGetAsync(string id)
        {
            Employee? _employee = await UserManager.FindByIdAsync(id);

            if (_employee == null)
            {
                return NotFound();
            }
            await LoadAsync(_employee);
			Employees = UserManager.Users
                            .Select(u => new SelectListItem
                            {
                                Value = u.Id,
                                Text = u.FirstName + " " + u.LastName
                            });

			Roles = new RoleStore<IdentityRole>(Context)
                                    .Roles
                                    .Select(r => new SelectListItem
                                    {
                                        Value = r.Name,
                                        Text = r.Name
                                    });
            Teams = Context.Teams.Select(
                t => new SelectListItem
                {
                    Value = t.TeamId.ToString(),
                    Text = t.TeamName
                });

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                User, _employee,
                                                AuthorizationOperations.ContactOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                User, user,
                                                AuthorizationOperations.ContactOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

			user.FirstName = Input.FirstName;
			user.LastName = Input.LastName;
			user.PersonalEmail = Input.PersonalEmail;
			user.ReportsToId = Input.ReportsToId;
			user.UserName = Input.UserName;
            user.TeamID = Input.TeamID;
            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Forbid($"UpdateAsync for : [ {result} ] failed.");
            }

            string userCurrentRole = string.Join(",", UserManager.GetRolesAsync(user).Result.ToArray());

			var result2 = await UserManager.RemoveFromRoleAsync(user, userCurrentRole);

            if (!result2.Succeeded)
            {
				return Forbid($"JAKOB: {result2}");
			}

            var result3 = await UserManager.AddToRoleAsync(user, Input.EmployeeRole);

            if (!result3.Succeeded)
            {
                return Forbid($"JAKOB: {result3}");
            }

            return RedirectToPage("./Index");
        }
    }
}
