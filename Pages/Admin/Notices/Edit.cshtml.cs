using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models;

namespace ArgoCMS.Pages.Admin.Notices
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Notice Notice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notice =  await _context.Notices.FirstOrDefaultAsync(m => m.NoticeId == id);
            if (notice == null)
            {
                return NotFound();
            }
            Notice = notice;
           ViewData["OwnerID"] = new SelectList(_context.Employees, "Id", "Id");
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

            _context.Attach(Notice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeExists(Notice.NoticeId))
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

        private bool NoticeExists(int id)
        {
          return (_context.Notices?.Any(e => e.NoticeId == id)).GetValueOrDefault();
        }
    }
}
