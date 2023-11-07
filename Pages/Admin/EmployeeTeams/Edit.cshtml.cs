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

namespace ArgoCMS.Pages.Admin.EmployeeTeams
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
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

            var employeeteam =  await _context.EmployeeTeams.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeteam == null)
            {
                return NotFound();
            }
            EmployeeTeam = employeeteam;
           ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
           ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamDescription");
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

            _context.Attach(EmployeeTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeTeamExists(EmployeeTeam.EmployeeId))
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

        private bool EmployeeTeamExists(string id)
        {
          return (_context.EmployeeTeams?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
