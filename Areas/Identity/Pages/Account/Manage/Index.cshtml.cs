﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Models.JointEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArgoCMS.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : DbContextPageModel
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;

        public IndexModel(
            ApplicationDbContext context,
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager)
            : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public Employee Employee { get; set; }

        // The supervisor of the employee
        public Employee ReportsTo { get; set; }

        // The supervisor of the supervisor (boss's boss)
        public Employee ReportsToBoss { get; set; }

        public int NumOfCompletedJobs { get; set; }
        public int NumOfCompletedProjects { get; set; }
        public List<Team> Teams { get; set; }
        public string Role { get; set; }

        private async Task LoadAsync(Employee user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Employee user = new Employee();
            if (id == null)
            {
                user = await _userManager.GetUserAsync(User);
            }
            else
            {
                user = await _userManager.FindByIdAsync(id);
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Employee = user;
            ReportsTo = await _userManager.FindByIdAsync(Employee.ReportsToId);
            ReportsToBoss = await _userManager.FindByIdAsync(ReportsTo.ReportsToId);

            var role = string.Join(",", _userManager.GetRolesAsync(Employee).Result.ToArray());

            if (role != null)
            {
                Role = role;
            }

            var employeeTeams = await Context.EmployeeTeams
                .Include(et => et.Team)
                .Where(et => et.EmployeeId == Employee.Id)
                .ToListAsync();

            if (employeeTeams != null)
                Teams = employeeTeams.Select(et => et.Team).ToList();

            NumOfCompletedJobs = await GetNumOfCompletedJobs(Employee);
            NumOfCompletedProjects = await GetNumOfCompletedProjects(Employee);

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        private async Task<int> GetNumOfCompletedJobs(Employee currentEmployee)
        {
            var completedJobCount = 0;
            completedJobCount = await Context.Jobs
                .Where(j => j.AssignedEmployeeID == currentEmployee.Id)
                .CountAsync(j => j.JobStatus == JobStatus.Completed);

            return completedJobCount;
        }

        private async Task<int> GetNumOfCompletedProjects(Employee currentEmployee)
        {
            var completedProjectCount = 0;
            completedProjectCount = await Context.EmployeesProjects
                .Where(ep => ep.EmployeeId ==  currentEmployee.Id)
                .CountAsync(ep => ep.IsCompleted == true);

            return completedProjectCount;
        }
    }
}
