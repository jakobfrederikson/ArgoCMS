using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArgoCMS.Data;
using ArgoCMS.Models.Notifications;

namespace ArgoCMS.Pages.Admin.NotificationGroups
{
    public class DetailsModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public DetailsModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public NotificationGroup NotificationGroup { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.NotificationGroups == null)
            {
                return NotFound();
            }

            var notificationgroup = await _context.NotificationGroups.FirstOrDefaultAsync(m => m.Id == id);
            if (notificationgroup == null)
            {
                return NotFound();
            }
            else 
            {
                NotificationGroup = notificationgroup;
            }
            return Page();
        }
    }
}
