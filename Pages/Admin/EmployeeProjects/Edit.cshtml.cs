using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeProjects
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
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

            var employeeproject =  await _context.EmployeesProjects.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeproject == null)
            {
                return NotFound();
            }
            EmployeeProject = employeeproject;
           ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
           ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "OwnerID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EmployeeProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeProjectExists(EmployeeProject.EmployeeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EmployeeProjectExists(string id)
        {
          return (_context.EmployeesProjects?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
