using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Projects
{
    [Authorize(Roles = "Administrators")]
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
        public Project Project { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (Project.ProjectName == null ||
                Project.ProjectDescription == null ||
                Project.DueDate == DateTime.MinValue)
            {
                return Page();
            }

            var currentUser = await UserManager.GetUserAsync(User);
            if(currentUser == null)
            {
                return Page();
            }

            Project.OwnerID = currentUser.Id;
            Project.DateCreated = DateTime.UtcNow;
            Project.ProjectStatus = ProjectStatus.NotStarted;

            Context.Projects.Add(Project);
            await Context.SaveChangesAsync();

            // Create EmployeeProject entry.
            Context.EmployeesProjects.Add(new EmployeeProject
            {
                EmployeeId = currentUser.Id,
                ProjectId = Project.ProjectId
            });

            Context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
