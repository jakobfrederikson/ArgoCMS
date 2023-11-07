using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;
using ArgoCMS.Models.JointEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ArgoCMS.Pages.Projects
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

        public List<EmployeeProject> EmployeeProjects { get; set; }
            = new List<EmployeeProject>();

        public async Task OnGetAsync()
        {
            if (Context.Projects != null)
            {
                var employeeId = UserManager.GetUserId(User);

                var employeeProjects = await Context.EmployeesProjects
                    .Include(et => et.Project)
                    .Where(et => et.EmployeeId == employeeId)
                    .ToListAsync();

                if (employeeProjects != null)
                {
                    EmployeeProjects = employeeProjects;
                }
            }
        }
    }
}
