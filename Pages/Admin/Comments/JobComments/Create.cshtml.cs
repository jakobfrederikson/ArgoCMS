using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Admin.Comments.JobComments
{
    public class CreateModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public CreateModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ParentId"] = new SelectList(_context.Jobs, "JobId", "AssignedEmployeeID");
        ViewData["OwnerID"] = new SelectList(_context.Employees, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public JobComment JobComment { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.JobComments == null || JobComment == null)
            {
                return Page();
            }

            _context.JobComments.Add(JobComment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
