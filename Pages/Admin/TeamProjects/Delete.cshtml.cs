using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.TeamProjects
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public TeamProject TeamProject { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TeamProjects == null)
            {
                return NotFound();
            }

            var teamproject = await _context.TeamProjects.FirstOrDefaultAsync(m => m.TeamId == id);

            if (teamproject == null)
            {
                return NotFound();
            }
            else 
            {
                TeamProject = teamproject;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TeamProjects == null)
            {
                return NotFound();
            }
            var teamproject = await _context.TeamProjects.FindAsync(id);

            if (teamproject != null)
            {
                TeamProject = teamproject;
                _context.TeamProjects.Remove(TeamProject);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
