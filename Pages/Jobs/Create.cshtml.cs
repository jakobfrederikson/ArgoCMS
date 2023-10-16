﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models;
using CompanyManagementSystem.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ArgoCMS.Pages.Jobs
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
            Job = new Job();
            Job.DueDate = DateTime.Now;
            Employees = Context.Employees.Select(
                    e => new SelectListItem { Text = e.FullName, Value = e.Id });

            var currId = UserManager.GetUserId(User);
            var currUser = Context.Employees.FirstOrDefault(e => e.Id == currId);

            if (currUser == null)
            {
                return Forbid($"Could not find a user with the id: {currId}");
            }

            return Page();
        }

        [BindProperty]
        public Job Job { get; set; }

        public string OwnerID { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (Job.EmployeeID == null
             || Job.JobName == null
             || Job.JobDescription == null)
            {
                return Page();
            }

            Job.OwnerID = UserManager.GetUserId(User);
            Job.DateCreated = DateTime.Now;
            Job.JobStatus = JobStatus.Unread;
            Job.TeamID = Context.Employees.FirstOrDefault(
                e => e.Id == Job.EmployeeID).TeamID;

            Context.Jobs.Add(Job);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
