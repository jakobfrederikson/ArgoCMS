using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeNotificationGroup
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
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

            var employeenotificationgroup = await _context.EmployeeNotificationGroups.FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employeenotificationgroup == null)
            {
                return NotFound();
            }
            else 
            {
                EmployeeNotificationGroup = employeenotificationgroup;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.EmployeeNotificationGroups == null)
            {
                return NotFound();
            }
            var employeenotificationgroup = await _context.EmployeeNotificationGroups.FindAsync(id);

            if (employeenotificationgroup != null)
            {
                EmployeeNotificationGroup = employeenotificationgroup;
                _context.EmployeeNotificationGroups.Remove(EmployeeNotificationGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
