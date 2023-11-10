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

namespace ArgoCMS.Pages.Admin.EmployeeNotificationGroup
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ArgoCMS.Models.JointEntities.EmployeeNotificationGroup EmployeeNotificationGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.EmployeeNotificationGroups == null)
            {
                return NotFound();
            }

            var employeenotificationgroup =  await _context.EmployeeNotificationGroups.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeenotificationgroup == null)
            {
                return NotFound();
            }
            EmployeeNotificationGroup = employeenotificationgroup;
           ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
           ViewData["NotificationGroupId"] = new SelectList(_context.NotificationGroups, "Id", "Id");
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

            _context.Attach(EmployeeNotificationGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeNotificationGroupExists(EmployeeNotificationGroup.EmployeeId))
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

        private bool EmployeeNotificationGroupExists(string id)
        {
          return (_context.EmployeeNotificationGroups?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
