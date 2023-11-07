using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Comments;

namespace ArgoCMS.Pages.Admin.Comments.NoticeComments
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public NoticeComment NoticeComment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.NoticeComments == null)
            {
                return NotFound();
            }

            var noticecomment = await _context.NoticeComments.FirstOrDefaultAsync(m => m.CommentId == id);

            if (noticecomment == null)
            {
                return NotFound();
            }
            else 
            {
                NoticeComment = noticecomment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.NoticeComments == null)
            {
                return NotFound();
            }
            var noticecomment = await _context.NoticeComments.FindAsync(id);

            if (noticecomment != null)
            {
                NoticeComment = noticecomment;
                _context.NoticeComments.Remove(NoticeComment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
