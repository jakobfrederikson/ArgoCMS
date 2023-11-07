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

namespace ArgoCMS.Pages.Admin.TeamProjects
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
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

            var teamproject =  await _context.TeamProjects.FirstOrDefaultAsync(m => m.TeamId == id);
            if (teamproject == null)
            {
                return NotFound();
            }
            TeamProject = teamproject;
           ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "OwnerID");
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

            _context.Attach(TeamProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamProjectExists(TeamProject.TeamId))
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

        private bool TeamProjectExists(int id)
        {
          return (_context.TeamProjects?.Any(e => e.TeamId == id)).GetValueOrDefault();
        }
    }
}
