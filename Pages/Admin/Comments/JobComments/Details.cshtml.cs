using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Admin.Comments.JobComments
{
    public class DetailsModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DetailsModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public JobComment JobComment { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.JobComments == null)
            {
                return NotFound();
            }

            var jobcomment = await _context.JobComments.FirstOrDefaultAsync(m => m.CommentId == id);
            if (jobcomment == null)
            {
                return NotFound();
            }
            else 
            {
                JobComment = jobcomment;
            }
            return Page();
        }
    }
}
