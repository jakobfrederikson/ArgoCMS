﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Models;

namespace ArgoCMS.Pages.Notices
{
    public class DeleteModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DeleteModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Notice Notice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.FirstOrDefaultAsync(m => m.NoticeId == id);

            if (notice == null)
            {
                return NotFound();
            }
            else 
            {
                Notice = notice;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }
            var notice = await _context.Notices.FindAsync(id);
            var noticeNotification = await _context.NoticeNotifications
                .FirstOrDefaultAsync(nn => nn.ObjectId == id.ToString());

            if (notice != null && noticeNotification != null)
            {
                Notice = notice;
                _context.Notices.Remove(Notice);
                _context.NoticeNotifications.Remove(noticeNotification);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
