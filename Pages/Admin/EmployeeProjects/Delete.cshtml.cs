using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeProjects
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public EmployeeProject EmployeeProject { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.EmployeesProjects == null)
            {
                return NotFound();
            }

            var employeeproject = await _context.EmployeesProjects.FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employeeproject == null)
            {
                return NotFound();
            }
            else 
            {
                EmployeeProject = employeeproject;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.EmployeesProjects == null)
            {
                return NotFound();
            }
            var employeeproject = await _context.EmployeesProjects.FindAsync(id);

            if (employeeproject != null)
            {
                EmployeeProject = employeeproject;
                _context.EmployeesProjects.Remove(EmployeeProject);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
