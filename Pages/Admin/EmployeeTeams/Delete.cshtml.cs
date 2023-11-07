using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeTeams
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public EmployeeTeam EmployeeTeam { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.EmployeeTeams == null)
            {
                return NotFound();
            }

            var employeeteam = await _context.EmployeeTeams.FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employeeteam == null)
            {
                return NotFound();
            }
            else 
            {
                EmployeeTeam = employeeteam;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.EmployeeTeams == null)
            {
                return NotFound();
            }
            var employeeteam = await _context.EmployeeTeams.FindAsync(id);

            if (employeeteam != null)
            {
                EmployeeTeam = employeeteam;
                _context.EmployeeTeams.Remove(EmployeeTeam);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
