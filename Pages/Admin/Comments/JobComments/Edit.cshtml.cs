using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Admin.Comments.JobComments
{
    public class EditModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public EditModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public JobComment JobComment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.JobComments == null)
            {
                return NotFound();
            }

            var jobcomment =  await _context.JobComments.FirstOrDefaultAsync(m => m.CommentId == id);
            if (jobcomment == null)
            {
                return NotFound();
            }
            JobComment = jobcomment;
           ViewData["ParentId"] = new SelectList(_context.Jobs, "JobId", "AssignedEmployeeID");
           ViewData["OwnerID"] = new SelectList(_context.Employees, "Id", "Id");
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

            _context.Attach(JobComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobCommentExists(JobComment.CommentId))
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

        private bool JobCommentExists(int id)
        {
          return (_context.JobComments?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    }
}
